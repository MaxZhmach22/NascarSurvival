using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NascarSurvival.Collectable;
using UniRx;
using UnityEngine;
using Zenject;


namespace NascarSurvival
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(HeroSettings))]
    public class HeroInitializer : MonoBehaviour, IInitializer
    {
        public RaceMovement RaceMovement { get; private set; }
        private HeroSettings _heroSettings;
        private IMoveController _dynamicJoystick;
        private GameStateHandler _gameStateHandler;
        private FinishZone _finishZone;
        private GameUI _gameUI;
        private CollectablesSpawner collectablesSpawner;
        private int _previousValue;

        [Inject]
        private void SetReferences(
            GameStateHandler gameStateHandler, 
            IMoveController dynamicJoystick, 
            HeroSettings heroSettings,
            FinishZone finishZone,
            CollectablesSpawner collectablesSpawner,
            GameUI gameUI)
        {
            _gameStateHandler = gameStateHandler;
            _dynamicJoystick = dynamicJoystick;
            _heroSettings = heroSettings;
            _finishZone = finishZone;
            this.collectablesSpawner = collectablesSpawner;
            _gameUI = gameUI;
        }
        
        private void Start()
        {
            RaceMovement = new RaceMovement(_dynamicJoystick, _gameStateHandler, _heroSettings, _finishZone);
            

            CreateSequence();
            UiSubcribe();
        }

        private void UiSubcribe()
        {
            RaceMovement.ObserveEveryValueChanged(x => x.CurrentSpeed)
                .Subscribe(x =>
                {
                    _gameUI.SpeedText.DOCounter(_previousValue, (int)x, 0.1f).SetEase(Ease.Linear);
                    _previousValue = (int)x;
                })
                .AddTo(this);

            RaceMovement.ObserveEveryValueChanged(x => x.StartCounter)
                .Select(x => (int) x)
                .Subscribe( async counter =>
                {
                    if (counter > 0)
                    {
                        _gameUI.StartCounter.DOCounter(counter, counter - 1, 0.3f).SetEase(Ease.Linear);
                    }
                    else
                    {
                        _gameUI.StartCounter.text = "GO!";
                        await UniTask.Delay(TimeSpan.FromSeconds(1),
                            cancellationToken: this.GetCancellationTokenOnDestroy());
                        _gameUI.StartCounter.gameObject.SetActive(false);
                    }
                    
                })
                .AddTo(this);


        }

        private void CreateSequence()
        {
            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Take(1)
                .DoOnTerminate(() => Debug.Log("Game is Started!"))
                .Subscribe(_ => _gameStateHandler.ChangeState(GameStates.Start))
                .AddTo(this);
        }

        private void OnDestroy()
        {
            collectablesSpawner.Dispose();
            RaceMovement.Dispose();
        }
    }
}
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
        public string Name => gameObject.name;
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
            
            CreateSequence();
        }

        private void CounterUi()
        {
            RaceMovement.ObserveEveryValueChanged(x => x.StartCounter)
                .Skip(1)
                .Select(x => (int) x)
                .Subscribe(async counter =>
                {
                    if (counter > 0)
                    {
                        _gameUI.GameMessages.DOCounter(counter, counter - 1, 0.3f).SetEase(Ease.Linear);
                    }
                    else
                    {
                        _gameUI.GameMessages.text = "GO!";
                        await UniTask.Delay(TimeSpan.FromSeconds(1),
                            cancellationToken: this.GetCancellationTokenOnDestroy());
                        _gameUI.GameMessages.gameObject.SetActive(false);
                    }
                })
                .AddTo(this);
        }

        private void CreateSequence()
        {
            var startFontSize = _gameUI.GameMessages.fontSize;
            
            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Take(1)
                .DoOnSubscribe(() =>
                {
                    _gameUI.GameMessages.fontSize /= 2;
                    _gameUI.GameMessages.text = "Tap to start!";
                })
                .DoOnTerminate(() =>
                {
                    _gameUI.GameMessages.text = "";
                    _gameUI.GameMessages.fontSize = startFontSize;
                    CounterUi();
                })
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
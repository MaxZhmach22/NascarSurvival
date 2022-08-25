using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

namespace NascarSurvival
{
    public class EnemyInitializer : MonoBehaviour
    {
        private RaceMovement raceMovement;
        private HeroSettings _heroSettings;
        private DynamicJoystick _dynamicJoystick;
        private GameStateHandler _gameStateHandler;
        private FinishZone _finishZone;
        private GameUI _gameUI;
        private int _previousValue;

        // [Inject]
        // private void SetReferences(
        //     GameStateHandler gameStateHandler, 
        //     DynamicJoystick dynamicJoystick, 
        //     HeroSettings heroSettings,
        //     FinishZone finishZone,
        //     GameUI gameUI)
        // {
        //     _gameStateHandler = gameStateHandler;
        //     _dynamicJoystick = dynamicJoystick;
        //     _heroSettings = heroSettings;
        //     _finishZone = finishZone;
        //     _gameUI = gameUI;
        // }
        
        private void Start()
        {
            raceMovement = new RaceMovement(_dynamicJoystick, _gameStateHandler, _heroSettings, _finishZone);
            

            CreateSequence();
            UiSubcribe();
        }

        private void UiSubcribe()
        {
            raceMovement.ObserveEveryValueChanged(x => x.CurrentSpeed)
                .Select(x => (int) x)
                .Subscribe(x =>
                {
                    _gameUI.SpeedText.DOCounter(_previousValue, x, 0.1f).SetEase(Ease.Linear);
                    _previousValue = x;
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
            raceMovement.Dispose();
        }
    }
}
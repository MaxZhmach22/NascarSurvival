using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace NascarSurvival
{
    public class HeroMovement : IDisposable
    {
        private GameStateHandler _gameStateHandler;
        private DynamicJoystick _dynamicJoystick;
        private CompositeDisposable _disposable = new CompositeDisposable();
        
        public HeroMovement(DynamicJoystick dynamicJoystick, GameStateHandler gameStateHandler)
        {
            _dynamicJoystick = dynamicJoystick;
            _gameStateHandler = gameStateHandler;


            Debug.Log(_dynamicJoystick);
            Debug.Log(GameStateHandler.Counter);

            Observable.EveryUpdate()
                .Where(_ => _gameStateHandler.CurrentGameState == GameStates.Start)
                .Subscribe(_ => Debug.Log("Moving"))
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable.Clear();
        }
    }
}
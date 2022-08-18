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
        private HeroSettings _heroSettings;
        private float _startSpeed;
 

        public HeroMovement(DynamicJoystick dynamicJoystick, GameStateHandler gameStateHandler, HeroSettings heroSettings)
        {
            _dynamicJoystick = dynamicJoystick;
            _gameStateHandler = gameStateHandler;
            _heroSettings = heroSettings;

            AccelerationSequence();
        }

        private void AccelerationSequence()
        {
            Observable.EveryUpdate()
                .Where(_ => _gameStateHandler.CurrentGameState == GameStates.Start)
                .TakeWhile(_ => _startSpeed < _heroSettings.ClampConstantSpeed)
                .DoOnTerminate(() =>
                {
                    ConstantMovementSequence();
                    Debug.Log("<color=red>Accelerated</color>");
                })
                .Subscribe(_ => StartAccelerationMovement())
                .AddTo(_disposable);
        }
        
        private void ConstantMovementSequence()
        {
            Observable.EveryUpdate()
                .Where(_ => _gameStateHandler.CurrentGameState == GameStates.Start)
                .Subscribe(_ => ConstantForwardMovement())
                .AddTo(_disposable);
        }

        private void StartAccelerationMovement()
        { 
            _startSpeed += Time.deltaTime * _heroSettings.StartAccelerationTime;
            _startSpeed = Mathf.Clamp(_startSpeed, 0, _heroSettings.ClampConstantSpeed);
            _heroSettings.transform.position += Vector3.forward * _startSpeed * Time.deltaTime;
        }
        
        private void ConstantForwardMovement()
        {
            var constantForwardVector = Vector3.forward * _startSpeed;
            var joystickAddition = new Vector3(_dynamicJoystick.Horizontal,0, _dynamicJoystick.Vertical) * _heroSettings.Speed;
            _heroSettings.transform.position += (constantForwardVector + joystickAddition) * Time.deltaTime;
        }

        public void Dispose()
        {
            _disposable.Clear();
        }
    }
}
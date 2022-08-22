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
        private FinishZone _finisZone;
        private float _startSpeed;
 

        public HeroMovement(DynamicJoystick dynamicJoystick, 
            GameStateHandler gameStateHandler, 
            HeroSettings heroSettings, 
            FinishZone finisZone)
        {
            _dynamicJoystick = dynamicJoystick;
            _gameStateHandler = gameStateHandler;
            _heroSettings = heroSettings;
            _finisZone = finisZone;

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
                .TakeWhile(_ => _heroSettings.transform.position.z < _finisZone.transform.position.z)
                .DoOnTerminate(() => DecelerationMovement())
                .Subscribe(_ => ConstantForwardMovement())
                .AddTo(_disposable);
        }

        private void DecelerationMovement()
        {
            Observable.EveryUpdate()
                .TakeWhile(_ => _startSpeed > 0)
                .DoOnTerminate(() =>
                {
                    Debug.Log("<color=red>Stopped!</color>");
                })
                .Subscribe(_ => StartDecelerationMovement())
                .AddTo(_disposable);
        }

        private void StartAccelerationMovement()
        { 
            _startSpeed += Time.deltaTime * _heroSettings.StartAccelerationTime;
            _startSpeed = Mathf.Clamp(_startSpeed, 0, _heroSettings.ClampConstantSpeed);
            _heroSettings.transform.position += Vector3.forward * _startSpeed * Time.deltaTime;
        }
        
        private void StartDecelerationMovement()
        { 
            _startSpeed -= Time.deltaTime * _heroSettings.StartAccelerationTime;
            _startSpeed = Mathf.Clamp(_startSpeed, 0, _heroSettings.ClampConstantSpeed);
            _heroSettings.transform.position += Vector3.forward * _startSpeed * Time.deltaTime;
        }
        
        private void ConstantForwardMovement()
        {
            var constantForwardVector = Vector3.forward * _startSpeed;
            var joystickAddition = new Vector3(_dynamicJoystick.Horizontal,0, _dynamicJoystick.Vertical) * _heroSettings.Speed;
            _heroSettings.transform.position += (constantForwardVector + joystickAddition) * Time.deltaTime;
            var clampBorders = Mathf.Clamp(_heroSettings.transform.position.x, -5, 5);
            _heroSettings.transform.position = new Vector3(clampBorders, _heroSettings.transform.position.y,
                _heroSettings.transform.position.z);
        }

        public void Dispose()
        {
            _disposable.Clear();
        }
    }
}
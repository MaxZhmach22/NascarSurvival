using System;
using UniRx;
using UnityEngine;


namespace NascarSurvival
{
    public class HeroMovement : IDisposable
    {
        private GameStateHandler _gameStateHandler;
        private DynamicJoystick _dynamicJoystick;
        private CompositeDisposable _disposable = new CompositeDisposable();
        private HeroSettings _heroSettings;
        private FinishZone _finisZone;
        private float _currentSpeed;
        private bool _bonusSpeedEffect;


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


            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.A))
                .Subscribe(_ => BonusSpeedEffect(10, 3, 5))
                .AddTo(_disposable);
        }

        private void AccelerationSequence()
        {
            Observable.EveryUpdate()
                .Where(_ => _gameStateHandler.CurrentGameState == GameStates.Start)
                .TakeWhile(_ => _currentSpeed < _heroSettings.StartSpeedToAccelerate)
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
                .Where(_ => !_bonusSpeedEffect)
                .TakeWhile(_ => _heroSettings.transform.position.z < _finisZone.transform.position.z)
                .DoOnTerminate(() => DecelerationMovement())
                .Subscribe(_ => ConstantForwardMovement())
                .AddTo(_disposable);
        }

        private void DecelerationMovement()
        {
            Observable.EveryUpdate()
                .TakeWhile(_ => _currentSpeed > 0)
                .DoOnTerminate(() =>
                {
                    Debug.Log("<color=red>Stopped!</color>");
                })
                .Subscribe(_ => StartDecelerationMovement())
                .AddTo(_disposable);
        }

        private void StartAccelerationMovement()
        { 
            _currentSpeed += Time.deltaTime * _heroSettings.StartAccelerationTime;
            _currentSpeed = Mathf.Clamp(_currentSpeed, 0, _heroSettings.StartSpeedToAccelerate);
            _heroSettings.transform.position += Vector3.forward * _currentSpeed * Time.deltaTime;
        }
        
        private void StartDecelerationMovement()
        { 
            _currentSpeed -= Time.deltaTime * _heroSettings.StartAccelerationTime;
            _currentSpeed = Mathf.Clamp(_currentSpeed, 0, _heroSettings.StartSpeedToAccelerate);
            _heroSettings.transform.position += Vector3.forward * _currentSpeed * Time.deltaTime;
        }
        
        private void ConstantForwardMovement()
        {
            var constantForwardVector = Vector3.forward * _currentSpeed;
            var joystickAddition = new Vector3(_dynamicJoystick.Horizontal,0, _dynamicJoystick.Vertical) * _heroSettings.Speed;
            _heroSettings.transform.position += (constantForwardVector + joystickAddition) * Time.deltaTime;
            var clampBorders = Mathf.Clamp(_heroSettings.transform.position.x, -5, 5);
            _heroSettings.transform.position = new Vector3(clampBorders, _heroSettings.transform.position.y,
                _heroSettings.transform.position.z);
        }

        public void BonusSpeedEffect(float bonusToSpeed, float acceletartionTime, float activeTime)
        {
            Observable
                .Timer(TimeSpan.FromSeconds(acceletartionTime))
                .DoOnSubscribe(() => Debug.Log("Bonus activated"))
                .DoOnTerminate(() =>
                {
                    Debug.Log("acceletartionTime");
                    
                    Observable.Timer
                        (TimeSpan.FromSeconds(activeTime))
                        .DoOnTerminate(() => Debug.Log("Bonus end"))
                        .Subscribe(_ => Debug.Log(activeTime))
                        .AddTo(_disposable);
                    
                })
                .Subscribe(_ => Debug.Log(""))
                .AddTo(_disposable);
            
           
        }

        public void Dispose()
        {
            _disposable.Clear();
        }
    }
}
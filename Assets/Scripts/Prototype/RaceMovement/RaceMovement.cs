using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;


namespace NascarSurvival
{
    public class RaceMovement : IDisposable
    {
        public float CurrentSpeed => _lengthOfTheVector;
        
        private GameStateHandler _gameStateHandler;
        private IMoveController _dynamicJoystick;
        private CompositeDisposable _disposable = new CompositeDisposable();
        private CancellationTokenSource _cancellationToken = new CancellationTokenSource();
        private HeroSettings _heroSettings;
        private FinishZone _finisZone;
        private float _currentSpeed;
        private float _lengthOfTheVector;
        private bool _bonusSpeedEffect;


        public RaceMovement(IMoveController dynamicJoystick, 
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
                .Subscribe(_ => BonusSpeedEffect(5, 5, 5))
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
            var joystickAddition = new Vector3(_dynamicJoystick.Movement.x,0, _dynamicJoystick.Movement.y) * _heroSettings.Speed;
            var vectorToAdd = (constantForwardVector + joystickAddition);
            _heroSettings.transform.position += vectorToAdd * Time.deltaTime;
            var clampBorders = Mathf.Clamp(_heroSettings.transform.position.x, -5, 5);
            _heroSettings.transform.position = new Vector3(clampBorders, _heroSettings.transform.position.y,
                _heroSettings.transform.position.z);

            _lengthOfTheVector = vectorToAdd.sqrMagnitude;
        }

        public async void BonusSpeedEffect(float bonusToSpeed, float acceletartionTime, float activeTime)
        {
            var startSpeed = _currentSpeed;
            var allSpeed = _currentSpeed + bonusToSpeed;
            
            DOTween.To(() => _currentSpeed, x => _currentSpeed = x, allSpeed, acceletartionTime)
                .SetEase(Ease.Linear)
                .OnStart(() =>
                {
                    _bonusSpeedEffect = true;
                    Debug.Log("Start accelerate");
                })
                .OnComplete(() => Debug.Log(_currentSpeed));
            
            await UniTask.Delay(TimeSpan.FromSeconds(activeTime + acceletartionTime), cancellationToken: _cancellationToken.Token);
            
            DOTween.To(() => _currentSpeed, x => _currentSpeed = x, startSpeed, acceletartionTime)
                .SetEase(Ease.Linear)
                .OnStart(() => Debug.Log("Start deccelerate"))
                .OnComplete(() =>
                {
                    _bonusSpeedEffect = false;
                    Debug.Log(_currentSpeed);
                });
        }

        public void Dispose()
        {
            _cancellationToken.Cancel();
            _disposable.Clear();
        }
    }
}
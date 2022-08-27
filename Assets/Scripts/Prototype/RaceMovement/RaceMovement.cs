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

        private float _currentSpeed;
        private float _lengthOfTheVector;
        private bool _bonusSpeedEffect;
        private readonly GameStateHandler _gameStateHandler;
        private readonly IMoveController _movingController;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();
        private readonly ObjectSettings _objectSettings;
        private readonly FinishZone _finisZone;


        public RaceMovement(IMoveController movingController, 
            GameStateHandler gameStateHandler, 
            ObjectSettings objectSettings, 
            FinishZone finisZone)
        {
            _movingController = movingController;
            _gameStateHandler = gameStateHandler;
            _objectSettings = objectSettings;
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
                .TakeWhile(_ => _currentSpeed < _objectSettings.StartSpeedToAccelerate)
                .DoOnTerminate(() =>
                {
                    ConstantMovementSequence();
                    // Debug.Log("<color=red>Accelerated</color>");
                })
                .Subscribe(_ => StartAccelerationMovement())
                .AddTo(_disposable);
        }
        
        private void ConstantMovementSequence()
        {
            Observable.EveryUpdate()
                .TakeWhile(_ => _objectSettings.transform.position.z < _finisZone.transform.position.z)
                .DoOnTerminate(() =>
                {
                    EndGame();
                    
                })
                .Subscribe(_ => ConstantForwardMovement())
                .AddTo(_disposable);
        }

        private void EndGame()
        {
            DecelerationMovement();
            var state = _movingController is DynamicJoystick ? GameStates.Finish : GameStates.Defeat;
            _gameStateHandler.ChangeState(state);
        }

        private void DecelerationMovement()
        {
            Observable.EveryUpdate()
                .TakeWhile(_ => _currentSpeed > 0)
                .DoOnTerminate(() =>
                {
                    // Debug.Log("<color=red>Stopped!</color>");
                })
                .Subscribe(_ => StartDecelerationMovement())
                .AddTo(_disposable);
        }

        private void StartAccelerationMovement()
        { 
            _currentSpeed += Time.deltaTime * _objectSettings.StartAccelerationTime;
            _currentSpeed = Mathf.Clamp(_currentSpeed, 0, _objectSettings.StartSpeedToAccelerate);
            _objectSettings.transform.position += Vector3.forward * _currentSpeed * Time.deltaTime;
        }
        
        private void StartDecelerationMovement()
        { 
            _currentSpeed -= Time.deltaTime * _objectSettings.StartAccelerationTime;
            _currentSpeed = Mathf.Clamp(_currentSpeed, 0, _objectSettings.StartSpeedToAccelerate);
            _objectSettings.transform.position += Vector3.forward * _currentSpeed * Time.deltaTime;
        }
        
        private void ConstantForwardMovement()
        {
            var constantForwardVector = Vector3.forward * _currentSpeed;
            var joystickAddition = new Vector3(_movingController.Movement.x,0, _movingController.Movement.y) * _objectSettings.Speed;
            var vectorToAdd = (constantForwardVector + joystickAddition);
            _objectSettings.transform.position += vectorToAdd * Time.deltaTime;
            var clampBorders = Mathf.Clamp(_objectSettings.transform.position.x, -5, 5);
            _objectSettings.transform.position = new Vector3(clampBorders, _objectSettings.transform.position.y,
                _objectSettings.transform.position.z);

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
                    // Debug.Log("Start accelerate");
                })
                .OnComplete(() => Debug.Log(_currentSpeed));
            
            await UniTask.Delay(TimeSpan.FromSeconds(activeTime + acceletartionTime), cancellationToken: _cancellationToken.Token);
            
            DOTween.To(() => _currentSpeed, x => _currentSpeed = x, startSpeed, acceletartionTime)
                .SetEase(Ease.Linear)
                // .OnStart(() => Debug.Log("Start deccelerate"))
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
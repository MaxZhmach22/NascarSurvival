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
        public int CurrentSpeed => _lengthOfTheVector;

        private float _constantForwardSpeed;
        private float _currentSpeed;
      
        private int _lengthOfTheVector;
        private bool _bonusSpeedEffect;
        private Vector3 _acceleratedVector;
        private readonly GameStateHandler _gameStateHandler;
        private readonly IMoveController _movingController;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();
        private readonly ObjectSettings _objectSettings;
        private readonly FinishZone _finisZone;
        private float _startSpeedScalar;


        public RaceMovement(IMoveController movingController, 
            GameStateHandler gameStateHandler, 
            ObjectSettings objectSettings, 
            FinishZone finisZone)
        {
            _movingController = movingController;
            _gameStateHandler = gameStateHandler;
            _objectSettings = objectSettings;
            _finisZone = finisZone;
            _currentSpeed = _objectSettings.SensitiveXaxes;
            
            AccelerationSequence();
            
            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.A))
                .Subscribe(_ => BonusSpeedEffect(5, 5, 5))
                .AddTo(_disposable);
        }

        private void AccelerationSequence()
        {
            DOTween.To(() => _constantForwardSpeed, x => _constantForwardSpeed = x, _objectSettings.StartSpeedToAccelerate,
                    _objectSettings.StartAccelerationTime)
                .SetEase(Ease.Linear)
                .OnUpdate(() => StartAccelerationMovement())
                .OnComplete(() =>
                {
                    Debug.Log(_constantForwardSpeed);
                    ConstantMovementSequence();
                });
                
                
            //     
            // Observable.EveryUpdate()
            //     .Where(_ => _gameStateHandler.CurrentGameState == GameStates.Start)
            //     .TakeWhile(_ => _currentSpeed < _objectSettings.StartSpeedToAccelerate)
            //     .DoOnTerminate(() =>
            //     {
            //         ConstantMovementSequence();
            //         // Debug.Log("<color=red>Accelerated</color>");
            //     })
            //     .Subscribe(_ => StartAccelerationMovement())
            //     .AddTo(_disposable);
        }
        
        private void ConstantMovementSequence()
        {
            Observable.EveryFixedUpdate()
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
                .TakeWhile(_ => _constantForwardSpeed > 0)
                .DoOnTerminate(() =>
                {
                    // Debug.Log("<color=red>Stopped!</color>");
                })
                .Subscribe(_ => StartDecelerationMovement())
                .AddTo(_disposable);
        }

        private void StartAccelerationMovement()
        {
            // _acceleratedVector = Vector3.forward * 2 * _constantForwardSpeed * Time.deltaTime;
            // _objectSettings.transform.position += _acceleratedVector;
            //_lengthOfTheVector = _acceleratedVector.z * 500;
        }
        
        private void StartDecelerationMovement()
        {
            _constantForwardSpeed -= Time.deltaTime * _objectSettings.StartAccelerationTime;
            _constantForwardSpeed = Mathf.Clamp(_constantForwardSpeed, 0, _objectSettings.StartSpeedToAccelerate);
            _objectSettings.transform.position +=_objectSettings.transform.forward + Vector3.forward * _constantForwardSpeed * Time.deltaTime;
        }
        
        private void ConstantForwardMovement()
        {
            var forwardControllerVector = Vector3.zero;
            var rotation = Vector3.zero;
            
            if (_movingController is DynamicJoystick)
            {
                forwardControllerVector =
                    new Vector3(0, 0, _movingController.Movement.y) * _currentSpeed * Time.deltaTime;
                rotation = new Vector3(_movingController.Movement.x, 0, 0) * _objectSettings.SensitiveYaxes *
                           Time.deltaTime;
                _objectSettings.transform.position += rotation + forwardControllerVector * _startSpeedScalar;
            }
            else
            {
                forwardControllerVector = _objectSettings.transform.forward * _currentSpeed * Time.deltaTime;
                _objectSettings.transform.position += forwardControllerVector * _startSpeedScalar;
            }
  
            var clampBorders = Mathf.Clamp(_objectSettings.transform.position.x, -5, 5);
            _objectSettings.transform.position = new Vector3(clampBorders, _objectSettings.transform.position.y,
                _objectSettings.transform.position.z);

            //_lengthOfTheVector = forwardControllerVector.z + _acceleratedVector.z;
        }

        public async void BonusSpeedEffect(float bonusToSpeed, float acceletartionTime, float activeTime)
        {
            var startSpeed = _constantForwardSpeed;
            var allSpeed = _constantForwardSpeed + bonusToSpeed;
            
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
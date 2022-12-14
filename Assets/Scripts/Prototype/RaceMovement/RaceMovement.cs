using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;


namespace NascarSurvival
{
    public class RaceMovement : IDisposable
    {
        public float CurrentSpeed => _lengthOfTheVector;
        public float StartCounter => _startCounter;

        private float _startCounter;
        private float _currentSpeed;
      
        private float _lengthOfTheVector;
        private bool _bonusSpeedEffect;
        private Vector3 _acceleratedVector;
        private readonly GameUI _gameUI;
        private float _startSpeedScalar;
        private bool _isPlayerControl;
        private float _deadZone = 0.03f;
        private float _previousCurrentYAxes;
        private bool _isDotweening;
        private bool _isBoomed;
        private Transform _model;
        private Vector3 _playerRotation;
        private readonly GameStateHandler _gameStateHandler;
        private readonly IMoveController _movingController;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();
        private readonly ObjectSettings _objectSettings;
        private readonly FinishZone _finisZone;


        public RaceMovement(IMoveController movingController, 
            GameStateHandler gameStateHandler, 
            ObjectSettings objectSettings, 
            FinishZone finisZone, 
            GameUI gameUI)
        {
            _movingController = movingController;
            _gameStateHandler = gameStateHandler;
            _objectSettings = objectSettings;
            _finisZone = finisZone;
            _gameUI = gameUI;
            _currentSpeed = _objectSettings.StartSpeed;
            _startCounter = 4;
            _isPlayerControl = _movingController is DynamicJoystick;
            _model = _objectSettings.GetComponentsInChildren<Transform>().First(x => x.name.Contains("Model"));
            
            AccelerationSequence();

            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.A))
                .Subscribe(_ => BonusSpeedEffect(5, 5, 5))
                .AddTo(_disposable);
        }

        private void AccelerationSequence()
        {
            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Take(1)
                .Subscribe(_ =>
                {
                    DOTween.To(() => _startCounter, x => _startCounter = x, 0,
                            3)
                        .SetEase(Ease.Linear)
                        .OnUpdate(() => StartAccelerationMovement())
                        .OnComplete(() =>
                        {
                            ConstantMovementSequence();
                        });
                })
                .AddTo(_disposable);
        }
        
        private void ConstantMovementSequence()
        {
            Observable.EveryFixedUpdate()
                .TakeWhile(_ =>
                {
                    if (_objectSettings == null) return false;
                    
                    return _objectSettings.transform.position.z < _finisZone.transform.position.z;
                })
                .DoOnSubscribe(() => _gameUI.SoundHandler.Play("Engine03"))
                .DoOnTerminate(() =>
                {
                    EndGame();
                })
                .Subscribe(_ => ConstantForwardMovement())
                .AddTo(_disposable);
        }

        private void EndGame()
        {
            var state = _movingController is DynamicJoystick ? GameStates.Finish : GameStates.Defeat;
            if (state == GameStates.Finish && _isPlayerControl)
            {
                _gameUI.SoundHandler.Play("WinSound");
            }

            if (state == GameStates.Defeat && _isPlayerControl)
            {
                _gameUI.SoundHandler.Play("LooseSound");
            }
          
            _gameStateHandler.ChangeState(state);
        }

        private void DecelerationMovement()
        {
           EndGame();
        }

        private void StartAccelerationMovement()
        {
            // _acceleratedVector = Vector3.forward * 2 * _constantForwardSpeed * Time.deltaTime;
            // _objectSettings.transform.position += _acceleratedVector;
            //_lengthOfTheVector = _acceleratedVector.z * 500;
        }
        
        
        private void ConstantForwardMovement()
        {
            if (_isBoomed)  return;

            var forwardControllerVector = Vector3.zero;
            
            if (_isPlayerControl)
            {
                var currentXAxes = _movingController.Movement.y * _objectSettings.SensitiveYaxes;
                
                if (Mathf.Abs(currentXAxes - _previousCurrentYAxes) > _deadZone)
                {
                    if(_isDotweening) return;
                    
                    DOTween.To(() => _previousCurrentYAxes, x => _previousCurrentYAxes = x, currentXAxes, 0.5f)
                        .SetEase(Ease.Linear)
                        .SetUpdate(UpdateType.Fixed)
                        .OnStart(() => _isDotweening = true)
                        .OnUpdate(() =>
                        {
                            forwardControllerVector =
                                new Vector3(0, 0, 1f + _previousCurrentYAxes) * _currentSpeed * Time.deltaTime;
                            _playerRotation += new Vector3(_movingController.Movement.x, 0, 0) *
                                       _objectSettings.SensitiveXaxes *
                                       Time.deltaTime;
                            _objectSettings.transform.position += _playerRotation + forwardControllerVector;
                            _lengthOfTheVector = forwardControllerVector.z * 1000f;
                            _model.transform.rotation = Quaternion.Euler(new Vector3(0,_playerRotation.x * 800,0));
                        })
                        .OnComplete(() =>
                        {
                            _isDotweening = false;
                            _previousCurrentYAxes = currentXAxes;
                        });
                }
                else
                {
                    if(_isDotweening) return;
                    
                    forwardControllerVector =
                        new Vector3(0, 0, 1f + currentXAxes) * _currentSpeed * Time.deltaTime;
                    _playerRotation += new Vector3(_movingController.Movement.x, 0, 0) * _objectSettings.SensitiveXaxes *
                                      Time.deltaTime;
                    _objectSettings.transform.position += _playerRotation + forwardControllerVector;
                    _previousCurrentYAxes = currentXAxes;
                    _model.transform.rotation = Quaternion.Euler(new Vector3(0,_playerRotation.x * 800,0));
                }

                

            }
            else
            {
                _objectSettings.transform.forward = new Vector3(_objectSettings.transform.forward.x, 0, _objectSettings.transform.forward.z);
                forwardControllerVector = _objectSettings.transform.forward * _currentSpeed * Time.deltaTime;
                _objectSettings.transform.position += forwardControllerVector;
            }
  
            var clampBorders = Mathf.Clamp(_objectSettings.transform.position.x, -10, 10);
            _objectSettings.transform.position = new Vector3(clampBorders, _objectSettings.transform.position.y,
                _objectSettings.transform.position.z);

            _lengthOfTheVector = forwardControllerVector.z * 1000f;
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

        public async void BoomBonus(Vector3 position)
        {
            _isBoomed = true;
            var startPosition = _objectSettings.transform.position;
            var startRotation = _objectSettings.transform.rotation;
            var colliders = _objectSettings.GetComponentsInChildren<Collider>().ToList();
            var rigidBody = _objectSettings.GetComponentInChildren<Rigidbody>();
            
            rigidBody.isKinematic = false;
            colliders.ForEach(colliders => colliders.enabled = false);
            rigidBody.AddExplosionForce(20f, position, 5f, 10f,ForceMode.VelocityChange);
            rigidBody.AddTorque(new Vector3(Random.Range(0,100),0, Random.Range(0,100)), ForceMode.VelocityChange);

            await UniTask.Delay(TimeSpan.FromSeconds(3f), cancellationToken: _cancellationToken.Token);

            rigidBody.isKinematic = true;
            colliders.ForEach(colliders => colliders.enabled = true);
            _objectSettings.transform.position = startPosition;
            _objectSettings.transform.rotation = startRotation;
            _isBoomed = false;
        }
    }
}
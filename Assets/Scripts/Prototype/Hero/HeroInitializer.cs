using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace NascarSurvival
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(HeroSettings))]
    public class HeroInitializer : MonoBehaviour
    {
        private HeroMovement _heroMovement;
        private HeroSettings _heroSettings;
        private DynamicJoystick _dynamicJoystick;
        private GameStateHandler _gameStateHandler;

        [Inject]
        private void SetReferences(GameStateHandler gameStateHandler, DynamicJoystick dynamicJoystick)
        {
            _gameStateHandler = gameStateHandler;
            _dynamicJoystick = dynamicJoystick;
        }
        
        private void Start()
        {
            _heroMovement = new HeroMovement(_dynamicJoystick, _gameStateHandler);

            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.Space))
                .Subscribe(_ => _gameStateHandler.ChangeState(GameStates.Start))
                .AddTo(this);
        }

        private void OnDestroy()
        {
            _heroMovement.Dispose();
        }
    }
}
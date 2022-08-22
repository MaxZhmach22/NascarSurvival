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
        private FinishZone _finishZone;

        [Inject]
        private void SetReferences(
            GameStateHandler gameStateHandler, 
            DynamicJoystick dynamicJoystick, 
            HeroSettings heroSettings,
            FinishZone finishZone)
        {
            _gameStateHandler = gameStateHandler;
            _dynamicJoystick = dynamicJoystick;
            _heroSettings = heroSettings;
            _finishZone = finishZone;
        }
        
        private void Start()
        {
            _heroMovement = new HeroMovement(_dynamicJoystick, _gameStateHandler, _heroSettings, _finishZone);

            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Take(1)
                .DoOnTerminate(() => Debug.Log("Game is Started!"))
                .Subscribe(_ => _gameStateHandler.ChangeState(GameStates.Start))
                .AddTo(this);
        }

        private void OnDestroy()
        {
            _heroMovement.Dispose();
        }
    }
}
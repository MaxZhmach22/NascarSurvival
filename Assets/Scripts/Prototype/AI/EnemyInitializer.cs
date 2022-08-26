using Prototype.AI;
using UnityEngine;
using Zenject;


namespace NascarSurvival
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EnemySettings))]
    public class EnemyInitializer : MonoBehaviour, IInitializer
    {
        public RaceMovement RaceMovement { get; private set; }
        private EnemySettings _enemySettings;
        private AIMovementController _aiMoveController;
        private GameStateHandler _gameStateHandler;
        private AiBehaviour _aiBehaviour;
        private FinishZone _finishZone;
        
        [Inject]
        private void SetReferences(
            GameStateHandler gameStateHandler,
            FinishZone finishZone
        )
        {
            _gameStateHandler = gameStateHandler;
            _finishZone = finishZone;
        }

        private void Awake()
        {
            _enemySettings = GetComponent<EnemySettings>();
        }

        private void Start()
        {
            _aiMoveController = new AIMovementController();
            _aiBehaviour = new AiBehaviour(_enemySettings, _aiMoveController);
            RaceMovement = new RaceMovement(_aiMoveController, _gameStateHandler, _enemySettings, _finishZone);
        }
        
        private void CreateSequence()
        {
          
        }

        private void OnDestroy()
        {
            _aiBehaviour.Dispose();
            RaceMovement.Dispose();
        }
    }
}
using System;
using NascarSurvival;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Prototype.AI
{
    public class AiBehaviour : IDisposable 
    {
        private readonly EnemySettings _enemySettings;
        private readonly AIMovementController _aiMovementController;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public AiBehaviour(EnemySettings enemySettings, AIMovementController aiMovementController)
        {
            _enemySettings = enemySettings;
            _aiMovementController = aiMovementController;

            CreateSequence();
        }

        private void CreateSequence()
        {
            Observable.Timer(TimeSpan.FromSeconds(_enemySettings.TimeToSwitchBehaviour))
                .DoOnSubscribe(() => _aiMovementController.Movement = new Vector2(0, _enemySettings.MaxPowerOfCar / 100))
                .Subscribe(_ =>
                {
                    CheckPowerChances();
                    
                })
                .AddTo(_disposable);

        }

        private void CheckPowerChances()
        {
            if (TakeRandomBool(_enemySettings.ChanceToIncreasePower))
            {
                _aiMovementController.Movement += new Vector2(1, _enemySettings.ValueToIncrease);
                Debug.Log($"Speed increase");
            }
            else
            {
                _aiMovementController.Movement -= new Vector2(1, _enemySettings.ValueToIncrease);
                Debug.Log($"Speed decrease");
            }
        }

        private bool TakeRandomBool(int enemySettingsChanceToIncreasePower)
        {
            var boolArray = new bool[100];
            
            for (int i = 0; i < boolArray.Length; i++)
            {
                boolArray[i] = true;
                if (i >= enemySettingsChanceToIncreasePower)
                {
                    boolArray[i] = false;
                }
            }

            return boolArray[Random.Range(0, boolArray.Length - 1)];
        }
        
        public void Dispose()
        {
            _disposable.Clear();
        }
    }
}
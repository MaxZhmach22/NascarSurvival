using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NascarSurvival;
using NascarSurvival.Collectable;
using UniRx;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;


namespace Prototype.AI
{
    public class AiBehaviour : IDisposable
    {
        private readonly EnemySettings _enemySettings;
        private readonly AIMovementController _aiMovementController;
        private readonly CollectablesSpawner _collectableSpawner;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        

        public AiBehaviour(EnemySettings enemySettings, AIMovementController aiMovementController, CollectablesSpawner collectableSpawner)
        {
            _enemySettings = enemySettings;
            _aiMovementController = aiMovementController;
            _collectableSpawner = collectableSpawner;

            CreateSequence();
        }

        private void CreateSequence()
        {
            _aiMovementController.Movement = Vector2.up;
            
            _collectableSpawner.ObserveEveryValueChanged(bonus => bonus.AccelerateBonusList.Count)
                .Subscribe(_ => ChooseBonusPath())
                .AddTo(_disposable);
        }

        private void ChooseBonusPath()
        {
            var nearestBonus = FindNearestBonus(_collectableSpawner.AccelerateBonusList);
            
            if(nearestBonus == null) return;
            
            //Debug
            nearestBonus.GetComponent<Renderer>().material.color = Color.cyan;
            Debug.Log($"Nearest Bonus {nearestBonus}", nearestBonus);

            var direction = nearestBonus.transform.position - _enemySettings.transform.position;
            Debug.DrawRay(_enemySettings.transform.position, direction, Color.yellow,4f);
            //Debug.Break();
            
            if (TakeRandomBool(_enemySettings.ChanceToPickUpBonus))
            {
                _enemySettings.transform.forward = direction;
                
                //_aiMovementController.Movement = new Vector2(direction.x, direction.z).normalized;
                // Debug.Log($"Speed increase {_aiMovementController.Movement.y}");
            }
            else
            {
                Debug.Log($"<color=black>Nearest Bonus Ignored{nearestBonus}</color>", nearestBonus);
                // _aiMovementController.Movement -= new Vector2(0, _enemySettings.ValueToIncrease / 100f);
                // Debug.Log($"Speed decrease {_aiMovementController.Movement.y}");
            }
        }

        private AccelerateBonus FindNearestBonus(List<AccelerateBonus> collectableSpawnerAccelerateBonusList)
        {
            var bonus = collectableSpawnerAccelerateBonusList.FirstOrDefault();
            if(bonus == null) return null;
            
            var firstBonusDistance = Vector3.Distance(bonus.transform.position, _enemySettings.transform.position);
            
            collectableSpawnerAccelerateBonusList.ForEach(x =>
            {
                var distance = Vector3.Distance(x.transform.position, _enemySettings.transform.position);
                if (distance < firstBonusDistance)
                {
                    bonus = x;
                }
            });

            if (_enemySettings.transform.position.z > bonus.transform.position.z)
            {
                return null;
            }
            
            return bonus;
        }

        private void CheckPowerChances()
        {
            if (TakeRandomBool(_enemySettings.ChanceToIncreasePower))
            {
                _aiMovementController.Movement += new Vector2(0, _enemySettings.ValueToIncrease / 100f);
                //Debug.Log($"Speed increase {_aiMovementController.Movement.y}");
            }
            else
            {
                _aiMovementController.Movement -= new Vector2(0, _enemySettings.ValueToIncrease / 100f);
                //Debug.Log($"Speed decrease {_aiMovementController.Movement.y}");
            }
        }

        private bool TakeRandomBool(int chanceValue)
        {
            var boolArray = new bool[100];
            
            for (int i = 0; i < boolArray.Length; i++)
            {
                boolArray[i] = true;
                if (i >= chanceValue)
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
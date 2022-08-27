using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NascarSurvival;
using NascarSurvival.Collectable;
using UniRx;
using UnityEngine;
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
            Observable.Interval(TimeSpan.FromSeconds(_enemySettings.TimeToSwitchBehaviour))
                .DoOnSubscribe(() => _aiMovementController.Movement = new Vector2(0, _enemySettings.MaxPowerOfCar / 100f))
                .Subscribe(_ =>
                {
                    CheckPowerChances();
                })
                .AddTo(_disposable);

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
                // Observable.EveryUpdate()
                //     .TakeWhile(_ => (_enemySettings.transform.position.x - nearestBonus.transform.position.x) > 0.1f)
                //     .Subscribe(_ => )
                
                // _aiMovementController.Movement += new Vector2(direction.x, _enemySettings.ValueToIncrease / 100f);
                // Debug.Log($"Speed increase {_aiMovementController.Movement.y}");
            }
            else
            {
                Debug.Log($"<color=black>Nearest Bonus Ignored{nearestBonus}", nearestBonus);
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
                if (distance < firstBonusDistance && _enemySettings.transform.position.z < x.transform.position.z)
                {
                    bonus = x;
                }
            });

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
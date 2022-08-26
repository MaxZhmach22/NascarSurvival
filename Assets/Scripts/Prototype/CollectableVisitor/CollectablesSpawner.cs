using System;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace NascarSurvival.Collectable
{
    public class CollectablesSpawner : IDisposable
    {
        private readonly AccelerateBonus.Factory _accelerationBonusFactory;
        private readonly FinishZone _finishZone;
        private readonly CollectableSpawnSettings _collectableSpawnSettings;
        private readonly GameStateHandler _gameStateHandler;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private Transform _bonusesParent;
        
        
        public CollectablesSpawner(AccelerateBonus.Factory accelerationBonusFactory, 
            FinishZone finishZone, 
            CollectableSpawnSettings collectableSpawnSettings, GameStateHandler gameStateHandler)
        {
            _accelerationBonusFactory = accelerationBonusFactory;
            _collectableSpawnSettings = collectableSpawnSettings;
            _gameStateHandler = gameStateHandler;
            _finishZone = finishZone;
            _bonusesParent = new GameObject("Bonuses Parent").transform;
            
            CreateSequence();
        }

        private void CreateSequence()
        {
            var timer = TakeRandomTime();
            
            Observable.Timer(TimeSpan.FromSeconds(timer))
                .Where(_ => _gameStateHandler.CurrentGameState == GameStates.Start)
                .DoOnTerminate(() => CreateSequence())
                .DoOnSubscribe(() => Debug.Log($"New timer value {timer}"))
                .Subscribe(_ => CreateAccelerationBonus())
                .AddTo(_disposable);
        }

        private void CreateAccelerationBonus()
        {
            var bonus = _accelerationBonusFactory.Create();
            bonus.transform.SetParent(_bonusesParent);
            var pos = Vector3.Lerp(_collectableSpawnSettings.transform.position, _finishZone.transform.position, Random.Range(0.2f, 0.8f));
            pos.Set(Random.Range(-4,4), 2, pos.z);
            bonus.gameObject.transform.position = pos;
            Debug.Log($"<color=cyan>Instanciated{bonus}</color>",bonus);
        }

        private float TakeRandomTime()
        {
            return Random.Range(_collectableSpawnSettings.MinTimeToSpwan, _collectableSpawnSettings.MaxTimeToSpwan);
        }

        public void Dispose()
        {
            Debug.Log($"{this} is Disposed");
            _disposable.Clear();
        }
    }
}
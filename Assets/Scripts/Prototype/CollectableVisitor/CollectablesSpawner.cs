using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace NascarSurvival.Collectable
{
    public class CollectablesSpawner : IDisposable
    {
        public List<AccelerateBonus> AccelerateBonusList = new List<AccelerateBonus>();
        
        private readonly AccelerateBonus.Factory _accelerationBonusFactory;
        private readonly DeccelerateBonus.Factory _deccelerationBonusFactory;
        private readonly BombBonus.Factory _bombBonusFactory;
        private readonly FinishZone _finishZone;
        private readonly CollectableSpawnSettings _collectableSpawnSettings;
        private readonly GameStateHandler _gameStateHandler;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private Transform _bonusesParent;

        public CollectablesSpawner(AccelerateBonus.Factory accelerationBonusFactory, 
            FinishZone finishZone, 
            CollectableSpawnSettings collectableSpawnSettings, GameStateHandler gameStateHandler, 
            DeccelerateBonus.Factory deccelerationBonusFactory, 
            BombBonus.Factory bombBonusFactory)
        {
            _accelerationBonusFactory = accelerationBonusFactory;
            _collectableSpawnSettings = collectableSpawnSettings;
            _gameStateHandler = gameStateHandler;
            _deccelerationBonusFactory = deccelerationBonusFactory;
            _bombBonusFactory = bombBonusFactory;
            _finishZone = finishZone;
            _bonusesParent = new GameObject("Bonuses Parent").transform;
            
            AccelerateBonusSpawnSequence();
            BombBonusSpawnSequence();
            BonusListObservable();
        }

        private void BombBonusSpawnSequence()
        {
            var timer = TakeRandomTime(_collectableSpawnSettings.BombBonusMixTimeToSpwan, _collectableSpawnSettings.BombBonusMaxTimeToSpwan);

            Observable.Timer(TimeSpan.FromSeconds(timer))
                .Where(_ => _gameStateHandler.CurrentGameState == GameStates.Start)
                .DoOnTerminate(() => BombBonusSpawnSequence())
                // .DoOnSubscribe(() => Debug.Log($"New timer value {timer}"))
                .Subscribe(_ => CreateBombBonus())
                .AddTo(_disposable);
        }

        private void BonusListObservable()
        {
            AccelerateBonusList.ObserveEveryValueChanged(x => x.FirstOrDefault(bonus => bonus.IsInteracted))
                .Where(bonus => bonus != null)
                .Subscribe(bonus => AccelerateBonusList.Remove(bonus))
                .AddTo(_disposable);
        }

        private void AccelerateBonusSpawnSequence()
        {
            var timer = TakeRandomTime(_collectableSpawnSettings.AccelerateBonusMinTimeToSpwan, _collectableSpawnSettings.AccelerateBonusMaxTimeToSpwan);

            Observable.Timer(TimeSpan.FromSeconds(timer))
                .Where(_ => _gameStateHandler.CurrentGameState == GameStates.Start)
                .DoOnTerminate(() => AccelerateBonusSpawnSequence())
                // .DoOnSubscribe(() => Debug.Log($"New timer value {timer}"))
                .Subscribe(_ => CreateAccelerationBonus())
                .AddTo(_disposable);
        }

        private void CreateAccelerationBonus()
        {
            var bonus = _accelerationBonusFactory.Create();
            bonus.transform.SetParent(_bonusesParent);
            var pos = Vector3.Lerp(_collectableSpawnSettings.transform.position, _finishZone.transform.position, Random.Range(0.1f, 0.8f));
            pos.Set(Random.Range(-4,4), 2, pos.z);
            bonus.gameObject.transform.position = pos;
            AccelerateBonusList.Add(bonus);
            // Debug.Log($"<color=cyan>Instanciated{bonus}</color>",bonus);
        }
        
        
        private void CreateBombBonus()
        {
            var bonus = _bombBonusFactory.Create();
            bonus.transform.SetParent(_bonusesParent);
            var pos = Vector3.Lerp(_collectableSpawnSettings.transform.position, _finishZone.transform.position, Random.Range(0.1f, 0.8f));
            pos.Set(Random.Range(-4,4), 5, pos.z);
            bonus.gameObject.transform.position = pos;
            // Debug.Log($"<color=cyan>Instanciated{bonus}</color>",bonus);
        }

        private float TakeRandomTime(float minValue,float maxValue)
        {
            return Random.Range(minValue, maxValue);
        }

        public void Dispose()
        {
            // Debug.Log($"{this} is Disposed");
            _disposable.Clear();
        }
    }
}
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Octavian.Runtime.Pools.ObjectPool
{
    [DisallowMultipleComponent]
    public  class AbstractPool : MonoBehaviour
    {
        private readonly Queue<PoolObject> pool = new Queue<PoolObject>();
        
        [SerializeField]
        [BoxGroup("Settings:")]
        private PoolObject poolObjectPrefab = default;
        
        [SerializeField]
        [BoxGroup("Settings:")] 
        private int poolSize = 128;

        private void Awake()
        {
            FillPool(poolSize);
        }

        public PoolObject Get()
        {
            if (pool.Count == 0) FillPool(1);

            var poolObject = pool.Dequeue();
            poolObject.TurnOn();
            
            return poolObject;
        }

        public void ReturnToPool(PoolObject poolObject) => pool.Enqueue(poolObject);

        private void FillPool(int amount)
        {
            for (var i = 0; i < amount; ++i)
            {
                var poolObject = Instantiate(poolObjectPrefab, transform, true);
                poolObject.Initialize(this);
                poolObject.TurnOff();
            }
        }
    }
}
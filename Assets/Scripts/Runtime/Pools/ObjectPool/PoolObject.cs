using UnityEngine;

namespace Octavian.Runtime.Pools.ObjectPool
{
    public abstract class PoolObject : MonoBehaviour
    {
        public abstract Rigidbody Rigidbody { get; }
        /// <summary>
        /// Initialize pool object with pool.
        /// </summary>
        /// <param name="pool"></param>
        public abstract void Initialize(AbstractPool pool);
        
        /// <summary>
        /// Prepare poolObject to perform it's tasks.
        /// </summary>
        public abstract void TurnOn();
        
        /// <summary>
        /// Reset and return to the pool.
        /// </summary>
        public abstract void TurnOff();
    }
}
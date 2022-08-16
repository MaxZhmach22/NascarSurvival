using UnityEngine;

namespace Octavian.Runtime.Pools.ObjectPool.ConcretePoolObjects
{
    [DisallowMultipleComponent]
    public class ExamplePoolObject : PoolObject
    {
        private AbstractPool _thisPool;

        public override Rigidbody Rigidbody { get; }

        public override void Initialize(AbstractPool pool)
        {
            _thisPool = pool;
        }

        public override void TurnOn()
        {
            gameObject.SetActive(true);
        }

        public override void TurnOff()
        {
            gameObject.SetActive(false);
            _thisPool.ReturnToPool(this);
        }
    }
}
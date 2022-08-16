using Octavian.Runtime.Pools.ObjectPool;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Code.Octavian.DestructibleBuilding.Runtime
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    public class DebrisPoolObject : PoolObject
    {
        private const float DestructionHeight = -10f;
        
        private AbstractPool _pool;
        private Rigidbody _rigidbody;

        public override Rigidbody Rigidbody => _rigidbody;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            
            Bind();
        }

        private void OnDisable() => TurnOff();

        public override void Initialize(AbstractPool pool) => _pool = pool;

        public override void TurnOn()
        {
            gameObject.SetActive(true);
            _rigidbody.isKinematic = false;
        }

        public override void TurnOff()
        {
            _rigidbody.isKinematic = true;
            gameObject.SetActive(false);
            _pool.ReturnToPool(this);
        }

        private void Bind()
        {
            this.UpdateAsObservable()
                .Where(_ => transform.position.y < DestructionHeight)
                .Subscribe(_ => TurnOff())
                .AddTo(this);
        }
    }
}






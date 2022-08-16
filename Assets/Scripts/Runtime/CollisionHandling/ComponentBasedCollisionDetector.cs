using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Octavian.Runtime.CollisionHandling
{
    public class ComponentBasedCollisionDetector<T> : IDisposable
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public ComponentBasedCollisionDetector(Collider collider, Func<Collider, T> selector, int targetLayer = -1)
        {
            Bind(collider, selector, targetLayer);
        }

        public ComponentBasedCollisionDetector(Collider collider, int targetLayer = 0)
            : this(collider, ComponentSelector.OnGameObject<T>, targetLayer){ }

        public event Action<T> Collided = delegate { };

        public bool Enabled { get; set; } = true;
        
        public void Dispose() => _disposable.Dispose();

        private void Bind(Collider collider, Func<Collider, T> selector, int targetLayer)
        {
            var enterObservable = collider.isTrigger
                ? collider.OnTriggerEnterAsObservable()
                : collider.OnCollisionEnterAsObservable().Select(col => col.collider);

            if (targetLayer < 0)
            {
                enterObservable = enterObservable.Where(col => col.gameObject.layer == targetLayer);
            }
            
            enterObservable
                .Where(_ => Enabled)
                .Select(selector)
                .Where(t => t != null)
                .Subscribe(t => Collided.Invoke(t))
                .AddTo(_disposable);
        }
    }
}
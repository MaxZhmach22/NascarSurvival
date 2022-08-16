using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Octavian.Runtime.CollisionHandling
{
    public class LayerBasedCollisionDetector : IDisposable
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        public LayerBasedCollisionDetector(Collider collider, int targetLayer)
        {
            Bind(collider, targetLayer);
        }

        public event Action<CollisionData> ChangedCollisionPhase = delegate { };

        public bool Enabled { get; set; } = true;
        
        public void Dispose() => _disposable.Dispose();

        private void Bind(Collider collider, int targetLayer)
        {
            var enterObservable = collider.isTrigger
                ? collider.OnTriggerEnterAsObservable()
                : collider.OnCollisionEnterAsObservable().Select(col => col.collider);
            
            var exitObservable = collider.isTrigger
                ? collider.OnTriggerExitAsObservable()
                : collider.OnCollisionExitAsObservable().Select(col => col.collider);
            
            enterObservable
                .Where(_ => Enabled)
                .Where(col => col.gameObject.layer == targetLayer)
                .Subscribe(col =>
                {
                    var collisionData = new CollisionData(col, CollisionPhase.Enter);
                    ChangedCollisionPhase.Invoke(collisionData);
                })
                .AddTo(_disposable);
            
            exitObservable
                .Where(_ => Enabled)
                .Where(col => col.gameObject.layer == targetLayer)
                .Subscribe(col =>
                {
                    var collisionData = new CollisionData(col, CollisionPhase.Exit);
                    ChangedCollisionPhase.Invoke(collisionData);
                })
                .AddTo(_disposable);
        }
    }
}
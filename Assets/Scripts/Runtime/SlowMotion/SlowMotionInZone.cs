using NaughtyAttributes;
using Octavian.Runtime.SlowMotion.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Octavian.Runtime.SlowMotion
{
    public class SlowMotionInZone : MonoBehaviour
    {
        private const float DefaultTimeScale = 1f;
        
        [SerializeField]
        private bool useDefaultSettings = false;
        [HideIf(nameof(useDefaultSettings))] [SerializeField]
        private float targetTimeScale = 0.1f;
        
        private const int PlayerLayer = 12;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        private void OnEnable()
        {
            this.OnTriggerEnterAsObservable()
                .Where(IsCollidingWithPlayer)
                .Take(1)
                .Subscribe(t => DoOnCollision(true))
                .AddTo(_disposable);

            this.OnTriggerExitAsObservable()
                .Where(IsCollidingWithPlayer)
                .Take(1)
                .Subscribe(t => DoOnCollision(false))
                .AddTo(_disposable);
        }

        private void OnDestroy() => _disposable.Dispose();

        private bool IsCollidingWithPlayer(Collider col) => col.gameObject.layer == PlayerLayer;
        
        private void DoOnCollision(bool newState)
        {
            if(useDefaultSettings) SlowMotionBoolMessage.Publish(newState);
            else SlowMotionFloatMessage.Publish(newState ? targetTimeScale : DefaultTimeScale);
        }
    }
}

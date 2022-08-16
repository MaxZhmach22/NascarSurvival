using Octavian.Runtime.SlowMotion.Message;
using Octavian.Runtime.Extensions;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Octavian.Runtime.SlowMotion
{
    public class SlowMotion : MonoBehaviour
    {
        private const float DefaultFixedDeltaTime = 0.02f;
        private const float DefaultTimeScale = 1f;

        private readonly CompositeDisposable _slowMotionDisposable = new CompositeDisposable();

        [SerializeField]
        private float defaultTargetTimeScale = 0.1f;
        [SerializeField]
        private float defaultTransitionTime = 1f;

        private void Awake()
        {
            MessageBroker.Default
                .Receive<SlowMotionBoolMessage>()
                .Subscribe(m =>
                {
                    var target = m.NewState ? defaultTargetTimeScale : DefaultTimeScale;
                    ChangeTimeScale(target, defaultTransitionTime);
                })
                .AddTo(this);
            
            MessageBroker.Default
                .Receive<SlowMotionFloatMessage>()
                .Subscribe(m => ChangeTimeScale(m.TargetTimeScale, defaultTransitionTime))
                .AddTo(this);
            
            MessageBroker.Default
                .Receive<SlowMotionMessage>()
                .Subscribe(m => ChangeTimeScale(m.TargetTimeScale, m.TransitionTime))
                .AddTo(this);
        }

        private void Start() => SetTimeScale(DefaultTimeScale);

        private void OnDestroy() => _slowMotionDisposable.Dispose();

        private void ChangeTimeScale(float target, float transitionTime)
        {
            // If slow motion is already running remove it.
            _slowMotionDisposable.Clear();
            var numberOfSteps = (int)(transitionTime / Time.unscaledDeltaTime);
            var timeScaleDelta = (Time.timeScale - target).Abs();
            var direction = target > Time.timeScale ? 1f : -1f;
            var changePerStep = (timeScaleDelta / numberOfSteps) * direction;
            this.UpdateAsObservable()
                .Take(numberOfSteps)
                .Subscribe(_ => SetTimeScale(Time.timeScale + changePerStep)
                    , () =>
                    {
                        SetTimeScale(target);
                        _slowMotionDisposable.Clear();
                    })
                .AddTo(_slowMotionDisposable);
        }

        private void SetTimeScale(float target)
        {
            target = Mathf.Clamp(target, 0f, 1f);
            Time.timeScale = target;
            Time.fixedDeltaTime = Time.timeScale * DefaultFixedDeltaTime;
        }
    }
}






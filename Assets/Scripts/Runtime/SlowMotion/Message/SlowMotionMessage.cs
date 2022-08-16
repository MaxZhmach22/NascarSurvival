using UniRx;

namespace Octavian.Runtime.SlowMotion.Message
{
    public readonly struct SlowMotionMessage 
    {
        public float TargetTimeScale { get; }
        
        public float TransitionTime { get; }

        private SlowMotionMessage(float targetTimeScale, float transitionTime)
        {
            TargetTimeScale = targetTimeScale;
            TransitionTime = transitionTime;
        }
    
        public static void Publish(float targetTimeScale, float transitionTime) => 
            MessageBroker.Default.Publish(new SlowMotionMessage(targetTimeScale, transitionTime));
    }
}

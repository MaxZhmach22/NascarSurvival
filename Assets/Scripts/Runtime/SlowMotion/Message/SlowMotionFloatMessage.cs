using UniRx;

namespace Octavian.Runtime.SlowMotion.Message
{
    public readonly struct SlowMotionFloatMessage
    {
        public float TargetTimeScale { get; }

        private SlowMotionFloatMessage(float targetTimeScale)
        {
            TargetTimeScale = targetTimeScale;
        }
    
        public static void Publish(float targetTimeScale) => MessageBroker.Default.Publish(new SlowMotionFloatMessage(targetTimeScale));
    }
}


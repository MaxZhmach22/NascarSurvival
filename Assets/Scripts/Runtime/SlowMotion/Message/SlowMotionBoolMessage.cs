using UniRx;

namespace Octavian.Runtime.SlowMotion.Message
{
    public readonly struct SlowMotionBoolMessage
    {
        public bool NewState { get; }

        private SlowMotionBoolMessage(bool newState)
        {
            NewState = newState;
        }
        
        public static void Publish(bool newState) => MessageBroker.Default.Publish(new SlowMotionBoolMessage(newState));
    }
}
using Octavian.Runtime.GeneralUseInterfaces;
using UniRx;

namespace Octavian.Runtime.Extensions
{
    public static class MessageExtension
    {
        public static void Publish<T>(this T message) where T : IMessage
        {
            MessageBroker.Default.Publish(message);
        }
    }
}
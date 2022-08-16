using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Octavian.Runtime.Utility
{
    public static class Timer
    {
        public static void RunAfter(double time, Action callback)
        {
            RunAfter(time, callback, CancellationToken.None);
        } 
        
        public static async void RunAfter(double time, Action callback, CancellationToken cancellationToken)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: cancellationToken);
            callback?.Invoke();
        }

        public static void RunBeforeAndAfter(double time, Action before, Action after)
        {
            RunBeforeAndAfter(time, before, after, CancellationToken.None);
        }
        
        public static async void RunBeforeAndAfter(double time, Action before, Action after, CancellationToken cancellationToken)
        {
            before?.Invoke();
            await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: cancellationToken);
            after?.Invoke();
        }
    }
}

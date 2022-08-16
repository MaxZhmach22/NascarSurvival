using System;
using UniRx;

namespace Octavian.Runtime.Extensions
{
    public static class DisposableExtensions
    {
        public static void AddRange(this CompositeDisposable compositeDisposable, params IDisposable[] disposables)
        {
            foreach (var t in disposables)
            {
                compositeDisposable.Add(t);
            }
        }
    }
}
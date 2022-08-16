using System.Collections.Generic;
using UnityEngine;

namespace Octavian.Runtime.DebugTools
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Print<T>(this IEnumerable<T> source)
        {
#if UNITY_EDITOR
            int index = 1;
            foreach (var item in source)
            {
                Debug.Log($"{index++}. {item}");
            }
#endif
            return source;
        }
    }
}


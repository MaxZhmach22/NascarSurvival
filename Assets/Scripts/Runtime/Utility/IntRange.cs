using Octavian.Runtime.Extensions;
using Octavian.Runtime.Math;
using UnityEngine;

namespace Octavian.Runtime.Utility
{
    [System.Serializable]
    public struct IntRange
    {
        [SerializeField] private int min;
        [SerializeField] private int max;

        public IntRange(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public int Min => min;
        public int Max => max;
        public int Length => (max - min).Abs();
        public int RandomFromRange => RandomF.Int(min, max);
        public static IntRange Default => new IntRange(0, 1);
    }
}
using Octavian.Runtime.Extensions;
using Octavian.Runtime.Math;
using UnityEngine;

namespace Octavian.Runtime.Utility
{
    [System.Serializable]
    public class FloatRange
    {
        [SerializeField] private float min;
        [SerializeField] private float max;
        
        public FloatRange(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public float Min => min;
        public float Max => max;
        public float Length => (max - min).Abs();
        public float RandomFromRange => RandomF.Float(min, max);
        
        public static FloatRange Default => new FloatRange(0f, 1f);
    }
}
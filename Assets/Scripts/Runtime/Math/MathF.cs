using Octavian.Runtime.Extensions;
using UnityEngine;

namespace Octavian.Runtime.Math
{
    public class MathF
    {
        /// <summary>
        /// Ignores Y axis.
        /// </summary>
        public static float ManhattanDistance(Vector3 a, Vector3 b)
        {
            var x = (a.x - b.x).Abs();
            var z = (a.z - b.z).Abs();
            return x + z;
        }        
    }
}
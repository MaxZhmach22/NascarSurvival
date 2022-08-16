using UnityEngine;

namespace Octavian.Runtime.Extensions
{
    public static class Vector3Extensions
    {
        /// <summary>
        /// Divides two vectors component-wise.
        /// </summary>
        public static Vector3 InverseScale(this Vector3 a, Vector3 b)
        {
            a.x /= b.x;
            a.y /= b.y;
            a.z /= b.z;
            return a;
        }

        public static Vector3 Round(this Vector3 a)
        {
            var x = Mathf.Round(a.x);
            var y = Mathf.Round(a.y);
            var z = Mathf.Round(a.z);
            return new Vector3(x, y, z);
        }
        
        public static float AxisSum(this Vector3 a)
        {
            return a.x + a.y + a.z;
        }
        
        public static float AxisProduct(this Vector3 a)
        {
            var clampedVector = new Vector3(a.x != 0 ? a.x : 1f, a.y != 0 ? a.y : 1f, a.z != 0 ? a.z : 1f);
            return clampedVector.x * clampedVector.y * clampedVector.z;
        }

        /// <summary>
        /// Return Vector3 with all its axis equal to value.
        /// </summary> 
        public static Vector3 DiagonalVector3(this float value)
        {
            return Vector3.one * value;
        }
        
        /// <summary>
        /// Return Vector3 with all its axis equal to value.
        /// </summary> 
        public static Vector2 DiagonalVector2(this float value)
        {
            return Vector2.one * value;
        }
        
        public static Vector3 Abs(this Vector3 a)
        {
            var x = System.Math.Abs(a.x);
            var y = System.Math.Abs(a.y);
            var z = System.Math.Abs(a.z);
            return new Vector3(x,y, z);
        }
        
        /// <summary>
        /// Return Vector3 with sign of each axis using System.Math.Sign.
        /// </summary>
        public static Vector3 Sign(this Vector3 a)
        {
            a.x = System.Math.Sign(a.x);
            a.y = System.Math.Sign(a.y);
            a.z = System.Math.Sign(a.z);
            return a;
        }

        public static Vector3 ClampExtremum(this Vector3 a, Vector3 clamp)
        {
            var x = Mathf.Clamp(a.x, clamp.x.Negative(), clamp.x.Abs());
            var y = Mathf.Clamp(a.y, clamp.y.Negative(), clamp.y.Abs());
            var z = Mathf.Clamp(a.z, clamp.z.Negative(), clamp.z.Abs());
            return new Vector3(x, y, z);
        }
        
        /// <summary>
        /// Clamps all axes of given vector between negative and positive clamp values.
        /// </summary>
        public static Vector3 ClampExtremum(this Vector3 a, float clamp)
        {
            return a.ClampExtremum(new Vector3(clamp, clamp, clamp));
        }

        /// <summary>
        /// Prints vector3 to console
        /// </summary>
        public static void Print(this Vector3 a)
        {
#if UNITY_EDITOR
            var message = $"({a.x :0.000000}, {a.y :0.000000}, {a.z :0.000000})";
            Debug.Log(message);
#endif
        }
        
        /// <summary>
        /// Prints vector2 to console
        /// </summary>
        public static void Print(this Vector2 a)
        {
#if UNITY_EDITOR
            var message = $"({a.x :0.000000}, {a.y :0.000000}";
            Debug.Log(message);
#endif
        }
    }
}






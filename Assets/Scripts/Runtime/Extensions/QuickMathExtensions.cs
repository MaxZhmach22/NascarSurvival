using UnityEngine;
using SystemMath = System.Math;

namespace Octavian.Runtime.Extensions
{
    public static class QuickMathExtensions
    {
        public static float Abs(this float a) => SystemMath.Abs(a);

        public static int Abs(this int a) => SystemMath.Abs(a);

        public static float Negative(this float a) => SystemMath.Abs(a) * -1;

        public static float Negative(this int a) => SystemMath.Abs(a) * -1;
        
        public static float Square(this float a) => a * a;

        public static int Square(this int a) => a * a;

        public static float Clamp(this ref float a, float min, float max) => a = Mathf.Clamp(a, min, max);

        public static int Clamp(this ref int a, int min, int max) => a = Mathf.Clamp(a, min, max);

        public static float Pow(this float a, float p) => Mathf.Pow(a, p);
        
        public static int Pow(this int a, int p) => (int)Mathf.Pow(a, p);
    }
}
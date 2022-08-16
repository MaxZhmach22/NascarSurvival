using UnityEngine;

namespace Octavian.Runtime.Math
{
    public static class RandomF
    {
        private static readonly System.Random Seed = new System.Random();
        
        public static int Int()
        {
            return Seed.Next();
        }

        /// <summary>
        /// Returns random in from min to max. 
        /// </summary>
        /// <param name="min">Inclusive</param>
        /// <param name="max">Inclusive</param>
        public static int Int(int min, int max)
        {
            return Seed.Next(min, max + 1);
        }
        
        public static int IntFromZero(int max)
        {
            return Seed.Next(0, max + 1);
        }
        
        /// <summary>
        /// Returns a random float that is greater than or equal to 0.0, and less than 1.0.
        /// </summary>
        public static float Float()
        {
            return (float)Seed.NextDouble();
        }
        
        public static float Float(float min, float max)
        {
            return (float)Seed.NextDouble() * (max - min) + min;
        }

        public static Vector3 Vector3(Vector3 min, Vector3 max)
        {
            var x = Float(min.x, max.x);
            var y = Float(min.y, max.y);
            var z = Float(min.z, max.z);
            return new Vector3(x, y, z);
        }
    }
}
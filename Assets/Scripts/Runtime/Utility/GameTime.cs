using UnityEngine;

namespace Octavian.Runtime.Utility
{
    public static class GameTime
    {
        public static float Multiplier = 100f;

        public static float SlowDeltaTime = Time.deltaTime * Time.timeScale;

        public static float SlowDeltaTimeWithMultiplier = Multiplier * Time.deltaTime * Time.timeScale;

        public static float DeltaTimeWithMultiplier = Multiplier * Time.deltaTime;

        public static float FixedDeltaTimeWithMultiplier = Multiplier * Time.fixedDeltaTime;
    }
}
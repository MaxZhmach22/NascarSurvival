using Octavian.Runtime.Extensions;

namespace Octavian.Runtime.Effects
{
    public enum GlobalEffect
    {
        NotReceivingEffectMessages = 0,
        SpeedLanes = 1,
        Confetti = 2,
        Test = 1000
    }

    public static class EffectExtension
    {
        public static void Play(this GlobalEffect globalEffect)
        {
            new EffectNewStateMessage(globalEffect, EffectState.Play).Publish();
        }
        
        public static void Pause(this GlobalEffect globalEffect)
        {
            new EffectNewStateMessage(globalEffect, EffectState.Pause).Publish();
        }
        
        public static void Stop(this GlobalEffect globalEffect)
        {
            new EffectNewStateMessage(globalEffect, EffectState.Stop).Publish();
        }
        
        public static void Restart(this GlobalEffect globalEffect)
        {
            new EffectNewStateMessage(globalEffect, EffectState.Restart).Publish();
        }
    }
}

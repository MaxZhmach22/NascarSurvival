using Octavian.Runtime.GeneralUseInterfaces;

namespace Octavian.Runtime.Effects
{
    public readonly struct EffectNewStateMessage : IMessage
    {
        public GlobalEffect GlobalEffectName { get; }
        public EffectState NewEffectState { get; }

        public EffectNewStateMessage(GlobalEffect globalEffectName, EffectState newEffectState)
        {
            GlobalEffectName = globalEffectName;
            NewEffectState = newEffectState;
        }
    }
}


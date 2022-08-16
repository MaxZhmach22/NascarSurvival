using UnityEngine;
using UniRx;

namespace Octavian.Runtime.Effects
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleEffectSwitch : MonoBehaviour
    {
        [SerializeField]
        private GlobalEffect globalEffectName = GlobalEffect.NotReceivingEffectMessages;

        private ParticleSystem _particleSystem;
        
        public GlobalEffect Name => globalEffectName;
        public ParticleSystem.MainModule MainModule { get; private set; }

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            MainModule = _particleSystem.main;

            if (globalEffectName != GlobalEffect.NotReceivingEffectMessages)
            {
                MessageBroker.Default.Receive<EffectNewStateMessage>()
                    .Where(m => globalEffectName == m.GlobalEffectName)
                    .Subscribe(m => ChangeEffectState(m.NewEffectState))
                    .AddTo(this);
            }
        }
        
        public void Play()
        {
            if (_particleSystem.isPlaying) return;
            _particleSystem.Play();
        }

        public void Stop()
        {
            _particleSystem.Stop();
        }
        
        public void Restart()
        {
            Stop();
            Play();
        }

        public void Pause()
        {
            _particleSystem.Pause();
        }
        
        public ParticleEffectSwitch DetachFromParent()
        {
            transform.parent = null;
            return this;
        }
        
        private void ChangeEffectState(EffectState newEffectState)
        {
            switch (newEffectState)
            {
                case EffectState.Play:
                    Play();
                    break;
                case EffectState.Stop:
                    Stop();
                    break;
                case EffectState.Pause:
                    Pause();
                    break;
                case EffectState.Restart:
                    Restart();
                    break;
            }
        }
    }
}







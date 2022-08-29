using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace NascarSurvival
{
    
    public class SoundHandler : MonoBehaviour
    {
        [SerializeField] private List<Sounds> _sounds;
        private bool _isMutedMusic;
        private bool _isMutedSound;
        
        private void Awake()
        {
            foreach (var sound in _sounds)
            {
                sound.AudioSource = gameObject.AddComponent<AudioSource>();
                sound.AudioSource.clip = sound.AudioClip;
                sound.AudioSource.volume = sound.Volume;
                sound.AudioSource.loop = sound.Looped;
                sound.AudioSource.pitch = sound.Pitch;
                sound.AudioSource.playOnAwake = sound.PlayOnAwake;
            }
        }

        private void Start()
        {
            Debug.Log("asdas");
            Play("MainTheme");
        }

        public void Play(string clipName)
        {
            var source = _sounds.FirstOrDefault(x => x.Name.Contains(clipName));
            if (source != null) source.AudioSource.Play();
            else
            {
                Debug.Log($"{clipName} not found!");
            }
        }

        public bool MuteMusic()
        {
            var source = _sounds.FirstOrDefault(x => x.Name.Contains("MainTheme"));
            _isMutedMusic = !_isMutedMusic;
            source.AudioSource.mute = _isMutedMusic;
            return _isMutedMusic;
        }
        
        public bool MuteSounds()
        {
            _sounds.ForEach(sound =>
            {
                if(!sound.Name.Contains("MainTheme"))
                    sound.AudioSource.mute = !_isMutedSound;
            });
            
            _isMutedSound = !_isMutedSound;
            return _isMutedSound;
        }

        public void StopAllSounds()
        {
            _sounds.ForEach(sound =>
            {
                if(!sound.Name.Contains("MainTheme"))
                    sound.AudioSource.Stop();
            });
        }
    }
}
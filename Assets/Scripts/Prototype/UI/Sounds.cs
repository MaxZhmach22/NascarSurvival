using System;
using UnityEngine;

namespace NascarSurvival
{
    [Serializable]
    public class Sounds
    {
        public string Name;
        public AudioClip AudioClip;
        [HideInInspector]public AudioSource AudioSource;
        [Range(0f, 1f)] public float Volume;
        [Range(0f, 1f)] public float Pitch;
        public bool Looped;
        public bool PlayOnAwake;
    }
}
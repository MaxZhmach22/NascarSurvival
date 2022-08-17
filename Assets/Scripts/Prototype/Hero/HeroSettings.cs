using NaughtyAttributes;
using UnityEngine;

namespace NascarSurvival
{
    public class HeroSettings : MonoBehaviour
    {
        [field: BoxGroup("Move Settings")] [field: SerializeField] public float Speed { get; private set; }
    }
}
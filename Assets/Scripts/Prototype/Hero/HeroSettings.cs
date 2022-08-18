using NaughtyAttributes;
using UnityEngine;

namespace NascarSurvival
{
    public class HeroSettings : MonoBehaviour
    {
        [field: BoxGroup("Move Settings")] [field: SerializeField] public float Speed { get; private set; } = 1f;
        [field: BoxGroup("Move Settings")] [field: SerializeField] public float ClampConstantSpeed { get; private set; } = 5f;
        [field: BoxGroup("Move Settings")] [field: SerializeField] public float StartAccelerationTime { get; private set; } = 1f;
    }
}
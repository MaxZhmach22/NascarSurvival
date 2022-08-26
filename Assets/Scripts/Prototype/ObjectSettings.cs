using NaughtyAttributes;
using UnityEngine;

namespace NascarSurvival
{
    public class ObjectSettings : MonoBehaviour
    {
        [field: BoxGroup("Move Settings")] [field: SerializeField] public float Speed { get; private set; } = 1f;
        [field: BoxGroup("Move Settings")] [field: SerializeField] public float StartSpeedToAccelerate { get; private set; } = 5f;
        [field: BoxGroup("Move Settings")] [field: SerializeField] public float StartAccelerationTime { get; private set; } = 1f;
    }
}
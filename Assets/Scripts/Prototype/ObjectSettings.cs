using NaughtyAttributes;
using UnityEngine;

namespace NascarSurvival
{
    public class ObjectSettings : MonoBehaviour
    {
        [field: BoxGroup("Move Settings")] [field: SerializeField] public float SensitiveXaxes { get; private set; } = 1f;
        [field: BoxGroup("Move Settings")] [field: SerializeField] public float SensitiveYaxes { get; private set; } = 2f;
        [field: BoxGroup("Move Settings")] [field: SerializeField] public float StartSpeed { get; private set; } = 5f;
        [field: BoxGroup("Move Settings")] [field: SerializeField] public float StartAccelerationTime { get; private set; } = 1f;
    }
}
using NaughtyAttributes;
using UnityEngine;

namespace NascarSurvival.Collectable
{
    public class CollectableSpawnSettings : MonoBehaviour
    {
        [field: BoxGroup("Accelerate bonus:")] [field: SerializeField] public float AccelerateBonusMinTimeToSpwan { get; private set; } = 3f;
        [field: BoxGroup("Accelerate bonus:")] [field: SerializeField] public float AccelerateBonusMaxTimeToSpwan { get; private set; } = 10f;
        [field: BoxGroup("Deccelerate bonus:")] [field: SerializeField] public float DeccelerateBonusMinTimeToSpwan { get; private set; } = 3f;
        [field: BoxGroup("Deccelerate bonus:")] [field: SerializeField] public float DeccelerateBonusMaxTimeToSpwan { get; private set; } = 10f;
        [field: BoxGroup("Bomb bonus:")] [field: SerializeField] public float BombBonusMixTimeToSpwan { get; private set; } = 3f;
        [field: BoxGroup("Bomb bonus:")] [field: SerializeField] public float BombBonusMaxTimeToSpwan { get; private set; } = 10f;
    }
}
using NaughtyAttributes;
using UnityEngine;

namespace NascarSurvival.Collectable
{
    public class CollectableSpawnSettings : MonoBehaviour
    {
        [field: BoxGroup("Accelerate bonus:")] [field: SerializeField] public float MinTimeToSpwan { get; private set; } = 3f;
        [field: BoxGroup("Accelerate bonus:")] [field: SerializeField] public float MaxTimeToSpwan { get; private set; } = 10f;
        
    }
}
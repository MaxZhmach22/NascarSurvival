using NaughtyAttributes;
using UnityEngine;

namespace Prototype.AI
{
    public class EnemySettings : MonoBehaviour
    {
        [field: BoxGroup("Move Settings")] [field: SerializeField] public float Speed { get; private set; } = 1f;
        [field: BoxGroup("Move Settings")] [field: SerializeField] public float StartSpeedToAccelerate { get; private set; } = 5f;
        [field: BoxGroup("Move Settings")] [field: SerializeField] public float StartAccelerationTime { get; private set; } = 1f;
        [field: Space(20f)]
        [field: Range(1, 100)] [field: BoxGroup("Chances:")] [field: SerializeField] public int ChanceToPickUpBonus { get; private set; } = 50;
        [field: Range(1, 100)] [field: BoxGroup("Chances:")] [field: SerializeField] public int ChanceToIncreasePower { get; private set; } = 50;
        [field: Range(1, 10)] [field: BoxGroup("Chances:")] [field: SerializeField] public int ValueToIncrease { get; private set; } = 5;
        [field: Range(1, 100)] [field: BoxGroup("Chances:")] [field: SerializeField] public int MaxPowerOfCar { get; private set; } = 80;
        [field: Range(0, 10)] [field: BoxGroup("Chances:")] [field: SerializeField] public float TimeToSwitchBehaviour { get; private set; } = 3f;
    }
}
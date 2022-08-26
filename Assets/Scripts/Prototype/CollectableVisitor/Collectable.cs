using NaughtyAttributes;
using UnityEngine;


namespace NascarSurvival.Collectable
{
    public abstract class Collectable : MonoBehaviour
    {
        [field: BoxGroup("Settings:")] [field: SerializeField] public float Value { get; private set; }
        [field: BoxGroup("Settings:")] [field: SerializeField] public float Duration  { get; private set; }
        [field: BoxGroup("Settings:")] [field: SerializeField] public float TimeBeforeStart  { get; private set; }
        
        public abstract void Request(IInteractable interactable, IInitializer initializer);
    }
}
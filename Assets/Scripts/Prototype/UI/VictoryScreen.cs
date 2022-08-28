using NaughtyAttributes;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


namespace NascarSurvival
{
    public class VictoryScreen : MonoBehaviour
    {
        [field: BoxGroup("References")] [field:SerializeField] public Button NextLevelButton { get; private set; }
        [field: BoxGroup("References")] [field:SerializeField] public Button RestartButton { get; private set; }
        
    }
}
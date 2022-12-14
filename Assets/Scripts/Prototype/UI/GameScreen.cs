using NaughtyAttributes;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


namespace NascarSurvival
{
    public class GameScreen : MonoBehaviour
    {
        [field: Foldout("References")] [field: SerializeField] public Button RestartButton { get; private set; }
    }
}
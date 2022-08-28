using NaughtyAttributes;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;


namespace NascarSurvival
{
    public class DefeatScreen : MonoBehaviour
    {
        [field: BoxGroup("References")] [field:SerializeField] public Button RestartButton { get; private set; }
    }
}
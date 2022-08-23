using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NascarSurvival
{
    public class GameUI : MonoBehaviour
    {
        [field: Foldout("References")] [field: SerializeField] public VictoryScreen VictoryScreen { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public DefeatScreen DefeatScreen { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public GameScreen GameScreen { get; private set; }
        [SerializeField] private Button _restartButton;
        [SerializeField] private TMP_Text _speedText;
    }
}
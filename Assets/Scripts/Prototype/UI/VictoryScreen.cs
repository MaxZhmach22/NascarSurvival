using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


namespace NascarSurvival
{
    public class VictoryScreen : MonoBehaviour
    {
        [SerializeField] private Button _nextLevelButton;
        private LoadSceneHandler _loadSceneHandler;

        
        private void Init(LoadSceneHandler loadSceneHandler)
        {
            _loadSceneHandler = loadSceneHandler;
        }
        
        private void Awake()
        {
            _nextLevelButton.OnClickAsObservable()
                .Subscribe(_ => _loadSceneHandler.LoadNextScene())
                .AddTo(this);
        }
    }
}
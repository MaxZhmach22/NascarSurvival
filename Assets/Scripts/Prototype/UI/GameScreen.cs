using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


namespace NascarSurvival
{
    public class GameScreen : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        private LoadSceneHandler _loadSceneHandler;
        
        private void Init(LoadSceneHandler loadSceneHandler)
        {
            _loadSceneHandler = loadSceneHandler;
        }
        
        private void Awake()
        {
            _restartButton.OnClickAsObservable()
                .Subscribe(_ => _loadSceneHandler.ReloadCurrentScene())
                .AddTo(this);
        }
    }
}
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;


namespace NascarSurvival
{
    public class DefeatScreen : MonoBehaviour
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
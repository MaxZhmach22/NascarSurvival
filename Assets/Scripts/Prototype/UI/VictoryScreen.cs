using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


namespace NascarSurvival
{
    public class VictoryScreen : MonoBehaviour
    {
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Button _restartButton;
        
        private void Awake()
        {
            _nextLevelButton.OnClickAsObservable()
                .Subscribe(_ => new LoadSceneHandler().LoadNextScene())
                .AddTo(this);
            
            _restartButton.OnClickAsObservable()
                .Subscribe(_ => new LoadSceneHandler().ReloadCurrentScene())
                .AddTo(this);
        }
    }
}
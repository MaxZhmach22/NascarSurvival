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
        
        private void Awake()
        {
            _restartButton.OnClickAsObservable()
                .Subscribe(_ => new LoadSceneHandler().ReloadCurrentScene())
                .AddTo(this);
        }
    }
}
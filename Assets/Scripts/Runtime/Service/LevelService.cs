using System;
using Octavian.Runtime.Extensions;
using Octavian.Runtime.Service.Messages;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Octavian.Runtime.Service
{
    public class LevelService : MonoBehaviour
    {
        private const string KeyLevel = "level";
        private const string CounterLevels = "counter";
        private static int _indexActiveScene = -1;
        private static int _counterLevels = 1;

        private void Awake()
        {
            if (PlayerPrefs.HasKey(CounterLevels))
            {
                _counterLevels = PlayerPrefs.GetInt(CounterLevels);
            }
            
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                if (PlayerPrefs.HasKey(KeyLevel))
                {
                    _indexActiveScene = PlayerPrefs.GetInt(KeyLevel);
                }
                
                if (_indexActiveScene == -1 || _indexActiveScene >= SceneManager.sceneCountInBuildSettings)
                {
                    LoadScene(1);
                }
                else
                {
                    LoadScene(_indexActiveScene);
                }
            }
            else
            {
                _indexActiveScene = SceneManager.GetActiveScene().buildIndex;
            }

            Bind();
        }

        private void Start()
        {
            MessageBroker.Default.Publish(this);
        }

        private void Bind()
        {
            MessageBroker.Default
                .Receive<GetLevelServiceMessage>()
                .Subscribe(_ => MessageBroker.Default.Publish(this))
                .AddTo(this);
        }


        public void LoadNextScene()
        {
            ++_counterLevels;
            var scene = SceneManager.GetActiveScene().buildIndex;
            _indexActiveScene = scene <= 0 || scene + 1 >= SceneManager.sceneCountInBuildSettings ? 1 : scene + 1;
            LoadAsyncScene(_indexActiveScene);
        }
        
        public void LoadScene(int index)
        {
            // Logging
            _indexActiveScene = index;
            LoadAsyncScene(_indexActiveScene);
        }
        
        public void ReloadCurrentScene()
        {
            LoadScene(_indexActiveScene);
        }
        
        public int GetActiveScene()
        {
            return _indexActiveScene;
        }

        public int GetCounterLevels()
        {
            return _counterLevels;
        }
        
        private void LoadAsyncScene(int indexScene, Action callback = null)
        {
            new LoadLevelMessage().Publish();
            PlayerPrefs.SetInt(CounterLevels, _counterLevels);
            PlayerPrefs.SetInt(KeyLevel, indexScene);
            PlayerPrefs.Save();
            SceneManager.LoadSceneAsync(indexScene, LoadSceneMode.Single).completed += operation =>
            {
                callback?.Invoke();
            };
        }
    }
}
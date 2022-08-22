using System;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace NascarSurvival
{
    public class LoadSceneHandler
    {
        private const string KeyLevel = "level";
        private const string CounterLevels = "counter";
        private static int _indexActiveScene = -1;
        private static int _counterLevels = 1;


        public LoadSceneHandler()
        {
            // if (PlayerPrefs.HasKey(CounterLevels))
            // {
            //     _counterLevels = PlayerPrefs.GetInt(CounterLevels);
            // }
            //
            // if (SceneManager.GetActiveScene().buildIndex == 0)
            // {
            //     if (PlayerPrefs.HasKey(KeyLevel))
            //     {
            //         _indexActiveScene = PlayerPrefs.GetInt(KeyLevel);
            //     }
            //     
            //     if (_indexActiveScene == -1 || _indexActiveScene >= SceneManager.sceneCountInBuildSettings)
            //     {
            //         LoadScene(1);
            //     }
            //     else
            //     {
            //         LoadScene(_indexActiveScene);
            //     }
            // }
            // else
            // {
            //     _indexActiveScene = SceneManager.GetActiveScene().buildIndex;
            // }
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
        
        public void LoadAsyncScene(int index, Action callback = null)
        {
            PlayerPrefs.SetInt(KeyLevel, _counterLevels);
            PlayerPrefs.SetInt(KeyLevel, index);
            PlayerPrefs.Save();
            SceneManager.LoadSceneAsync(index).completed += operation =>
                callback?.Invoke();
        }
    }
}
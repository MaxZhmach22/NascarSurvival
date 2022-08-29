using System;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace NascarSurvival
{
    public class GameUI : MonoBehaviour
    {
        [field: Foldout("References")] [field: SerializeField] public VictoryScreen VictoryScreen { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public DefeatScreen DefeatScreen { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public GameScreen GameScreen { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public Button GameMuteButton { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public TMP_Text SpeedText { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public TMP_Text GameMessages { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public TMP_Text TextLevel { get; private set; }

        private List<GameObject> _screens = new List<GameObject>();
        private GameStateHandler _gameStateHandler;
        public SoundHandler SoundHandler { get; private set; }

        [Inject]
        private void Init(GameStateHandler gameStateHandler, SoundHandler soundHandler)
        {
            _gameStateHandler = gameStateHandler;
            SoundHandler = soundHandler;
        }
        
        private void Start()
        {
            
            
            _screens.Add(VictoryScreen.gameObject);
            _screens.Add(DefeatScreen.gameObject);
            _screens.Add(GameScreen.gameObject);

            _gameStateHandler.OnChangeState += OnChangeState;

            TextLevel.text = SceneManager.GetActiveScene().name;

            BindButtons();
        }

        private void BindButtons()
        {
            GameScreen.RestartButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    SoundHandler.Play("ClickUi2");
                    new LoadSceneHandler().ReloadCurrentScene();
                })
                .AddTo(this);
            
            VictoryScreen.NextLevelButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    SoundHandler.Play("ClickUi1");
                    new LoadSceneHandler().LoadNextScene();
                })
                .AddTo(this);
            
            VictoryScreen.RestartButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    SoundHandler.Play("ClickUi2");
                    new LoadSceneHandler().ReloadCurrentScene();
                })
                .AddTo(this);
            
            DefeatScreen.RestartButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    SoundHandler.Play("ClickUi2");
                    new LoadSceneHandler().ReloadCurrentScene();
                })
                .AddTo(this);

            GameMuteButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    SoundHandler.MuteMusic();
                    SoundHandler.MuteSounds();
                })
                .AddTo(this);
        }

        private void OnChangeState(GameStates state)
        {
            DeactivateScreen();
            
            switch (state)
            {
                case GameStates.Defeat:
                    DefeatScreen.gameObject.SetActive(true);
                    _gameStateHandler.OnChangeState -= OnChangeState;
                    break;
                
                case GameStates.Finish:
                    VictoryScreen.gameObject.SetActive(true);
                    _gameStateHandler.OnChangeState -= OnChangeState;
                    break;
                
                case GameStates.Start:
                    GameScreen.gameObject.SetActive(true);
                    break;
            }
        }

        private void DeactivateScreen()
        {
            _screens.ForEach(x => x.gameObject.SetActive(false));
        }
        
        private void OnDestroy()
        {
            SoundHandler.StopAllSounds();
            _gameStateHandler.OnChangeState -= OnChangeState;
        }
    }
}
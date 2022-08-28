using System;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace NascarSurvival
{
    public class GameUI : MonoBehaviour
    {
        [field: Foldout("References")] [field: SerializeField] public VictoryScreen VictoryScreen { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public DefeatScreen DefeatScreen { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public GameScreen GameScreen { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public TMP_Text SpeedText { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public TMP_Text StartCounter { get; private set; }

        private List<GameObject> _screens = new List<GameObject>();
        private GameStateHandler _gameStateHandler;
        
        [Inject]
        private void Init(GameStateHandler gameStateHandler)
        {
            _gameStateHandler = gameStateHandler;
        }
        
        private void Start()
        {
            _screens.Add(VictoryScreen.gameObject);
            _screens.Add(DefeatScreen.gameObject);
            _screens.Add(GameScreen.gameObject);

            _gameStateHandler.OnChangeState += OnChangeState;
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
            _gameStateHandler.OnChangeState -= OnChangeState;
        }
    }
}
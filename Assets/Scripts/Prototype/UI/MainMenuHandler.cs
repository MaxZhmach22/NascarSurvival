using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;


namespace NascarSurvival
{
    public class MainMenuHandler : MonoBehaviour
    {
        [field: Foldout("References")] [field: SerializeField] public GameObject SettingsMenu { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public GameObject MainMenu { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public GameObject PlayerPrefsMenuInfo { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public Button RaceButton { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public Button SettingsButton { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public Button MuteMusicButton { get; private set; }
        
        [field: Foldout("References")] [field: SerializeField] public Button MuteSoundButton { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public Button BackToMainMenu { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public Button ResetProgressButton { get; private set; }
        private List<GameObject> _menus = new List<GameObject>();
        private SoundHandler _soundHandler;


        private void Awake()
        {
            FindSoundHandler();
            
            _menus.Add(SettingsMenu);
            _menus.Add(MainMenu);
            MainMenu.gameObject.SetActive(true);
            Bind();
        }

        private void FindSoundHandler()
        {
            _soundHandler = FindObjectOfType<SoundHandler>();
            if (_soundHandler == null)
            {
                _soundHandler = new GameObject("SoundHandler").AddComponent<SoundHandler>();
            }
        }

        private void Bind()
        {
            SettingsButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _soundHandler.Play("ClickUi1");
                    _menus.ForEach(x => x.gameObject.SetActive(false));
                    SettingsMenu.gameObject.SetActive(true);
                })
                .AddTo(this);
            
            BackToMainMenu.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _soundHandler.Play("ClickUi2");
                    _menus.ForEach(x => x.gameObject.SetActive(false));
                    MainMenu.gameObject.SetActive(true);
                })
                .AddTo(this);
            
            ResetProgressButton.OnClickAsObservable()
                .Subscribe( async _=>
                {
                    _soundHandler.Play("ClickUi1");
                   PlayerPrefs.DeleteAll();
                   _menus.ForEach(x => x.gameObject.SetActive(false));
                   PlayerPrefsMenuInfo.gameObject.SetActive(true);
                   await UniTask.Delay(TimeSpan.FromSeconds(2),
                       cancellationToken: this.GetCancellationTokenOnDestroy());
                   PlayerPrefsMenuInfo.gameObject.SetActive(false);
                   SettingsMenu.gameObject.SetActive(true);
                })
                .AddTo(this);

            MuteMusicButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    var text = _soundHandler.MuteMusic() ? "Music OFF" : "Music ON";
                    MuteMusicButton.GetComponentInChildren<TMP_Text>().text = text;
                })
                .AddTo(this);
            
            MuteSoundButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    var text = _soundHandler.MuteSounds() ? "Sounds OFF" : "Sounds ON";
                    MuteSoundButton.GetComponentInChildren<TMP_Text>().text = text;
                })
                .AddTo(this);
            
            RaceButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _soundHandler.Play("ClickUi1");
                    new LoadSceneHandler();
                })
                .AddTo(this);
        }
    }

}
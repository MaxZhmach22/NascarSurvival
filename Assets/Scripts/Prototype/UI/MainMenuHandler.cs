using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
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
        [field: Foldout("References")] [field: SerializeField] public Button MuteOnButton { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public Button BackToMainMenu { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public Button ResetProgressButton { get; private set; }
        private List<GameObject> _menus = new List<GameObject>();

        private void Awake()
        {
            _menus.Add(SettingsMenu);
            _menus.Add(MainMenu);
            
            MainMenu.gameObject.SetActive(true);
            Bind();
        }

        private void Bind()
        {
            SettingsButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _menus.ForEach(x => x.gameObject.SetActive(false));
                    SettingsMenu.gameObject.SetActive(true);
                })
                .AddTo(this);
            
            BackToMainMenu.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _menus.ForEach(x => x.gameObject.SetActive(false));
                    MainMenu.gameObject.SetActive(true);
                })
                .AddTo(this);
            
            ResetProgressButton.OnClickAsObservable()
                .Subscribe( async _=>
                {
                   PlayerPrefs.DeleteAll();
                   _menus.ForEach(x => x.gameObject.SetActive(false));
                   PlayerPrefsMenuInfo.gameObject.SetActive(true);
                   await UniTask.Delay(TimeSpan.FromSeconds(2),
                       cancellationToken: this.GetCancellationTokenOnDestroy());
                   PlayerPrefsMenuInfo.gameObject.SetActive(false);
                   SettingsMenu.gameObject.SetActive(true);
                })
                .AddTo(this);

            MuteOnButton.OnClickAsObservable()
                .Subscribe(_ => Debug.Log("Mute All Sounds"))
                .AddTo(this);
            
            RaceButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    new LoadSceneHandler();
                })
                .AddTo(this);
        }
    }

}
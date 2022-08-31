using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


namespace NascarSurvival
{
    public class MainMenuHandler : MonoBehaviour
    {
        [field: Foldout("References")] [field: SerializeField] public GameObject SettingsMenu { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public GameObject MainMenu { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public GameObject PlayerPrefsMenuInfo { get; private set; }
        // [field: Foldout("References")] [field: SerializeField] public GameObject CreateAccountPanel { get; private set; }
        // [field: Foldout("References")] [field: SerializeField] public GameObject RegisterPanel { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public Button RaceButton { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public Button SettingsButton { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public Button MuteMusicButton { get; private set; }
        
        [field: Foldout("References")] [field: SerializeField] public Button MuteSoundButton { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public Button BackToMainMenu { get; private set; }
        [field: Foldout("References")] [field: SerializeField] public Button ResetProgressButton { get; private set; }
        // [field: Foldout("References")] [field: SerializeField] public TMP_Text UserName { get; private set; }
        // [field: Foldout("References")] [field: SerializeField] public TMP_Text LabelInMainMenu { get; private set; }
        
        
        
        // [field: Foldout("References")] [field: SerializeField] public Button Register { get; private set; }
        // [field: Foldout("References")] [field: SerializeField] public Button SingIn { get; private set; }
        //
        // [field: Foldout("References")] [field: SerializeField] public Button Create { get; private set; }
        // [field: Foldout("References")] [field: SerializeField] public TMP_Text CreatePassword { get; private set; }
        // [field: Foldout("References")] [field: SerializeField] public TMP_Text CreateUser { get; private set; }
        // [field: Foldout("References")] [field: SerializeField] public TMP_Text CreateMail { get; private set; }
        //
        // [field: Foldout("References")] [field: SerializeField] public TMP_Text Login { get; private set; }
        // [field: Foldout("References")] [field: SerializeField] public TMP_Text Password { get; private set; }

        
        
        private const string AuthGuidKey = "authorization-guid";
        private List<GameObject> _menus = new List<GameObject>();
        private SoundHandler _soundHandler;
        private string _username;
        private string _mail;
        private string _pass;
      
        
        [Inject]
        private void Init (SoundHandler soundHandler)
        {
            _soundHandler = soundHandler;
        }

        private void Awake()
        {
            _menus.Add(SettingsMenu);
            _menus.Add(MainMenu);
            // _menus.Add(CreateAccountPanel);
            // _menus.Add(RegisterPanel);
            // RegisterPanel.gameObject.SetActive(true);
            //MainMenu.gameObject.SetActive(true);
            Bind();
           // CreateObservableOnAccountText();
           // CreateObservableOnLoginText();
        }

        private void Start()
        {
            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            {
                PlayFabSettings.staticSettings.TitleId = " A823B";
            }
            var needCreation = PlayerPrefs.HasKey(AuthGuidKey);
            var id = PlayerPrefs.GetString(AuthGuidKey, Guid.NewGuid().ToString());
            PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
            {
                CustomId = id,
                CreateAccount = !needCreation
            }, success =>
            {
                PlayerPrefs.SetString(AuthGuidKey, id);
            }, error =>
            {
                Debug.Log("Fail");
            });
        }

        // private void CreateObservableOnLoginText()
        // {
        //     Login.ObserveEveryValueChanged(x => x.text)
        //         .Subscribe(text => UpdateEmail(text))
        //         .AddTo(this);
        //     
        //     Password.ObserveEveryValueChanged(x => x.text)
        //         .Subscribe(text => UpdatePassword(text))
        //         .AddTo(this);
        // }
        //
        // private void CreateObservableOnAccountText()
        // {
        //     CreateMail.ObserveEveryValueChanged(x => x.text)
        //         .Subscribe(text => UpdateEmail(text))
        //         .AddTo(this);
        //     CreatePassword.ObserveEveryValueChanged(x => x.text)
        //         .Subscribe(text => UpdatePassword(text))
        //         .AddTo(this);
        //     CreateUser.ObserveEveryValueChanged(x => x.text)
        //         .Subscribe(text => UpdateUsername(text))
        //         .AddTo(this);
        // }


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

            // Register.OnClickAsObservable()
            //     .Subscribe(_ =>
            //     {
            //         _soundHandler.Play("ClickUi1");
            //         RegisterPanel.gameObject.SetActive(false);
            //         CreateAccountPanel.gameObject.SetActive(true);
            //     })
            //     .AddTo(this);

            // SingIn.OnClickAsObservable()
            //     .Subscribe( async _ =>
            //     {
            //         _soundHandler.Play("ClickUi1");
            //
            //         SignIn();
            //         
            //         await UniTask.Delay(TimeSpan.FromSeconds(1),
            //             cancellationToken: this.GetCancellationTokenOnDestroy());
            //         
            //         RegisterPanel.gameObject.SetActive(false);
            //         CreateAccountPanel.SetActive(false);
            //         MainMenu.gameObject.SetActive(true);
            //     })
            //     .AddTo(this);

            // Create.OnClickAsObservable()
            //     .Subscribe(_ =>
            //     {
            //         CreateAccount();
            //     })
            //     .AddTo(this);
        }

        public void UpdateUsername(string username)
        {
            _username = username;
        }

        public void UpdateEmail(string mail)
        {
            _mail = mail;
        }

        public void UpdatePassword(string pass)
        {
            _pass = pass;
        }

        // public void CreateAccount()
        // {
        //     PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest()
        //     {
        //         Username = _username.Remove(_username.Length -1),
        //         Email = _mail,
        //         Password = _pass,
        //         RequireBothUsernameAndEmail = true
        //     }, result =>
        //     {
        //         Debug.Log($"Succes: {_username}");
        //     }, error =>
        //     {
        //         Debug.LogError($"Fail: {error.ErrorMessage}");
        //     });
        // }
        //
        // public void SignIn()
        // {
        //     PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        //     {
        //         Username = _username.Remove(_username.Length -1),
        //         Password = _pass
        //     }, result =>
        //     {
        //         Debug.Log($"Success: {_username}");
        //     }, error =>
        //     {
        //         Debug.LogError($"Fail: {error.ErrorMessage}");
        //     });
        // }
    }

}
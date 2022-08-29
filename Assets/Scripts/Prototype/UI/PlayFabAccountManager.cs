using System;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;


namespace NascarSurvival
{
    public class PlayFabAccountManager : MonoBehaviour
    {
        [SerializeField] private Text _titleLabel;
        private async  void Start()
        {
            await UniTask.Delay(TimeSpan.FromMilliseconds(500),
                cancellationToken: this.GetCancellationTokenOnDestroy());
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(),
                OnGetAccountSuccess, OnFailure);
        }
        private void OnGetAccountSuccess(GetAccountInfoResult result)
        {
            _titleLabel.text = $"Welcome back, Player ID {result.AccountInfo.PlayFabId}";
        }
        private void OnFailure(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            Debug.LogError($"Something went wrong: {errorMessage}");
        }
    }
}

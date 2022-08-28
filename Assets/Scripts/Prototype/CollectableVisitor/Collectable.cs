using System;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;


namespace NascarSurvival.Collectable
{
    public abstract class Collectable : MonoBehaviour
    {
        [field: BoxGroup("Settings:")] [field: SerializeField] public float Value { get; private set; }
        [field: BoxGroup("Settings:")] [field: SerializeField] public float Duration  { get; private set; }
        [field: BoxGroup("Settings:")] [field: SerializeField] public float TimeBeforeStart  { get; private set; }
        
        public abstract void Request(IInteractable interactable, IInitializer initializer);
        
        protected async void GameMessageText(GameUI gameUI, string message)
        {
            gameUI.GameMessages.gameObject.SetActive(true);
            var startFontSize = gameUI.GameMessages.fontSize;
            gameUI.GameMessages.fontSize /= 2;
            gameUI.GameMessages.text = message;
            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: this.GetCancellationTokenOnDestroy());
            gameUI.GameMessages.fontSize = startFontSize;
            gameUI.GameMessages.text = "";
        }
    }
}
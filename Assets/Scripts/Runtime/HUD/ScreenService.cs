using Octavian.Runtime.Service;
using Octavian.Runtime.Service.Messages;
using UniRx;
using UnityEngine;

namespace Octavian.Runtime.HUD
{
    public class ScreenService : MonoBehaviour
    {
        [SerializeField]
        private VictoryScreen victoryScreen = default;
        [SerializeField]
        private DefeatScreen defeatScreen = default;
        
        private LevelService _levelService;

        private void Awake()
        {
            victoryScreen.gameObject.SetActive(false);
            defeatScreen.gameObject.SetActive(false);
            Bind();
        }

        private void Bind()
        {
            MessageBroker.Default
                .Receive<LevelService>()
                .Subscribe(t => _levelService = t)
                .AddTo(this);

            MessageBroker.Default
                .Receive<EndGameMessage>()
                .Take(1)
                .Subscribe(t =>
                {
                    victoryScreen.gameObject.SetActive(t.IsWin);
                    defeatScreen.gameObject.SetActive(!t.IsWin);
                })
                .AddTo(this);
        }
    }
}

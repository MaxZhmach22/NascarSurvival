using Octavian.Runtime.Extensions;
using Octavian.Runtime.Service;
using Octavian.Runtime.Service.Messages;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Octavian.Runtime.HUD
{
    public abstract class ScreenBase : MonoBehaviour
    {
        public TextMeshProUGUI TextTitle => textTitle;
        
        [SerializeField]
        protected Button buttonNextLevel = null;
        [SerializeField]
        protected TextMeshProUGUI textTitle = null;
        
        protected LevelService _levelService;

        protected virtual void Awake()
        {
            Bind();
        }

        protected virtual void Start()
        {
            if (buttonNextLevel == null) { return; }

            new GetLevelServiceMessage().Publish();
        }
        
        protected virtual void Bind()
        {
            MessageBroker.Default
                .Receive<LevelService>()
                .Subscribe(t => _levelService = t)
                .AddTo(this);
        }
    }
}
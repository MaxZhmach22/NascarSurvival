using Octavian.Runtime.DebugTools;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Octavian.Runtime.HUD
{
    public class GameScreen : ScreenBase
    {
        private const KeyCode ReloadKey = KeyCode.R;

        [SerializeField]
        private bool clearConsoleOnReload = false;

        protected override void Bind()
        {
            base.Bind();
            this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(ReloadKey))
                .Subscribe(_ =>
                {
                    if(clearConsoleOnReload) Tools.ClearConsole();
                    _levelService.ReloadCurrentScene();
                })
                .AddTo(this);
        }

        protected override void Start()
        {
            base.Start();
            buttonNextLevel.onClick.AddListener(() => _levelService.ReloadCurrentScene());
        }
    }
}






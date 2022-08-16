namespace Octavian.Runtime.HUD
{ 
    public class DefeatScreen : ScreenBase
    {
        protected override void Start()
        {
            base.Start();
            buttonNextLevel.onClick.AddListener(_levelService.ReloadCurrentScene);
        }
    }
}
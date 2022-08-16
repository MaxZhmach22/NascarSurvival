namespace Octavian.Runtime.HUD
{
    public class VictoryScreen : ScreenBase
    {
        protected override void Start()
        {
            base.Start();
            buttonNextLevel.onClick.AddListener(_levelService.LoadNextScene);
        }
    }
}
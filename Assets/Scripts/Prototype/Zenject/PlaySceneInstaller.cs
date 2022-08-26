using NascarSurvival;
using NaughtyAttributes;
using Prototype.AI;
using UnityEngine;
using Zenject;

public class PlaySceneInstaller : MonoInstaller
{
    [field: BoxGroup("Settings:")] [field: SerializeField] public DynamicJoystick DynamicJoystick { get; private set; }
    [field: BoxGroup("Settings:")] [field: SerializeField] public HeroInitializer HeroInitializer { get; private set; }
    [field: BoxGroup("Settings:")] [field: SerializeField] public HeroSettings HeroSettings { get; private set; }
    [field: BoxGroup("Settings:")] [field: SerializeField] public FinishZone FinishZone { get; private set; }
    [field: BoxGroup("Settings:")] [field: SerializeField] public Canvas UICanvas { get; private set; }
    [field: BoxGroup("Settings:")] [field: SerializeField] public GameUI GameUI { get; private set; }
    
    
    public override void InstallBindings()
    {
        // Container.BindFactory<TestSphere, TestSphere.Factory>().FromComponentInNewPrefab(_gameObject);
        Container.BindInstance(GameUI).AsSingle();
        Container.Bind<IMoveController>().To<DynamicJoystick>().FromComponentInNewPrefab(DynamicJoystick).UnderTransform(UICanvas.transform).AsSingle();
        Container.Bind<HeroInitializer>().FromInstance(HeroInitializer).AsSingle();
        Container.Bind<HeroSettings>().FromInstance(HeroSettings).AsSingle();
        Container.Bind<FinishZone>().FromInstance(FinishZone).AsSingle();
        Container.Bind<GameStateHandler>().FromInstance(new GameStateHandler()).AsSingle();
    }
    
}
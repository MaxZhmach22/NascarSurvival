using NascarSurvival;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [field: BoxGroup("Settings:")] [field: SerializeField] public DynamicJoystick DynamicJoystick { get; private set; }
    [field: BoxGroup("Settings:")] [field: SerializeField] public HeroInitializer HeroInitializer { get; private set; }
    [field: BoxGroup("Settings:")] [field: SerializeField] public HeroSettings HeroSettings { get; private set; }
    [field: BoxGroup("Settings:")] [field: SerializeField] public FinishZone FinishZone { get; private set; }
    [field: BoxGroup("Settings:")] [field: SerializeField] public GameUI GameUI { get; private set; }
    
    public override void InstallBindings()
    {
        //var instance = new GameStateHandler();
        
        // Container.BindFactory<TestSphere, TestSphere.Factory>().FromComponentInNewPrefab(_gameObject);
       Container.Bind<DynamicJoystick>().FromInstance(DynamicJoystick).AsSingle();
       Container.Bind<HeroInitializer>().FromInstance(HeroInitializer).AsSingle();
       Container.Bind<HeroSettings>().FromInstance(HeroSettings).AsSingle();
       Container.Bind<FinishZone>().FromInstance(FinishZone).AsSingle();
       //Container.Bind<HeroMovement>().AsSingle();
       Container.Bind<GameStateHandler>().FromInstance(new GameStateHandler()).AsSingle();
       Container.Bind<GameUI>().FromInstance(GameUI).AsSingle();
    }
}
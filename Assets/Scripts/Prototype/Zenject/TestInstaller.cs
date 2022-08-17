using NascarSurvival;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class TestInstaller : MonoInstaller
{
    [field: BoxGroup("Settings:")] [field: SerializeField] public DynamicJoystick DynamicJoystick { get; private set; }
    [field: BoxGroup("Settings:")] [field: SerializeField] public HeroInitializer HeroInitializer { get; private set; }
    
    public override void InstallBindings()
    {
        //var instance = new GameStateHandler();
        
        // Container.BindFactory<TestSphere, TestSphere.Factory>().FromComponentInNewPrefab(_gameObject);
       Container.Bind<DynamicJoystick>().FromInstance(DynamicJoystick).AsSingle();
       Container.Bind<HeroInitializer>().FromInstance(HeroInitializer).AsSingle();
       //Container.Bind<HeroMovement>().AsSingle();
       Container.Bind<GameStateHandler>().FromInstance(new GameStateHandler()).AsSingle();
    }
}
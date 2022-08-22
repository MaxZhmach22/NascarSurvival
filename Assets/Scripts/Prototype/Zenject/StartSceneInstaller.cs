using UnityEngine;
using Zenject;

namespace NascarSurvival
{
    public class StartSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LoadSceneHandler>().AsSingle().NonLazy();
        }
    }
}
using UnityEngine;
using Zenject;

namespace NascarSurvival
{
    public class SoundHandlerInstaller : MonoInstaller
    {
        [SerializeField] private SoundHandler _soundHandler;
        
        public override void InstallBindings()
        {
            Container.Bind<SoundHandler>().FromComponentInNewPrefab(_soundHandler).AsSingle();
        }
    }
}
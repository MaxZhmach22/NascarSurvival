using UniRx;
using UnityEngine;
using Zenject;


namespace NascarSurvival
{
    public class Starter : MonoBehaviour
    {
        private TestSphere.Factory _sphereFactory;
        
        [Inject]
        private void Set(TestSphere.Factory sphereFactory)
        {
            _sphereFactory = sphereFactory;
        }
        
        private void Start()
        {
            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.A))
                .Subscribe(_ => _sphereFactory.Create())
                .AddTo(this);
        }
    }
}
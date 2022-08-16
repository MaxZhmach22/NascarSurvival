using UnityEngine;

namespace Octavian.Runtime.Service
{
    [DefaultExecutionOrder(-100)]
    public class ContainerService : MonoBehaviour
    {
        [SerializeField] private GameObject container = default;
        private void Awake()
        {
            if (!ConfigService.IsInitialized())
            {
                Instantiate(container);
            }
        }
    }
}
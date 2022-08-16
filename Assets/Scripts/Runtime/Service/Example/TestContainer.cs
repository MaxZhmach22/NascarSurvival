using UnityEngine;

namespace Octavian.Runtime.Service.Example
{
    public class TestContainer : MonoBehaviour
    {
        private void Start()
        {
            var so = ConfigService.Get<SomeSO2>().Text;
            print(so);
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace Octavian.Runtime.Service.Example
{
    public class ExampleScript : MonoBehaviour
    {
        [SerializeField]
        private Button _buttonNextLevel = null;

        private void Start()
        {
            //_buttonNextLevel.onClick.AddListener(() => LevelManager.LoadNextScene());
        }
    }
}

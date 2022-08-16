using System.Collections.Generic;
using System.Linq;
using Octavian.Runtime.Extensions;
using UnityEngine;

namespace Octavian.Runtime.Service
{
    public class ConfigService : MonoBehaviour
    {
        [SerializeField]
        private SettingsContainer settingsContainer = default;
        
        private static List<ScriptableObject> _sources = new List<ScriptableObject>();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            settingsContainer.sources.ForEach(AddSettings);
        }

        private void OnDestroy()
        {
            _sources.ForEach(t =>
            {
                t.Save();
            });
            _sources.Clear();
        }

        private void AddSettings(ScriptableObject setting)
        {
            setting = Instantiate(setting);
            setting.Open();
            _sources.Add(setting);
        }

        public static T Get<T>() where T : ScriptableObject
        {
           var so = _sources.FirstOrDefault(t => t is T) as T;
           return so;
        }
        
        public static bool IsInitialized()
        {
            return _sources.Count > 0;
        }
    }
}
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Octavian.Runtime.Service
{
    [CreateAssetMenu(fileName = "Container", menuName = "Settings/Container", order = 0)]
    public class SettingsContainer : ScriptableObject
    {
        [ReorderableList]
        public List<ScriptableObject> sources = new List<ScriptableObject>();
    }
}
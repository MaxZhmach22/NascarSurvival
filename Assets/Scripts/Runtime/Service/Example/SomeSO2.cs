using UnityEngine;

namespace Octavian.Runtime.Service.Example
{
    [CreateAssetMenu(fileName = "Some2", menuName = "Settings/Some2", order = 0)]
    public class SomeSO2 : ScriptableObject
    {
        public string Text = "some2";
    }
}
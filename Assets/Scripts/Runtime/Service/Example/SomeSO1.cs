using UnityEngine;

namespace Octavian.Runtime.Service.Example
{
    [CreateAssetMenu(fileName = "Some1", menuName = "Settings/Some1", order = 0)]
    public class SomeSO1 : ScriptableObject
    {
        public string Text = "some1";
    }
}
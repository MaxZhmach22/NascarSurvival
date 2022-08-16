using UnityEngine;

namespace Octavian.Runtime.Extensions
{
    public static class ScriptableObjectExtension
    {
        public static void Save(this ScriptableObject a)
        {
            var json = JsonUtility.ToJson(a);
            PlayerPrefs.SetString(a.name, json);
            PlayerPrefs.Save();
        }

        public static void Open(this ScriptableObject a)
        {
            if (PlayerPrefs.HasKey(a.name))
            {
                JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(a.name), a);
            }
        }
    }
}
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;


namespace Dron
{
    public class ShadowOff
    {
        [MenuItem("Octavian/Shadow off", priority = 0)]
        public static void ShadowsOff()
        {
            var meshRenderers = GameObject.FindObjectsOfType<MeshRenderer>();

            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
                meshRenderer.receiveShadows = false;
            }

#if UNITY_EDITOR
            Debug.Log("Octavian: Shadows off.");
#endif
        }

        [MenuItem("Octavian/Shadow on", priority = 0)]
        public static void Undo()
        {
            var meshRenderers = GameObject.FindObjectsOfType<MeshRenderer>();

            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.shadowCastingMode = ShadowCastingMode.On;
                meshRenderer.receiveShadows = true;
            }

#if UNITY_EDITOR
            Debug.Log("Octavian: Shadows on.");
#endif
        }
    }
}

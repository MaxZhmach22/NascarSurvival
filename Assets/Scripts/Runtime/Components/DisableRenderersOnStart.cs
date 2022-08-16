using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace Octavian.Runtime.Components
{
    [DisallowMultipleComponent]
    public class DisableRenderersOnStart : MonoBehaviour
    {
        [InfoBox("Disables all this gameObject's & it's children's renderers on Start(). List of renderers is below.")]
        [SerializeField]
        private List<Renderer> renderers;

        private void Awake()
        {
            renderers = GetComponentsInChildren<Renderer>().ToList();
        }

        private void Start()
        {
            renderers.ForEach(r => r.enabled = false);
        }
    }
}
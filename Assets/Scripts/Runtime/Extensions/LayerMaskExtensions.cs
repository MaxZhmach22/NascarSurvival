using UnityEngine;

namespace Octavian.Runtime.Extensions
{
    public static class LayerMaskExtensions
    {
        public static int ConvertToInt(this LayerMask layerMask)
        {
            return Mathf.RoundToInt(Mathf.Log(layerMask.value, 2));
        }
    }
}
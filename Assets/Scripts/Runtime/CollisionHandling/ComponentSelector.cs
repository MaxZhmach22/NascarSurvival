using UnityEngine;

namespace Octavian.Runtime.CollisionHandling
{
    public static class ComponentSelector
    {
        public static T OnGameObject<T>(Collider collider)
        {
            return collider.GetComponent<T>();
        }
        
        public static T InParent<T>(Collider collider)
        {
            return collider.GetComponentInParent<T>();
        }
        
        public static T InChildren<T>(Collider collider)
        {
            return collider.GetComponentInChildren<T>();
        }
        
        public static T AllHierarchy<T>(Collider collider)
        {
            return collider.transform.root.GetComponentInChildren<T>();
        }
    }
}
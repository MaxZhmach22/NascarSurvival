using System.Reflection;
using UnityEngine;

namespace Octavian.Runtime.DebugTools
{
    public static class Tools
    {
        /// <summary>
        /// Draws rectangle representing part of the plane
        /// </summary>
        public static void DrawPlane(Vector3 position, Vector3 normal)
        {
#if UNITY_EDITOR
            Vector3 vector;

            if (normal.normalized != Vector3.forward)
                vector = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
            else
                vector = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude;

            var corner0 = position + vector;
            var corner2 = position - vector;
            var quaternion = Quaternion.AngleAxis(90f, normal);
            vector = quaternion * vector;
            var corner1 = position + vector;
            var corner3 = position - vector;

            Debug.DrawLine(corner0, corner2, Color.green);
            Debug.DrawLine(corner1, corner3, Color.green);
            Debug.DrawLine(corner0, corner1, Color.green);
            Debug.DrawLine(corner1, corner2, Color.green);
            Debug.DrawLine(corner2, corner3, Color.green);
            Debug.DrawLine(corner3, corner0, Color.green);
            Debug.DrawRay(position, normal, Color.red);
#endif
        }
        
        /// <summary>
        /// Clears all entries from editor console.
        /// </summary>
        public static void ClearConsole()
        {
#if UNITY_EDITOR
            var logEntries = System.Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
            var clearMethod = logEntries.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);
            clearMethod.Invoke(null, null);
#endif
        }
    }
}

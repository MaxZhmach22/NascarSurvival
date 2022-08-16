using Octavian.Runtime.Utility;
using UnityEngine;

namespace Octavian.Runtime.DestructibleBuilding.Messages
{
    public interface IDestructibleBuildingSettings
    {
        public Vector3 CubeSize { get; }
        public FloatRange PushForce { get; }
        public FloatRange SinglePushForce { get; }
    }
}
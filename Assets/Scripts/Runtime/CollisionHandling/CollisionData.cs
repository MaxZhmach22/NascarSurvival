using UnityEngine;

namespace Octavian.Runtime.CollisionHandling
{
    public readonly struct CollisionData
    {
        public CollisionData(Collider other, CollisionPhase collisionPhase)
        {
            Other = other;
            CollisionPhase = collisionPhase;
        }

        public Collider Other { get; }
        
        public CollisionPhase CollisionPhase { get; }
    }
}
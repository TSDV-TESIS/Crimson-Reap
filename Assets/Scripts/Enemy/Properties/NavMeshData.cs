using System;

namespace Enemy.Properties
{
    [Serializable]
    public class NavMeshData
    {
        public float Acceleration;
        public float MaxSpeed;
        public float AngularVelocity;
        public float SlowdownDistance;
    }
}
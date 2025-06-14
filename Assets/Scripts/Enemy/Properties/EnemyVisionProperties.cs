using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Enemy Vision Properties", fileName = "EnemyVisionProperties")]
    public class EnemyVisionProperties : ScriptableObject
    {
        public float visionLength = 2f;
        public float visionAngle = 70f;
        public float anglePerRaycast = 5f;
        public LayerMask whatIsObjective;
        public LayerMask whatIsObstruction;
        public float minAnglePerRaycast = 5f;
        public float stillSeeingPlayerSeconds = 3f;
        public bool shouldDrawGizmos = false;
    }
}
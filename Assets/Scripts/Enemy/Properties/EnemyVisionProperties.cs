using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Enemy Vision Properties", fileName = "EnemyVisionProperties")]
    public class EnemyVisionProperties : ScriptableObject
    {
        public float visionLength = 2f;
        public float minAnglePerRaycast = 5f;
        
        [Header("Vision in Y")]
        public float visionAngleInY = 70f;
        public float anglePerRaycastInY = 5f;

        [Header("Vision in X")] 
        public float visionAngleInX = 30f;
        public float anglePerRaycastInX = 5f;
        
        public LayerMask whatIsObjective;
        public LayerMask whatIsObstruction;
        public float stillSeeingPlayerSeconds = 3f;
        public bool shouldDrawGizmos = false;
    }
}
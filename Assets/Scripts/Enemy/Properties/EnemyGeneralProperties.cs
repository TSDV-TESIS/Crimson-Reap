using UnityEngine;

namespace Enemy.Properties
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Enemy General Properties", fileName = "EnemyGeneralProperties")]
    public class EnemyGeneralProperties : ScriptableObject
    {
        [Header("General Properties")] 
        public float attackRange;
        public float hearingRadius;

        [Header("Scanning Properties")] 
        public float minRotationTime;
        public float maxRotationTime;

        [Header("Investigation Properties")] 
        public float investigationTime;

        [Header("Suspicious Properties")] 
        public NavMeshData suspiciousNavmeshData;

        [Header("Chase Properties")] 
        public NavMeshData chaseNavmeshData;

        [Header("Patrol Properties")] 
        public NavMeshData patrolNavmeshData;
        public float patrolPointWaitTimeSeconds;

        [Header("Attack properties")] 
        public float enemySpeedAttacking;
        public float attackNoiseLevel;
        public float attackDuration;
        public float attackStartTime;
        public float attackIframesDuration;
        public bool shouldShowAreaDebug;

        [Header("Debug")] 
        public bool shouldDrawGizmos;
    }
}

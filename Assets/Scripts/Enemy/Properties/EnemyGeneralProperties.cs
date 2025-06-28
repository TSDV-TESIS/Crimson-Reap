using Unity.Behavior;
using UnityEngine;

namespace Enemy.Properties
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Enemy General Properties", fileName = "EnemyGeneralProperties")]
    public class EnemyGeneralProperties : ScriptableObject
    {
        [Header("Hearing Properties")] 
        public float hearingRadius;
        public LayerMask hearingOcclussionMask;

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
        public BehaviorGraph attackSubgraph;
        public float attackRange;
        public float enemySpeedAttacking;
        public float attackNoiseLevel;
        public float attackCooldown;
        public float attackStartTime;
        public float attackIframesDuration;
        public bool shouldShowAreaDebug;

        [Header("Debug")] 
        public bool shouldDrawGizmos;

    }
}

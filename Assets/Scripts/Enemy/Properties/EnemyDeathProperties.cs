using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Enemy Death Properties", fileName = "EnemyDeathProperties")]
    public class EnemyDeathProperties : ScriptableObject
    {
        public int healthRewardOnDeath = 15;
        public float disableSeconds = 0.5f;
        public float screamingRadius = 10f;

        public LayerMask screamingOcclussionMask;
        public bool shouldDrawGizmos = false;
        public float hitStopTimeSeconds = 0.1f;
    }
}
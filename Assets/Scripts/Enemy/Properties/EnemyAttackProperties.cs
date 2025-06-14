using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Enemy Attack Properties", fileName = "EnemyAttackProperties")]
    public class EnemyAttackProperties : ScriptableObject
    {
        public float enemySpeedInAttack = 2f;
        public float attackWindupSeconds = 1f;
        public float attackIFrameSeconds = 0.5f;
        public float attackWindoffSeconds = 0.5f;

        public bool shouldShowAttackAreaDebug = true;
    }
}
using UnityEngine;

namespace Enemy
{
    public class EnemyAnimationHandler : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private static readonly int Death = Animator.StringToHash("Death");

        public void OnDead()
        {
            animator.SetTrigger(Death);
        }
    }
}

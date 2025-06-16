using UnityEngine;

namespace Player.Controllers
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator playerAnimator;
        private static readonly int Walking = Animator.StringToHash("Walking");
        private static readonly int Attack1 = Animator.StringToHash("AttackTrigger");
        private static readonly int Falling = Animator.StringToHash("Falling");
        private static readonly int Jump = Animator.StringToHash("Jump");

        public void HandleWalk()
        {
            playerAnimator.SetBool(Walking, true);
        }

        public void HandleIdle()
        {
            playerAnimator.SetBool(Walking, false);
        }

        public void HandleAttack()
        {
            Debug.LogWarning("ATTACK TRIGGER SET!");
            playerAnimator.SetTrigger(Attack1);
        }

        public void HandleStopAttack()
        {
        }

        public void HandleJump()
        {
            playerAnimator.SetBool(Jump, true);
            playerAnimator.SetBool(Falling, true);
        }

        public void HandleFalling()
        {
            playerAnimator.SetBool(Falling, true);
            playerAnimator.SetBool(Jump, false);
        }

        public void HandleOffJump()
        {
            playerAnimator.SetBool(Jump, false);
        }

        public void HandleGrounded()
        {
            playerAnimator.SetBool(Falling, false);
        }
    }
}

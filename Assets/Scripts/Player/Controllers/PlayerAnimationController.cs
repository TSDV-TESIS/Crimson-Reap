using Player.Properties;
using UnityEngine;

namespace Player.Controllers
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private PlayerMovementProperties properties;
        private static readonly int Walking = Animator.StringToHash("Velocity");
        private static readonly int Attack1 = Animator.StringToHash("Attack");
        private static readonly int Falling = Animator.StringToHash("Falling");
        private static readonly int Jump = Animator.StringToHash("Jump");

        public void HandleWalk(float velocity)
        {
            float velocityToUse = Mathf.Abs(velocity);
            playerAnimator.SetFloat(Walking, velocityToUse / properties.maxSpeed);
        }

        public void HandleAttack()
        {
            playerAnimator.SetTrigger(Attack1);
        }

        public void HandleJump()
        {
            playerAnimator.SetTrigger(Jump);
            playerAnimator.SetBool(Falling, true);
        }

        public void HandleFalling()
        {
            playerAnimator.SetBool(Falling, true);
        }

        public void HandleGrounded()
        {
            playerAnimator.SetBool(Falling, false);
        }
    }
}

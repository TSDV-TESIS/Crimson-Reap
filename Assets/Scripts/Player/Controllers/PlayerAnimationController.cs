using System.Collections;
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
        private static readonly int Dead = Animator.StringToHash("Dead");
        
        private Coroutine _jumpSetValuesCoroutine;
        
        public void HandleDeath()
        {
            playerAnimator.SetTrigger(Dead);
        }
        
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
            if(_jumpSetValuesCoroutine != null) StopCoroutine(_jumpSetValuesCoroutine);
            _jumpSetValuesCoroutine = StartCoroutine(JumpSetValues());
        }

        private IEnumerator JumpSetValues()
        {
            // Animator needs to read the trigger first then set the boolean, if not falling transition
            // occurs before jump
            playerAnimator.SetTrigger(Jump);
            yield return null;
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

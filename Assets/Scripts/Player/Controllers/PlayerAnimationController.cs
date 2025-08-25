using System;
using System.Collections;
using Player.Properties;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player.Controllers
{
    [RequireComponent(typeof(PlayerAgent))]
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private PlayerMovementProperties properties;
        [SerializeField] private PlayerAnimationProperties animationProperties;
        
        private static readonly int Walking = Animator.StringToHash("Velocity");
        private static readonly int Attack1 = Animator.StringToHash("Attack");
        private static readonly int Falling = Animator.StringToHash("IsFalling");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Dead = Animator.StringToHash("Dead");
        private static readonly int Interact = Animator.StringToHash("Interact");
        private static readonly int IsWallSliding = Animator.StringToHash("IsWallSliding");
        private static readonly int IsInShadowstep = Animator.StringToHash("IsInShadowstep");
        private static readonly int Step = Animator.StringToHash("ShadowStep");
        private static readonly int Glitch = Animator.StringToHash("Glitch");
        
        private PlayerAgent _agent;
        private float _secondsToGlitch;

        private void OnEnable()
        {
            _agent ??= GetComponent<PlayerAgent>();
            SetSecondsToGlitch();
        }

        public void Update()
        {
            if (_secondsToGlitch < Time.time)
            {
                playerAnimator.SetTrigger(Glitch);
                SetSecondsToGlitch();
            }
        }

        private void SetSecondsToGlitch()
        {
            _secondsToGlitch = Time.time + Random.Range(animationProperties.minSecondsToGlitch,
                animationProperties.maxSecondsToGlitch);
        }

        public void HandleShadowstep(bool value)
        {
            playerAnimator.SetBool(IsInShadowstep, value);
            if(value) playerAnimator.SetTrigger(Step);
        }
        
        public void HandleWallsliding(bool value)
        {
            playerAnimator.SetBool(IsWallSliding, value);
            if(!value) playerAnimator.ResetTrigger(Jump);
        }
        
        public void HandleInteract()
        {
            if(_agent.IsGrounded())
                playerAnimator.SetTrigger(Interact);
        }
        
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

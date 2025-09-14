using System;
using System.Collections;
using Player.Properties;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player.Controllers
{
    [RequireComponent(typeof(PlayerAgent))]
    [RequireComponent(typeof(PlayerMovement))]
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
        private static readonly int Knockback = Animator.StringToHash("Knockback");
        private static readonly int RunRotate = Animator.StringToHash("RunRotate");

        private PlayerAgent _agent;
        private PlayerMovement _playerMovement;
        private float _secondsToGlitch;
        private float _lastMovementDirection;
        private bool _shouldResetMoveDirection;
        private static readonly int StopRunning = Animator.StringToHash("StopRunning");

        private void OnEnable()
        {
            _agent ??= GetComponent<PlayerAgent>();
            _playerMovement ??= GetComponent<PlayerMovement>();
            _lastMovementDirection = 0f;
            SetSecondsToGlitch();
        }

        public void Update()
        {
            if (_secondsToGlitch < Time.time)
            {
                playerAnimator.SetTrigger(Glitch);
                SetSecondsToGlitch();
            }

            if (_shouldResetMoveDirection)
            {
                _shouldResetMoveDirection = false;
                _lastMovementDirection = 0f;
            }
        }

        private void SetSecondsToGlitch()
        {
            _secondsToGlitch = Time.time + Random.Range(animationProperties.minSecondsToGlitch,
                animationProperties.maxSecondsToGlitch);
        }

        public void HandleKnockback()
        {
            _shouldResetMoveDirection = true;
            playerAnimator.SetTrigger(Knockback);
        }

        public void HandleShadowstep(bool value)
        {
            _shouldResetMoveDirection = true;
            playerAnimator.SetBool(IsInShadowstep, value);
            if (value) playerAnimator.SetTrigger(Step);
        }

        public void HandleWallsliding(bool value)
        {
            _shouldResetMoveDirection = true;
            playerAnimator.SetBool(IsWallSliding, value);
            if (!value) playerAnimator.ResetTrigger(Jump);
        }

        public void HandleInteract()
        {
            if (_agent.IsGrounded())
                playerAnimator.SetTrigger(Interact);
        }

        public void HandleDeath()
        {
            playerAnimator.SetTrigger(Dead);
        }

        public void HandleWalk(float velocity)
        {
            float moveDirection = _playerMovement.MoveDirection.x;
            float velocityToUse = Mathf.Abs(velocity);

            if (Math.Abs(moveDirection - _lastMovementDirection) > properties.minStopAnimSpeedPercentage &&
                Math.Abs(moveDirection) < properties.minStopMovePercentage) playerAnimator.SetTrigger(StopRunning);

            playerAnimator.SetFloat(Walking, velocityToUse / properties.maxSpeed);
            _lastMovementDirection = moveDirection;
        }

        public void HandleAttack()
        {
            Debug.Log("Hello ATTACK");
            _shouldResetMoveDirection = true;
            playerAnimator.SetTrigger(Attack1);
        }

        public void HandleJump()
        {
            _shouldResetMoveDirection = true;
            playerAnimator.SetTrigger(Jump);
            playerAnimator.SetBool(Falling, true);
        }

        public void HandleFalling()
        {
            _shouldResetMoveDirection = true;
            playerAnimator.SetBool(Falling, true);
        }

        public void HandleGrounded()
        {
            _shouldResetMoveDirection = true;
            playerAnimator.SetBool(Falling, false);
        }
    }
}
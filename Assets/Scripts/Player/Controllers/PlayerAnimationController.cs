using System;
using System.Collections;
using Events;
using Events.Scriptables;
using Health;
using Player.Properties;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Player.Controllers
{
    [RequireComponent(typeof(PlayerAgent))]
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private GameObject model;
        [SerializeField] private PlayerMovementProperties properties;
        [SerializeField] private PlayerAnimationProperties animationProperties;
        [SerializeField] private PlayerRotation playerRotation;

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
        private static readonly int StopRunning = Animator.StringToHash("StopRunning");
        private static readonly int TimeDeath = Animator.StringToHash("TimeDeath");
        private static readonly int SpikeDeath = Animator.StringToHash("SpikeDeath");
        private static readonly int AcidDeath = Animator.StringToHash("AcidDeath");

        private PlayerAgent _agent;
        private PlayerMovement _playerMovement;
        private float _secondsToGlitch;
        private float _lastMovementDirection;
        private float _lastVelocityDirection;
        private bool _shouldResetMoveDirection;
        private bool _isRotating;
        private Coroutine _stopRunningAnimationCoroutine;

        private bool _isLockedRotation;
        private Coroutine _lockRotationCoroutine;

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

        public void HandleDeath(DeathCauses cause)
        {
            switch (cause)
            {
                case DeathCauses.External:
                    playerAnimator.SetTrigger(Dead);
                    break;
                case DeathCauses.Internal:
                    playerAnimator.SetTrigger(TimeDeath);
                    break;
                case DeathCauses.Spikes:
                    playerAnimator.SetTrigger(SpikeDeath);
                    break;
                case DeathCauses.Acid:
                    playerAnimator.SetTrigger(AcidDeath);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(cause), cause, null);
            }
        }

        public void HandleWalk(float velocity)
        {
            float moveDirection = _playerMovement.MoveDirection.x;
            float velocityToUse = Mathf.Abs(velocity);

            if (velocity != 0 && !Mathf.Approximately(Mathf.Sign(velocity), Mathf.Sign(model.transform.forward.x)) &&
                !playerRotation.LockRotation)
            {
                playerRotation.LockRotation = true;

                playerAnimator.SetTrigger(RunRotate);
            }

            if (Math.Abs(moveDirection - _lastMovementDirection) > properties.minStopAnimSpeedPercentage &&
                Math.Abs(moveDirection) < properties.minStopMovePercentage)
            {
                HandleStopRunningCoroutine();
            }

            playerAnimator.SetFloat(Walking, velocityToUse / properties.maxSpeed);
            _lastMovementDirection = moveDirection;
        }

        public void HandleAttack()
        {
            _shouldResetMoveDirection = true;
            playerAnimator.SetTrigger(Attack1);
            ResetRotate();
        }

        private void ResetRotate()
        {
            playerRotation.LockRotation = false;
            playerAnimator.ResetTrigger(RunRotate);
        }

        public void HandleJump()
        {
            _shouldResetMoveDirection = true;
            playerAnimator.SetTrigger(Jump);
            playerAnimator.SetBool(Falling, true);
            ResetRotate();
        }

        public void HandleFalling()
        {
            _shouldResetMoveDirection = true;
            playerAnimator.ResetTrigger(RunRotate);
            playerAnimator.SetBool(Falling, true);
            ResetRotate();
        }

        public void HandleGrounded()
        {
            _shouldResetMoveDirection = true;
            playerAnimator.SetBool(Falling, false);
        }

        private void HandleStopRunningCoroutine()
        {
            if (_stopRunningAnimationCoroutine != null) StopCoroutine(_stopRunningAnimationCoroutine);
            _stopRunningAnimationCoroutine = StartCoroutine(StopRunningCoroutine());
        }

        private IEnumerator StopRunningCoroutine()
        {
            yield return new WaitForSeconds(animationProperties.stopRunningAnimationWaitTime);
            if (!(Math.Abs(_playerMovement.MoveDirection.x) < properties.minStopMovePercentage)) yield break;
            ResetRotate();
            playerAnimator.SetTrigger(StopRunning);
        }
    }
}
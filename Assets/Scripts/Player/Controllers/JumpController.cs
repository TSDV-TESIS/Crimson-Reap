using System;
using System.Collections;
using FSM;
using Player.Properties;
using UnityEngine;

namespace Player.Controllers
{
    [RequireComponent(typeof(PlayerMovement))]
    public class JumpController : Controller<PlayerAgent>
    {
        private PlayerMovement _playerMovement;
        [SerializeField] private PlayerMovementProperties playerMovementProperties;
        [SerializeField] private InputHandler inputHandler;

        private Coroutine _changeToWallslideCheckCoroutine;
        private Coroutine _changeToFasterFallingCheckCoroutine;
        private float _timeInJump;
        private bool _isJumpCancelled;

        private void OnEnable()
        {
            _timeInJump = 0f;
            _isJumpCancelled = false;
            
            _changeToWallslideCheckCoroutine = null;
            _changeToFasterFallingCheckCoroutine = null;
            
            _playerMovement ??= GetComponent<PlayerMovement>();
            _playerMovement.ShouldAddFasterFallingValues = false;
            
            inputHandler?.OnPlayerShadowStep.AddListener(HandleShadowstep);
            inputHandler?.OnPlayerJumpCancelled.AddListener(OnJumpCancel);
            inputHandler?.OnPlayerJump.AddListener(OnJumpEnabled);
            inputHandler?.OnPlayerAttack.AddListener(OnAttack);
        }

        private void OnDisable()
        {
            _changeToWallslideCheckCoroutine = null;
            _changeToFasterFallingCheckCoroutine = null;
           
            inputHandler?.OnPlayerShadowStep.RemoveListener(OnJumpCancel);
            inputHandler?.OnPlayerJump.AddListener(OnJumpEnabled);
            inputHandler?.OnPlayerShadowStep.RemoveListener(HandleShadowstep);
            inputHandler?.OnPlayerAttack.RemoveListener(OnAttack);
        }

        private void OnJumpEnabled()
        {
            _isJumpCancelled = false;
        }

        private void Jump()
        {
            _playerMovement.Jump();
        }
        
        private void OnJumpCancel()
        {
            if (_timeInJump >= playerMovementProperties.maxCancelTime) return;
            
            _isJumpCancelled = true;
        }

        private void HandleShadowstep()
        {
            if (!agent.MovementChecks.CanShadowStepOnAir()) return;

            agent.ChangeStateToShadowStep();
        }

        private void OnAttack()
        {
            agent.ChangeStateToAttack();
        }

        public override void OnUpdate()
        {
            _playerMovement.HandleWalk();
            _playerMovement.FreeFall();
            _timeInJump += Time.deltaTime;

            if (_timeInJump > playerMovementProperties.minJumpTimeForCancel && _isJumpCancelled)
            {
                _playerMovement.JumpCancel();
                _isJumpCancelled = false;
            }

            if (agent.MovementChecks.IsFalling(_playerMovement.Velocity))
                agent.ChangeStateToFalling();

            if (agent.MovementChecks.IsNearCorner(out var cornerDisplace))
            {
                _playerMovement.Move(new Vector3(cornerDisplace, 0, 0));
            }
            else
            {
                if (agent.MovementChecks.IsNearCeiling())
                {
                    _playerMovement.SetVerticalVelocity(-playerMovementProperties.gravity * Time.deltaTime);
                    agent.ChangeStateToFalling();
                }
            }

            if (!agent.MovementChecks.IsInOffGroundGraceTime() && agent.MovementChecks.IsGrounded())
            {
                agent.ChangeStateToGrounded();
            }

            if (agent.MovementChecks.ShouldWallSlide(_playerMovement.MoveDirection, _playerMovement.Velocity))
            {
                if (_changeToWallslideCheckCoroutine != null) return;
                _changeToWallslideCheckCoroutine = StartCoroutine(
                    DelayedCheck(
                        () => !agent.MovementChecks.ShouldWallSlide(_playerMovement.MoveDirection,
                            _playerMovement.Velocity),
                        agent.ChangeStateToWallSlide
                    )
                );
            }

            if (agent.MovementChecks.IsDoingDropdown())
            {
                if (_changeToFasterFallingCheckCoroutine != null) return;
                _changeToFasterFallingCheckCoroutine = StartCoroutine(
                    DelayedCheck(
                        () => agent.MovementChecks.IsDoingDropdown(),
                        agent.ChangeStateToFasterFalling)
                );
            }
        }

        private IEnumerator DelayedCheck(Func<bool> isStillDoingAction, Action changeFunc)
        {
            float timeDoingAction = 0f;

            while (timeDoingAction < playerMovementProperties.maxJumpTimeDelayForActions)
            {
                Debug.Log("WAITING!");
                if (!isStillDoingAction())
                {
                    Debug.Log("BREAKING.");
                    yield break;
                }

                timeDoingAction += Time.deltaTime;
                yield return null;
            }

            changeFunc();
        }
    }
}
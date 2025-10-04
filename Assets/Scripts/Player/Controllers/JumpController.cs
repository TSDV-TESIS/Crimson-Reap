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

        private void OnEnable()
        {
            _changeToWallslideCheckCoroutine = null;
            _playerMovement ??= GetComponent<PlayerMovement>();
            inputHandler?.OnPlayerShadowStep.AddListener(HandleShadowstep);
            inputHandler?.OnPlayerAttack.AddListener(OnAttack);
        }

        private void OnDisable()
        {
            _changeToWallslideCheckCoroutine = null;
            inputHandler?.OnPlayerShadowStep.RemoveListener(HandleShadowstep);
            inputHandler?.OnPlayerAttack.RemoveListener(OnAttack);
        }

        private void Jump()
        {
            _playerMovement.Jump();
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

            if (agent.MovementChecks.IsFalling(_playerMovement.Velocity))
                agent.ChangeStateToFalling();

            float cornerDisplace = 0;

            if (agent.MovementChecks.IsNearCorner(out cornerDisplace))
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
                _changeToWallslideCheckCoroutine = StartCoroutine(CheckWallslide());
            }

            if (_playerMovement.IsGoingDownFaster())
            {
                agent.ChangeStateToFasterFalling();
            }
        }

        private IEnumerator CheckWallslide()
        {
            float timeInWallInit = 0f;

            while (timeInWallInit < playerMovementProperties.maxJumpTimeNearWall)
            {
                Debug.Log("WAITING!");
                if (!agent.MovementChecks.ShouldWallSlide(_playerMovement.MoveDirection, _playerMovement.Velocity))
                {
                    Debug.Log("BREAKING.");
                    yield break;
                }
                
                timeInWallInit += Time.deltaTime;
                yield return null;
            }
            
            agent.ChangeStateToWallSlide();
        }
    }

}

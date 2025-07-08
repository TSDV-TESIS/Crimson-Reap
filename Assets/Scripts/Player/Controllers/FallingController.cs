using System;
using FSM;
using Player.Properties;
using UnityEngine;

namespace Player.Controllers
{
    [RequireComponent(typeof(PlayerMovement))]
    public class FallingController : Controller<PlayerAgent>
    {
        private PlayerMovement _playerMovement;
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private PlayerMovementProperties playerMovementProperties;

        private bool _canJump;
        private bool _isFromGroundedTransition;
        private void OnEnable()
        {
            _playerMovement ??= GetComponent<PlayerMovement>();
            inputHandler?.OnPlayerShadowStep.AddListener(HandleShadowstep);
            inputHandler?.OnPlayerAttack.AddListener(OnAttack);
            inputHandler?.OnPlayerJump.AddListener(HandleCoyoteJump);
        }

        private void OnDisable()
        {
            inputHandler?.OnPlayerShadowStep.RemoveListener(HandleShadowstep);
            inputHandler?.OnPlayerAttack.RemoveListener(OnAttack);
            inputHandler?.OnPlayerJump.RemoveListener(HandleCoyoteJump);
            _isFromGroundedTransition = false;
        }

        public void SetIsFromGroundedTransition()
        {
            _isFromGroundedTransition = true;
        }
        
        private void HandleCoyoteJump()
        {
            if (_canJump && _isFromGroundedTransition)
            {
                agent.ChangeStateToJumping();
            }
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

            _canJump = agent.MovementChecks.IsInGroundedCoyoteTime();
            
            if (agent.MovementChecks.IsNearCeiling())
            {
                _playerMovement.SetVerticalVelocity(-playerMovementProperties.gravity * Time.deltaTime);
            }

            if (!agent.MovementChecks.IsInOffGroundGraceTime() && agent.MovementChecks.IsGrounded())
            {
                agent.ChangeStateToGrounded();
            }

            if (agent.MovementChecks.ShouldWallSlide(_playerMovement.MoveDirection, _playerMovement.Velocity))
                agent.ChangeStateToWallSlide();
        }
    }
}
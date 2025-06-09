using System;
using FSM;
using UnityEngine;

namespace Player.Controllers
{
    [RequireComponent(typeof(PlayerMovement))]
    public class GroundedController : Controller<PlayerAgent>
    {
        private PlayerMovement _playerMovement;
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private float unboundWallBufferSeconds = 0.75f;

        
        private void OnEnable()
        {
            _playerMovement ??= GetComponent<PlayerMovement>();
            inputHandler.OnPlayerJump.AddListener(OnJump);
            inputHandler.OnPlayerShadowStep.AddListener(OnShadowstep);
            inputHandler.OnPlayerAttack.AddListener(OnAttack);

            float clearance = agent.MovementChecks.GetGroundClearance();
            if (clearance == 0) return;

            _playerMovement.Grounded(clearance);
        }

        private void OnDisable()
        {
            inputHandler.OnPlayerJump.RemoveListener(OnJump);
            inputHandler.OnPlayerShadowStep.RemoveListener(OnShadowstep);
            inputHandler.OnPlayerAttack.RemoveListener(OnAttack);
        }

        public override void OnUpdate()
        {
            _playerMovement.HandleGroundedWalk(agent.MovementChecks.GetSlopeMovementDirection(_playerMovement.MoveDirection));
            _playerMovement.HandleDeceleration();

            if (agent.MovementChecks.IsNearCeiling() && _playerMovement.Velocity.y > 0)
                _playerMovement.SetVerticalVelocity(0);

            if (!agent.MovementChecks.IsGrounded())
                agent.ChangeStateToFalling();
        }

        private void OnJump()
        {
            agent.ChangeStateToJumping();
        }

        private void OnShadowstep()
        {
            agent.ChangeStateToShadowStep();
        }

        private void OnAttack()
        {
            agent.ChangeStateToAttack();
        }
    }
}
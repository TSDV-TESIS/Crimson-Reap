using FSM;
using Player.Properties;
using UnityEngine;

namespace Player.Controllers
{
    public abstract class FallingController : Controller<PlayerAgent>
    {
        protected PlayerMovement PlayerMovementActions;
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private PlayerMovementProperties playerMovementProperties;
        
        private bool _canJump;
        protected bool IsFromGroundedTransition;
        
        protected virtual void OnEnable()
        {
            PlayerMovementActions ??= GetComponent<PlayerMovement>();
            inputHandler?.OnPlayerShadowStep.AddListener(HandleShadowstep);
            inputHandler?.OnPlayerAttack.AddListener(OnAttack);
            inputHandler?.OnPlayerJump.AddListener(HandleCoyoteJump);
        }

        protected virtual void OnDisable()
        {
            inputHandler?.OnPlayerShadowStep.RemoveListener(HandleShadowstep);
            inputHandler?.OnPlayerAttack.RemoveListener(OnAttack);
            inputHandler?.OnPlayerJump.RemoveListener(HandleCoyoteJump);
        }

        private void HandleCoyoteJump()
        {
            if (_canJump && IsFromGroundedTransition)
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
            PlayerMovementActions.HandleWalk();
            PlayerMovementActions.FreeFall();

            _canJump = agent.MovementChecks.IsInGroundedCoyoteTime();
            
            if (agent.MovementChecks.IsNearCeiling())
            {
                PlayerMovementActions.SetVerticalVelocity(-playerMovementProperties.gravity * Time.deltaTime);
            }

            if (!agent.MovementChecks.IsInOffGroundGraceTime() && agent.MovementChecks.IsGrounded())
            {
                agent.ChangeStateToGrounded();
            }

            if (agent.MovementChecks.ShouldWallSlide(PlayerMovementActions.MoveDirection, PlayerMovementActions.Velocity))
                agent.ChangeStateToWallSlide();
        }
    }
}
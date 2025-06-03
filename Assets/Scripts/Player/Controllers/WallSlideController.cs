using System;
using FSM;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Player.Controllers
{
    [RequireComponent(typeof(PlayerMovement))]
    public class WallSlideController : Controller<PlayerAgent>
    {
        [SerializeField] private InputHandler input;
        private PlayerMovement _movement;

        [Header("Events")] 
        [SerializeField] private UnityEvent<Vector3, int> onWallHitEnter;

        private bool _isActive;
        private void OnEnable()
        {
            _movement ??= GetComponent<PlayerMovement>();
            _isActive = false;
        }

        private void OnDisable()
        {
            if(_isActive)
                input.OnPlayerJump.RemoveListener(OnJump);
        }

        public void OnEnter()
        {
            _isActive = true;
            input.OnPlayerJump.AddListener(OnJump);
            input.OnPlayerShadowStep.AddListener(OnShadowstep);
            input.OnPlayerAttack.AddListener(OnAttack);
            onWallHitEnter.Invoke(agent.MovementChecks.WallrideHitPosition, agent.MovementChecks.WallSlideDirection);
        }

        public override void OnUpdate()
        {
            _movement.WallSlide();

            if (agent.MovementChecks.IsGrounded())
            {
                _movement.Grounded(agent.MovementChecks.GetGroundClearance());
                agent.ChangeStateToGrounded();
            }

            if (agent.MovementChecks.ShouldUnboundWallslide(_movement.MoveDirection, _movement.Velocity))
                agent.ChangeStateToFalling();
        }

        public void OnLeave()
        {
            _isActive = false;
            input.OnPlayerJump.RemoveListener(OnJump);
            input.OnPlayerShadowStep.RemoveListener(OnShadowstep);
            input.OnPlayerAttack.RemoveListener(OnAttack);
        }

        private void OnJump()
        {
            _movement.WallJump(agent.MovementChecks.WallSlideDirection);
            agent.MovementChecks.StopCheckingWall();
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
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


        private void OnEnable()
        {
            _playerMovement ??= GetComponent<PlayerMovement>();
            inputHandler?.OnPlayerShadowStep.AddListener(HandleShadowstep);
            inputHandler?.OnPlayerAttack.AddListener(OnAttack);
        }

        private void Start()
        {
            inputHandler?.OnPlayerShadowStep.RemoveListener(HandleShadowstep);
            inputHandler?.OnPlayerAttack.RemoveListener(OnAttack);
        }

        private void OnDisable()
        {
            inputHandler?.OnPlayerShadowStep.RemoveListener(HandleShadowstep);
            inputHandler?.OnPlayerAttack.RemoveListener(OnAttack);
        }

        private void HandleShadowstep()
        {
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

            if (agent.MovementChecks.IsNearCeiling())
            {
                _playerMovement.SetVerticalVelocity(-playerMovementProperties.gravity * Time.deltaTime);
            }

            if (agent.MovementChecks.IsGrounded())
            {
                _playerMovement.Grounded(agent.MovementChecks.GetGroundHitPoint().y);
                agent.ChangeStateToGrounded();
            }

            if (agent.MovementChecks.ShouldWallSlide(_playerMovement.MoveDirection, _playerMovement.Velocity))
                agent.ChangeStateToWallSlide();
        }
    }
}
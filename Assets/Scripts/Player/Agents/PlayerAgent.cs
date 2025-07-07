using System;
using System.Collections.Generic;
using FSM;
using Health;
using Player.Checks;
using Player.Controllers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerAgent : Agent
    {
        [SerializeField] private bool logChanges;
        [SerializeField] private bool canWallSlide;
        [SerializeField] private HealthPoints healthPoints;
        [SerializeField] private PlayerMovementChecks playerMovementChecks;
        [SerializeField] private PlayerAttackChecks playerAttackChecks;

        [Header("Internal Events")]
        [SerializeField] private ActionEventsWrapper groundedEvents;
        [SerializeField] private ActionEventsWrapper jumpingEvents;
        [SerializeField] private ActionEventsWrapper fallingEvents;
        [SerializeField] private ActionEventsWrapper wallSlidingEvents;
        [SerializeField] private ActionEventsWrapper shadowStepEvents;
        [SerializeField] private ActionEventsWrapper attackEvents;
        
        [Header("Internal Transitions Events")]
        [SerializeField] private UnityEvent onGroundToFallTransition;
        
        private State _groundedState;
        private State _jumpingState;
        private State _fallingState;
        private State _wallSlideState;
        private State _shadowStepState;
        private State _attackState;

        public PlayerMovementChecks MovementChecks
        {
            get => playerMovementChecks;
        }

        public PlayerAttackChecks AttackChecks
        {
            get => playerAttackChecks;
        }


        protected override List<State> GetStates()
        {
            _groundedState = new State();
            _groundedState.EnterAction += groundedEvents.ExecuteOnEnter;
            _groundedState.UpdateAction += groundedEvents.ExecuteOnUpdate;
            _groundedState.ExitAction += groundedEvents.ExecuteOnExit;

            _jumpingState = new State();
            _jumpingState.EnterAction += jumpingEvents.ExecuteOnEnter;
            _jumpingState.UpdateAction += jumpingEvents.ExecuteOnUpdate;
            _jumpingState.ExitAction += jumpingEvents.ExecuteOnExit;

            _fallingState = new State();
            _fallingState.EnterAction += fallingEvents.ExecuteOnEnter;
            _fallingState.UpdateAction += fallingEvents.ExecuteOnUpdate;
            _fallingState.ExitAction += fallingEvents.ExecuteOnExit;

            _wallSlideState = new State();
            _wallSlideState.EnterAction += wallSlidingEvents.ExecuteOnEnter;
            _wallSlideState.UpdateAction += wallSlidingEvents.ExecuteOnUpdate;
            _wallSlideState.ExitAction += wallSlidingEvents.ExecuteOnExit;

            _shadowStepState = new State();
            _shadowStepState.EnterAction += shadowStepEvents.ExecuteOnEnter;
            _shadowStepState.UpdateAction += shadowStepEvents.ExecuteOnUpdate;
            _shadowStepState.ExitAction += shadowStepEvents.ExecuteOnExit;

            _attackState = new State();
            _attackState.EnterAction += attackEvents.ExecuteOnEnter;
            _attackState.UpdateAction += attackEvents.ExecuteOnUpdate;
            _attackState.ExitAction += attackEvents.ExecuteOnExit;

            Transition groundedToJumping = new Transition(_groundedState, _jumpingState);
            _groundedState.AddTransition(groundedToJumping);
            Transition groundedToFalling = new Transition(_groundedState, _fallingState);
            groundedToFalling.AddTransitionAction(onGroundToFallTransition.Invoke);
            _groundedState.AddTransition(groundedToFalling);

            Transition groundedToShadowStep = new Transition(_groundedState, _shadowStepState);
            _groundedState.AddTransition(groundedToShadowStep);
            Transition groundedToAttack = new Transition(_groundedState, _attackState);
            _groundedState.AddTransition(groundedToAttack);

            Transition jumpingToGrounded = new Transition(_jumpingState, _groundedState);
            _jumpingState.AddTransition(jumpingToGrounded);
            Transition jumpingToFalling = new Transition(_jumpingState, _fallingState);
            _jumpingState.AddTransition(jumpingToFalling);
            Transition jumpingToWallSlide = new Transition(_jumpingState, _wallSlideState);
            _jumpingState.AddTransition(jumpingToWallSlide);
            Transition jumpingToShadowStep = new Transition(_jumpingState, _shadowStepState);
            _jumpingState.AddTransition(jumpingToShadowStep);
            Transition jumpingToAttack = new Transition(_jumpingState, _attackState);
            _jumpingState.AddTransition(jumpingToAttack);

            Transition fallingToJumping = new Transition(_fallingState, _jumpingState);
            _fallingState.AddTransition(fallingToJumping);
            Transition fallingToGrounded = new Transition(_fallingState, _groundedState);
            _fallingState.AddTransition(fallingToGrounded);
            Transition fallingToWallSlide = new Transition(_fallingState, _wallSlideState);
            _fallingState.AddTransition(fallingToWallSlide);
            Transition fallingToShadowStep = new Transition(_fallingState, _shadowStepState);
            _fallingState.AddTransition(fallingToShadowStep);
            Transition fallingToAttack = new Transition(_fallingState, _attackState);
            _fallingState.AddTransition(fallingToAttack);

            Transition wallSlideToGrounded = new Transition(_wallSlideState, _groundedState);
            _wallSlideState.AddTransition(wallSlideToGrounded);
            Transition wallSlideToJumping = new Transition(_wallSlideState, _jumpingState);
            _wallSlideState.AddTransition(wallSlideToJumping);
            Transition wallSlideToFalling = new Transition(_wallSlideState, _fallingState);
            _wallSlideState.AddTransition(wallSlideToFalling);
            Transition wallSlideToShadowStep = new Transition(_wallSlideState, _shadowStepState);
            _wallSlideState.AddTransition(wallSlideToShadowStep);
            Transition wallSlideToAttack = new Transition(_wallSlideState, _attackState);
            _wallSlideState.AddTransition(wallSlideToAttack);

            Transition shadowStepToGrounded = new Transition(_shadowStepState, _groundedState);
            _shadowStepState.AddTransition(shadowStepToGrounded);
            Transition shadowStepToFalling = new Transition(_shadowStepState, _fallingState);
            _shadowStepState.AddTransition(shadowStepToFalling);
            Transition shadowStepToWallRiding = new Transition(_shadowStepState, _wallSlideState);
            _shadowStepState.AddTransition(shadowStepToWallRiding);
            Transition shadowStepToAttack = new Transition(_shadowStepState, _attackState);
            _shadowStepState.AddTransition(shadowStepToAttack);

            Transition attackToGrounded = new Transition(_attackState, _groundedState);
            _attackState.AddTransition(attackToGrounded);
            Transition attackToFalling = new Transition(_attackState, _fallingState);
            _attackState.AddTransition(attackToFalling);
            Transition attackToWallRiding = new Transition(_attackState, _wallSlideState);
            _attackState.AddTransition(attackToWallRiding);
            Transition attackToShadowStep = new Transition(_attackState, _shadowStepState);
            _attackState.AddTransition(attackToShadowStep);

            return new List<State>()
            {
                _groundedState,
                _jumpingState,
                _fallingState,
                _wallSlideState,
                _shadowStepState,
                _attackState
            };
        }

        public void ChangeStateToGrounded()
        {
            LogMessage("Change state to grounded");
            Fsm.ChangeState(_groundedState);
        }

        public void ChangeStateToJumping()
        {
            LogMessage("Change state to jumping");
            Fsm.ChangeState(_jumpingState);
        }

        public void ChangeStateToFalling()
        {
            LogMessage("Change state to falling");
            Fsm.ChangeState(_fallingState);
        }

        public void ChangeStateToWallSlide()
        {
            if (canWallSlide)
            {
                LogMessage("Change state to wallslide");
                Fsm.ChangeState(_wallSlideState);
            }
        }

        public void ChangeStateToShadowStep()
        {
            if (MovementChecks.IsShadowStepOnCooldown)
            {
                LogMessage("Dash Was On Cooldown");
                return;
            }

            LogMessage("Change state to Shadowstep");
            Fsm.ChangeState(_shadowStepState);
        }

        public void ChangeStateToAttack()
        {
            if (!AttackChecks.CanAttack()) return;

            LogMessage("Change state to Attack");
            Fsm.ChangeState(_attackState);
        }

        public void LogMessage(String message)
        {
            if (logChanges)
            {
                Debug.Log(message);
            }
        }

        public void StopFsm()
        {
            Fsm.Disable();
        }

        public bool IsGrounded()
        {
            return Fsm.GetCurrentState() == _groundedState;
        }
    }
}
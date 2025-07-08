using System;
using System.Collections;
using Events;
using FSM;
using Health;
using Player.Properties;
using UnityEngine;

namespace Player.Controllers
{
    [RequireComponent(typeof(PlayerMovement))]
    public class ShadowStepController : Controller<PlayerAgent>
    {
        [SerializeField] private PlayerMovementProperties playerMovementProperties;
        [SerializeField] private GameObject bloodStepCollider;
        [SerializeField] private Vector3 displacementIfBlocked = new Vector3(0.01f, 0.01f, 0);
        
        private PlayerMovement _playerMovement;
        private MouseLook _mouseLook;
        private CharacterController _characterController;
        private CapsuleCollider _collider;
        private HealthPoints _healthPoints;
        private Coroutine _shadowstepCoroutine;

        private bool _shouldExitShadowstep;
        
        private void OnEnable()
        {
            _playerMovement ??= GetComponent<PlayerMovement>();
            _mouseLook ??= GetComponent<MouseLook>();
            _characterController ??= GetComponent<CharacterController>();
            _collider ??= GetComponent<CapsuleCollider>();
            _healthPoints ??= GetComponent<HealthPoints>();
        }

        public void OnEnter()
        {
            _shouldExitShadowstep = false;
            if (_shadowstepCoroutine != null) StopCoroutine(_shadowstepCoroutine);
            _shadowstepCoroutine = StartCoroutine(Shadowstep());
        }

        public override void OnUpdate()
        {
            if(_shouldExitShadowstep) ExitShadowstep();
        }

        private IEnumerator Shadowstep()
        {
            Debug.Log("START Shadowstep");
            float timer = 0;
            Vector2 direction = _mouseLook.CursorDir.normalized;

            _characterController.excludeLayers |= playerMovementProperties.avoidableObjects;
            _collider.excludeLayers |= playerMovementProperties.avoidableObjects;

            float duration = playerMovementProperties.shadowStepTime;

            while (timer < duration)
            {
                if (timer >= playerMovementProperties.shadowStepIframes.x && timer <= playerMovementProperties.shadowStepIframes.y)
                    _healthPoints.SetCanTakeDamage(false);
                else
                    _healthPoints.SetCanTakeDamage(true);
                
                _playerMovement.Shadowstep(direction);
                
                timer += Time.deltaTime;
                yield return null;
            }
            
            _characterController.excludeLayers ^= playerMovementProperties.avoidableObjects;
            _collider.excludeLayers ^= playerMovementProperties.avoidableObjects;
            
            _characterController.Move(displacementIfBlocked);
            _shouldExitShadowstep = true;
        }

        private void ExitShadowstep()
        {
            Debug.Log("Exiting");
            agent.MovementChecks.SetShadowstepOnCooldown();
            _playerMovement.ExitShadowstep();
            Debug.Log("Exited normal stuff");

            if (agent.MovementChecks.IsNearNonAvoidableWall())
            {
                Debug.Log("WALLSOLIDE");
                agent.ChangeStateToWallSlide();
            }
            else if (!agent.MovementChecks.IsGrounded())
            {
                Debug.Log("Not wallslide... falling!");
                agent.MovementChecks.SetShadowStepOnAirUsed();
                agent.ChangeStateToFalling();
            }
            else
            {
                Debug.Log("Grounded!");
                agent.ChangeStateToGrounded();
            }
        }
    }
}
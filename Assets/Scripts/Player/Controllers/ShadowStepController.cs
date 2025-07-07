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
        
        private PlayerMovement _playerMovement;
        private MouseLook _mouseLook;
        private CharacterController _characterController;
        private CapsuleCollider _collider;
        private HealthPoints _healthPoints;
        private Coroutine _shadowstepCoroutine;

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
            if (_shadowstepCoroutine != null) StopCoroutine(_shadowstepCoroutine);
            _shadowstepCoroutine = StartCoroutine(Shadowstep());
        }

        public override void OnUpdate()
        {
        }

        private IEnumerator Shadowstep()
        {
            float timer = 0;
            Vector2 direction = _mouseLook.CursorDir.normalized;
            bool changedToWallslide = false;

            _characterController.excludeLayers |= playerMovementProperties.avoidableObjects;
            _collider.excludeLayers |= playerMovementProperties.avoidableObjects;

            float duration = playerMovementProperties.shadowStepTime;

            while (timer < duration && !changedToWallslide)
            {
                if (timer >= playerMovementProperties.shadowStepIframes.x && timer <= playerMovementProperties.shadowStepIframes.y)
                    _healthPoints.SetCanTakeDamage(false);
                else
                    _healthPoints.SetCanTakeDamage(true);

                if (agent.MovementChecks.IsNearCeiling())
                {
                    _playerMovement.SetVerticalVelocity(-playerMovementProperties.gravity * Time.deltaTime);
                    _healthPoints.SetCanTakeDamage(true);
                    break;
                }

                
                _playerMovement.Shadowstep(direction);

                if (agent.MovementChecks.IsNearNonAvoidableWall())
                {
                    Debug.Log("HEREEEE");
                    changedToWallslide = true;
                    agent.MovementChecks.SetShadowstepOnCooldown();
                }
                timer += Time.deltaTime;
                yield return null;
            }

            _characterController.excludeLayers ^= playerMovementProperties.avoidableObjects;
            _collider.includeLayers ^= playerMovementProperties.avoidableObjects;
            
            ExitShadowstep();
        }

        private void ExitShadowstep()
        {
            Debug.Log("Exiting");
            agent.MovementChecks.SetShadowstepOnCooldown();
            _playerMovement.ExitShadowstep();

            if (agent.MovementChecks.IsNearNonAvoidableWall())
                agent.ChangeStateToWallSlide();
            else if (!agent.MovementChecks.IsGrounded())
            {
                agent.MovementChecks.SetShadowStepOnAirUsed();
                agent.ChangeStateToFalling();
            }
            else
                agent.ChangeStateToGrounded();
        }
    }
}
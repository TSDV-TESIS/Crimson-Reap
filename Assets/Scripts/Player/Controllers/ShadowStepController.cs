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
        [SerializeField] private ShadowStepProperties shadowStepProperties;
        [SerializeField] private GameObject bloodStepCollider;

        [Header("Events")]
        [SerializeField] private VoidEventChannelSO onFrenzyEnable;
        [SerializeField] private VoidEventChannelSO onFrenzyDisable;

        private bool _isFrenzied;

        private PlayerMovement _playerMovement;
        private MouseLook _mouseLook;
        private CharacterController _characterController;
        private HealthPoints _healthPoints;
        private Coroutine _shadowstepCoroutine;

        private void OnEnable()
        {
            _playerMovement ??= GetComponent<PlayerMovement>();
            _mouseLook ??= GetComponent<MouseLook>();
            _characterController ??= GetComponent<CharacterController>();
            _healthPoints ??= GetComponent<HealthPoints>();

            onFrenzyEnable?.onEvent.AddListener(HandleOnFrenzyEnabled);
            onFrenzyDisable?.onEvent.AddListener(HandleOnFrenzyDisabled);
        }

        private void OnDisable()
        {
            onFrenzyEnable?.onEvent.RemoveListener(HandleOnFrenzyEnabled);
            onFrenzyDisable?.onEvent.RemoveListener(HandleOnFrenzyDisabled);
        }

        private void HandleOnFrenzyDisabled()
        {
            _isFrenzied = false;
        }

        private void HandleOnFrenzyEnabled()
        {
            _isFrenzied = true;
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
            Debug.Log($"IS FRENZIED {_isFrenzied}");
            bool isBloodstep = _isFrenzied;
            float timer = 0;
            Vector2 direction = _mouseLook.CursorDir.normalized;
            bool changedToWallslide = false;

            _characterController.excludeLayers |= shadowStepProperties.avoidableObjects;

            float duration = playerMovementProperties.shadowStepTime;
            if (isBloodstep)
            {
                bloodStepCollider?.SetActive(true);
                duration = playerMovementProperties.bloodStepTime;
            }

            while (timer < duration)
            {
                if (timer >= playerMovementProperties.shadowStepIframes.x && timer <= playerMovementProperties.shadowStepIframes.y)
                {
                    _healthPoints.SetCanTakeDamage(false);
                    Debug.Log($"IFRAME");
                }
                else
                {
                    Debug.Log($"NO IFRAME");
                    _healthPoints.SetCanTakeDamage(true);
                }

                if (agent.MovementChecks.IsNearCeiling())
                {
                    _playerMovement.SetVerticalVelocity(-playerMovementProperties.gravity * Time.deltaTime);
                    break;
                }

                _playerMovement.Shadowstep(direction, isBloodstep);
                timer += Time.deltaTime;
                yield return null;
            }

            if (isBloodstep)
            {
                bloodStepCollider?.SetActive(false);
                onFrenzyDisable.RaiseEvent();
            }

            _characterController.excludeLayers ^= shadowStepProperties.avoidableObjects;

            if (agent.MovementChecks.IsNearWall())
            {
                changedToWallslide = true;
                agent.MovementChecks.SetShadowstepOnCooldown();
                agent.ChangeStateToWallSlide();
            }

            if (!changedToWallslide)
                ExitShadowstep();
        }

        private void ExitShadowstep()
        {
            agent.MovementChecks.SetShadowstepOnCooldown();
            _playerMovement.ExitShadowstep();

            if (agent.MovementChecks.IsNearWall())
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
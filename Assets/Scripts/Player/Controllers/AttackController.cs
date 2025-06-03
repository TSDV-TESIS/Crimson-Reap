using System;
using System.Collections;
using FSM;
using Player.Properties;
using UnityEngine;

namespace Player.Controllers
{
    public class AttackController : Controller<PlayerAgent>
    {
        [SerializeField] private PlayerAnimationController animationController;

        [SerializeField] private GameObject attackObject;

        [Header("Attack properties")]
        [SerializeField] private PlayerAttackProperties attackProperties;

        private ShadowStep _shadows;
        private PlayerMovement _playerMovement;
        private MouseLook _mouseLook;
        private Coroutine _attackCoroutine;

        private void OnEnable()
        {
            _playerMovement ??= GetComponent<PlayerMovement>();
            _mouseLook ??= GetComponent<MouseLook>();
            _shadows ??= GetComponent<ShadowStep>();
            HandleAttack();
        }

        public override void OnUpdate()
        {
        }

        private void HandleAttack()
        {
            if (_attackCoroutine != null) StopCoroutine(_attackCoroutine);

            _attackCoroutine = StartCoroutine(HandleAttackCoroutine());
        }

        private IEnumerator HandleAttackCoroutine()
        {
            animationController.HandleAttack();
            agent.AttackChecks.IsAttacking = true;
            attackObject.SetActive(true);
            float timer = 0;
            float startTime = Time.time;

            bool stopped = false;
            _shadows.InitShadowStepShadows();
            _playerMovement.Velocity = _mouseLook.CursorDir * attackProperties.displacementForce;
            if (agent.AttackChecks.IsNearGround() && _playerMovement.Velocity.y < 0)
            {
                _playerMovement.Velocity = new Vector2(_playerMovement.Velocity.x, 0);
            }
            while (timer < attackProperties.duration)
            {
                _playerMovement.Move(_playerMovement.Velocity * Time.deltaTime);
                timer = Time.time - startTime;
                yield return null;
            }

            StopAttack();
        }

        private void StopAttack()
        {
            animationController.HandleStopAttack();
            _shadows.StopShadows();
            attackObject.SetActive(false);
            agent.AttackChecks.IsAttacking = false;

            if (agent.AttackChecks.IsNearGround())
            {
                agent.ChangeStateToGrounded();
            }
            else
                agent.ChangeStateToFalling();
        }
    }
}
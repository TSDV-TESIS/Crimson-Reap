using System.Collections;
using Player.Properties;
using UnityEngine;

namespace Player.Checks
{
    public class PlayerAttackChecks : MonoBehaviour
    {
        [Header("Movement Properties")]
        [SerializeField] private PlayerMovementProperties movementProperties;
        [Header("Attack Properties")]
        [SerializeField] private PlayerAttackProperties attackProperties;
        [SerializeField] private GameObject attackObject;

        [HideInInspector] public bool IsAttackInCoolDown;
        [HideInInspector] public bool IsAttacking;

        private Coroutine _attackCoolDown;

        public bool CanAttack()
        {
            return !IsAttacking && !IsAttackInCoolDown;
        }

        public void SetAttackOnCoolDown()
        {
            if (_attackCoolDown != null)
                StopCoroutine(_attackCoolDown);

            _attackCoolDown = StartCoroutine(AttackCoolDown());
        }

        private IEnumerator AttackCoolDown()
        {
            IsAttackInCoolDown = true;
            yield return new WaitForSeconds(attackProperties.coolDownDuration);
            IsAttackInCoolDown = false;
        }

        public bool IsNearGround()
        {
            return Physics.Raycast(transform.position, attackObject.transform.forward, out RaycastHit _groundHit, attackProperties.checkDistance, movementProperties.whatIsGround);
        }

        private void OnDrawGizmos()
        {
            if (attackProperties.shouldDrawGizmos && attackObject.activeInHierarchy)
            {
                // Draw Attack raycast
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, transform.position + attackObject.transform.forward * attackProperties.checkDistance);
            }
        }
    }
}
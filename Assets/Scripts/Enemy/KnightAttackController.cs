using System;
using System.Collections;
using Enemy.Properties;
using Events;
using Player.Properties;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class EnemyAttackController : MonoBehaviour
    {
        [SerializeField] private GameObject attackObject;
        [SerializeField] private GameObject windUpAttackAreaObject;
        [SerializeField] private PlayerTransform playerTransform;
        [SerializeField] private EnemyGeneralProperties enemyProperties;

        private Coroutine _attackCoroutine;

        private NavMeshAgent _navMeshAgent;
        private MeshRenderer _attackRenderer;
        private MeshRenderer _windUpRenderer;

        private float _timeDiff = 0f;

        private void OnEnable()
        {
            _navMeshAgent ??= GetComponent<NavMeshAgent>();
            _attackRenderer ??= attackObject?.GetComponent<MeshRenderer>();
            _windUpRenderer ??= windUpAttackAreaObject?.GetComponent<MeshRenderer>();
        }

        public void StartAttack()
        {
            _timeDiff = 0f;
        }

        public Node.Status Attack()
        {
            
            if (_attackRenderer) _attackRenderer.enabled = enemyProperties.shouldShowAreaDebug;
            if (_windUpRenderer) _windUpRenderer.enabled = enemyProperties.shouldShowAreaDebug;

            _navMeshAgent.destination = playerTransform.playerTransform.position;
            _navMeshAgent.speed = enemyProperties.enemySpeedAttacking;
            windUpAttackAreaObject.SetActive(true);
            
            if (IsRunning(enemyProperties.attackStartTime)) return Node.Status.Running;
            windUpAttackAreaObject.SetActive(false);
            attackObject.SetActive(true);

            if (IsRunning(enemyProperties.attackStartTime + enemyProperties.attackIframesDuration))
                return Node.Status.Running;

            attackObject.SetActive(false);
            windUpAttackAreaObject.SetActive(true);

            windUpAttackAreaObject.SetActive(false);
            return Node.Status.Success;
        }

        private bool IsRunning(float seconds)
        {
            if (_timeDiff < seconds)
            {
                _timeDiff += Time.deltaTime;
                return true;
            }

            return false;
        }
    }
}
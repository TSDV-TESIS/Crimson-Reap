using System;
using System.Collections;
using Player.Properties;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(EnemyAgent))]
    public class EnemyAttackController : MonoBehaviour
    {
        [SerializeField] private GameObject attackObject;
        [SerializeField] private PlayerTransform playerTransform;
        [SerializeField] private float enemySpeedInAttack = 2f;
        [SerializeField] private float secondsInAttack = 2f;
        
        private Coroutine _attackCoroutine;

        private NavMeshAgent _navMeshAgent;
        private EnemyAgent _enemyAgent;

        private float _timeDiff = 0f;
        private void OnEnable()
        {
            _navMeshAgent ??= GetComponent<NavMeshAgent>();
            _enemyAgent ??= GetComponent<EnemyAgent>();
        }

        public void StartAttack()
        {
            _timeDiff = 0f;
        }
        
        public Node.Status Attack()
        {
            attackObject.SetActive(true);
            _navMeshAgent.destination = playerTransform.playerTransform.position;
            _navMeshAgent.speed = enemySpeedInAttack;

            
            while (_timeDiff < secondsInAttack)
            {
                _timeDiff += Time.deltaTime;
                return Node.Status.Running;
            }
            
            attackObject.SetActive(false);
            return Node.Status.Success;
        }
    }
}

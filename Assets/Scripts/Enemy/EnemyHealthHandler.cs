using System;
using System.Collections;
using Events;
using Events.Scriptables;
using Sounds;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Enemy
{
    [RequireComponent(typeof(BehaviorGraphAgent))]
    public class EnemyHealthHandler : MonoBehaviour
    {
        [SerializeField] private EnemyDeathProperties enemyDeathProperties;
        [SerializeField] private String isDeadVariableName = "IsDead";
        [SerializeField] private SoundCollisionHandler screamSoundCollisionHandler;
        [SerializeField] private GameObject[] objectsToDisable;

        [Header("Events")]
        [SerializeField] private GameObjectEventChannelSO onEnemyEnabled;
        [SerializeField] private IntEventChannelSO onEnemyDeath;
        [SerializeField] private UnityEvent<Vector3> onInternalDeath;
        [SerializeField] private GameObjectEventChannelSO onEnemyDisabled;
        [SerializeField] private TimeStopEventChannelSO onHitStop;

        private Coroutine _disableCoroutine;
        private BehaviorGraphAgent _behaviorAgent;

        private void OnEnable()
        {
            screamSoundCollisionHandler.SoundRadius = enemyDeathProperties.screamingRadius;
            _behaviorAgent ??= GetComponent<BehaviorGraphAgent>();
        }

        private void Start()
        {
            onEnemyEnabled?.RaiseEvent(gameObject);
        }

        public void HandleInitMaxHealth(int maxHealth)
        {
        }

        public void OnEnemyHit(int value)
        {
        }

        public void OnDeath()
        {
            _behaviorAgent.GetVariable(isDeadVariableName, out BlackboardVariable isDeadVariable);
            isDeadVariable.ObjectValue = true;
            
            onEnemyDeath?.RaiseEvent(enemyDeathProperties.healthRewardOnDeath);
            onInternalDeath?.Invoke(gameObject.transform.position);
            screamSoundCollisionHandler.EnableSound(enemyDeathProperties.shouldDrawGizmos);

            onHitStop?.RaiseEvent(enemyDeathProperties.hitstopProperties);

            foreach (GameObject obj in objectsToDisable)
            {
                obj.SetActive(false);
            }

            onEnemyDisabled?.RaiseEvent(gameObject);

            if (_disableCoroutine != null) StopCoroutine(_disableCoroutine);
            _disableCoroutine = StartCoroutine(DisableCoroutine());
        }
        
        private IEnumerator DisableCoroutine()
        {
            float timer = 0;
            while (timer < enemyDeathProperties.disableSeconds)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            foreach (GameObject obj in objectsToDisable)
            {
                obj.SetActive(true);
            }

            gameObject.SetActive(false);
        }
    }
}
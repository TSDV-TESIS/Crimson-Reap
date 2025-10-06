using System;
using System.Collections.Generic;
using Enemy;
using Events;
using Events.Scriptables;
using Health;
using Objects;
using Player.Properties;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Player.Attacks
{
    public class MeleeWeapon : MonoBehaviour
    {
        [Header("Position properties")] [SerializeField]
        private GameObject pivot;

        [Header("Damage properties")] [SerializeField]
        private PlayerAttackProperties playerAttackProperties;

        [Header("Internal Events")] 
        [SerializeField] private UnityEvent onHit;
        
        [Header("Events")] 
        [SerializeField] private VoidEventChannelSO onFrenziedEvent;
        [SerializeField] private FloatEventChannel onHitStop;
        [SerializeField] private AkWwiseEventChannelSO onPlayEvent;
        [SerializeField] private AK.Wwise.Event decapitationEvent;

        private readonly List<Collider> _hittedEnemies = new List<Collider>();

        private void OnDisable()
        {
            ResetHittedEnemiesBuffer();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!this.enabled || _hittedEnemies.Contains(other) || other.CompareTag("Player"))
                return;
            
            if (other.transform.TryGetComponent<ITakeDamage>(out ITakeDamage takeDamageInterface) && 
                !Physics.Linecast(
                    pivot.transform.position,
                    other.transform.position,
                    playerAttackProperties.attackOcclussion))
            {
                takeDamageInterface.TryTakeDamage(playerAttackProperties.damage);
                onHitStop?.RaiseEvent();
                onHit?.Invoke();
                _hittedEnemies.Add(other);

                if (other.gameObject.TryGetComponent<EnemyBeatHandler>(out EnemyBeatHandler enemyBeatHandler) &&
                    enemyBeatHandler.IsInHeartBeat && enemyBeatHandler.IsInBloodlust)
                {
                    onPlayEvent?.RaiseEvent(decapitationEvent);
                    onHitStop?.RaiseEvent(playerAttackProperties.hitStopSeconds);
                    onFrenziedEvent?.RaiseEvent();
                }
            }

            if (other.gameObject.TryGetComponent<IOpenable>(out IOpenable openable))
            {
                openable.Open();
            }
        }

        public void ResetHittedEnemiesBuffer()
        {
            _hittedEnemies.Clear();
        }
    }
}
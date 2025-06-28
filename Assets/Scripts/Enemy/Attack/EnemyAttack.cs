using System;
using Enemy.Properties;
using Health;
using Sounds;
using UnityEngine;

namespace Enemy.Attack
{
    public class EnemyAttack : MonoBehaviour
    {
        [SerializeField] private int damage = 10;
        [SerializeField] private GameObject attackSoundObject;
        [SerializeField] private EnemyGeneralProperties properties;

        private SoundCollisionHandler _handler;
        private void OnEnable()
        {
            _handler ??= attackSoundObject.GetComponent<SoundCollisionHandler>();
            _handler.gameObject.SetActive(true);
            _handler.SoundRadius = properties.attackNoiseLevel;
        }

        private void OnDisable()
        {
            _handler.gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.TryGetComponent<ITakeDamage>(out ITakeDamage takeDamageObject))
            {
                takeDamageObject.TryTakeDamage(damage);
            }
        }
    }
}

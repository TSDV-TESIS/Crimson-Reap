using System;
using Health;
using UnityEngine;

namespace Objects.Door
{
    [RequireComponent(typeof(Collider))]
    public class DoorContactKill : MonoBehaviour
    {
        public event Action onContact;
        [SerializeField] private DoorProperties doorProperties;

        private void OnTriggerEnter(Collider other)
        {
            if ((doorProperties.enemyLayer & (1 << other.gameObject.layer)) != 0)
                other?.GetComponent<ITakeDamage>()?.TryTakeDamage(doorProperties.openDamage);
        }

        public void TurnOff()
        {
            transform.GetComponent<Collider>().enabled = false;
        }

        public void SetTrigger()
        {
            transform.GetComponent<Collider>().enabled = true;
            transform.GetComponent<Collider>().isTrigger = true;
        }
    }
}
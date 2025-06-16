using System;
using Events;
using UnityEngine;
using UnityEngine.Serialization;

namespace Objects.Traps
{
    public class TrapContactHandler : MonoBehaviour
    {
        [SerializeField] private BaseTrap baseTrap;

        private void OnTriggerEnter(Collider other)
        {
            baseTrap.onTrapContact?.Invoke();
        }
    }
}
using System;
using Events;
using UnityEngine;
using UnityEngine.Serialization;

namespace Objects.Traps
{
    public class TrapContactHandler : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO onTrapContact;

        private void OnTriggerEnter(Collider other)
        {
            onTrapContact.RaiseEvent();
        }
    }
}
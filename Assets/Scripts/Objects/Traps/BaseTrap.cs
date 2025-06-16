using System;
using Events;
using UnityEngine;

namespace Objects.Traps
{
    public class BaseTrap : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO onTrapContact;
        [SerializeField] private VoidEventChannelSO instaKill;

        protected virtual void OnEnable()
        {
            onTrapContact.onEvent.AddListener(KillPlayer);
        }

        protected virtual void OnDisable()
        {
            onTrapContact.onEvent.RemoveListener(KillPlayer);
        }

        protected virtual void KillPlayer()
        {
            instaKill.RaiseEvent();
        }
    }
}
using System;
using Events;
using Events.Scriptables;
using UnityEngine;
using UnityEngine.Events;

namespace Objects.Traps
{
    public class BaseTrap : MonoBehaviour
    {
        [SerializeField] private StringEventChannelSO instaKill;
        public UnityEvent onTrapContact;
        protected virtual void OnEnable()
        {
            onTrapContact.AddListener(KillPlayer);
        }

        protected virtual void OnDisable()
        {
            onTrapContact.RemoveListener(KillPlayer);
        }

        protected virtual void KillPlayer()
        {
            instaKill.RaiseEvent("Spike");
        }
    }
}

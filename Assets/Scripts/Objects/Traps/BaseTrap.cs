using Events.Scriptables;
using Health;
using UnityEngine;
using UnityEngine.Events;

namespace Objects.Traps
{
    public class BaseTrap : MonoBehaviour
    {
        [SerializeField] private DeathEventChannelSO instaKill;
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
            instaKill.RaiseEvent(DeathCauses.Environment);
        }
    }
}
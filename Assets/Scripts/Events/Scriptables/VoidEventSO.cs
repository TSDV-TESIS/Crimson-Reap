using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Void Channel")]
    public class VoidEventChannelSO : ScriptableObject
    {
        public UnityEvent onEvent;
        public bool shouldLog = false;

        public void RaiseEvent()
        {
            if (onEvent != null)
            {
                if (shouldLog)
                {
                    Debug.Log($"{this.name} CALLED!");
                }
                onEvent.Invoke();
            }
            else
            {
                LogNullEventError();
            }
        }

        protected void LogNullEventError()
        {
            Debug.LogError($"{this.name} has no events. Please check if" +
                           $"events have been added correctly.");
        }
    }
}

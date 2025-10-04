using Events;
using Events.ScriptableObjects;
using Events.Scriptables;
using UnityEngine;

namespace CameraScripts
{
    public class CameraZoomTriggerHandler : MonoBehaviour
    {
        [SerializeField] private CameraZoomZoneProperties properties;
        [SerializeField] private CameraZoomZoneEventSO onZoomTriggerEnter;
        [SerializeField] private VoidEventChannelSO onZoomTriggerLeave;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                onZoomTriggerEnter?.onTypedEvent.Invoke(properties);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                onZoomTriggerLeave?.onEvent.Invoke();
            }
        }
    }
}
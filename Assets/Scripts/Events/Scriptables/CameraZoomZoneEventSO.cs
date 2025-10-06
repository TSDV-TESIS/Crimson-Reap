using CameraScripts;
using Events.ScriptableObjects;
using UnityEngine;

namespace Events.Scriptables
{
    [CreateAssetMenu(menuName = "CameraZoomZoneEvent", fileName = "Events/CameraZoomZoneEvent")]
    public class CameraZoomZoneEventSO : EventChannelSO<CameraZoomZoneProperties> {}
}

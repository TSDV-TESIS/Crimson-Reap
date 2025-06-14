using CameraScripts;
using Events.ScriptableObjects;
using UnityEngine;

namespace Events.Scriptables
{
    [CreateAssetMenu(menuName = "Events/CameraShakeProfile Channel")]
    public class ShakeProfileEventChannel : EventChannelSO<CameraShakeProfile>
    {
    }
}

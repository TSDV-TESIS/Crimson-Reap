using Events.ScriptableObjects;
using Unity.Behavior;
using UnityEngine;
using Utils;

namespace Events.Scriptables
{
    [CreateAssetMenu(menuName = "Events/Rumble channel", fileName = "RumbleChannel")]
    public class RumbleChannel : EventChannelSO<RumbleControllerData>
    {
    }
}

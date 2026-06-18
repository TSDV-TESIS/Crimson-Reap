using Events.ScriptableObjects;
using Health;
using UnityEngine;

namespace Events.Scriptables
{
    [CreateAssetMenu(menuName = "Events/DeathEvent")]
    public class DeathEventChannelSO : EventChannelSO<DeathCauses>
    {
    
    }
}
using UnityEngine;

namespace Player.Animations
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Spike Death Properties", fileName = "SpikeDeathProperties")]
    public class SpikeDeathProperties : ScriptableObject
    {
        public float timeToInsertInSpike = 0.5f;
        public float timeInserting = 0.1f;
        public float downVelocity = 0.5f;
    }
}

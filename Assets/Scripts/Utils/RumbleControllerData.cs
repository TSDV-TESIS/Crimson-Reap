using UnityEngine;

namespace Utils
{
    [CreateAssetMenu(fileName = "RumbleData", menuName = "Scriptable Objects/Rumble Data")]
    public class RumbleControllerData : ScriptableObject
    {
        public float duration;
        public float forceAmount;
    }
}
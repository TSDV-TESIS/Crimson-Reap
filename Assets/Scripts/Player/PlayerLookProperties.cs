using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "PlayerLookProperties", menuName = "Scriptable Objects/Player Look Properties")]
    public class PlayerLookProperties : ScriptableObject
    {
        public bool showDeadzone = false;
        public float deadZoneRadius = 1.5f;
    }
}
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "PlayerName", menuName = "Scriptable Objects/PlayerName")]
    public class PlayerName : ScriptableObject
    {
        public string playerName;
        public bool isInitialized;
    }
}
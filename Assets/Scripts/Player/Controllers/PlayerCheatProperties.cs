using UnityEngine;

namespace Player.Controllers
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Player Cheat Properties", fileName = "PlayerCheatProperties")]
    public class PlayerCheatProperties : ScriptableObject
    {
        public bool cheatsEnabled;
    }
}
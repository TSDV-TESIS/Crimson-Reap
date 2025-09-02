using UnityEngine;

namespace Player.Controllers
{
    [CreateAssetMenu(fileName = "PlayerAnimationProperties", menuName = "Scriptable Objects/Player Animation Properties")]
    public class PlayerAnimationProperties : ScriptableObject
    {
        public float minSecondsToGlitch;
        public float maxSecondsToGlitch;
    }
}
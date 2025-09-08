using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "MouseDataFromPlayer", menuName = "Scriptable Objects/Mouse Data From Player")]
    public class MouseDataFromPlayer : ScriptableObject
    {
        public Vector2 mouseDirection;
    }
}
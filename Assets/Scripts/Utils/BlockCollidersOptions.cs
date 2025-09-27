using UnityEngine;

namespace Utils
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Blockings Options", fileName = "BlockingsOptions")]
    public class BlockCollidersOptions : ScriptableObject
    {
        [Tooltip("Will hide the colliders when pressing play or on build.")]
        public bool shouldHideOnAwake;
        [Tooltip("Will replace the collider with a standard material.")]
        public bool shouldReplaceWithTheirMaterials;
    }
}
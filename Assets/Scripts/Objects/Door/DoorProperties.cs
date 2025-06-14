using UnityEngine;

namespace Objects
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Door Properties", fileName = "DoorProperties")]
    public class DoorProperties : ScriptableObject
    {
        public float doorSoundRadius = 3;
        public float doorOpenTime = 1;
        public LayerMask enemyLayer;
        public int openDamage = 1000;
    }
}
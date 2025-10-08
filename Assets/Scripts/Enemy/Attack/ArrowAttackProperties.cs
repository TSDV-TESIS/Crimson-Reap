using UnityEngine;

namespace Enemy.Attack
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Arrow Attack Properties", fileName = "ArrowAttackProperties")]
    public class ArrowAttackProperties : ScriptableObject
    {
        public LayerMask whatIsStoppableColliders;
        public float destroySeconds;
        public float velocity;

        public string vfxStartEvent;
        public string vfxStopEvent;
        public string hitVfxStartEvent;
        public string hitJesterVfxStartEvent;
    }
}
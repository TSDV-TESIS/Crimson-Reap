using UnityEngine;
using UnityEngine.Serialization;

namespace Objects.Traps.Saw
{
    [CreateAssetMenu(fileName = "Saw Properties", menuName = "Traps/Saw", order = 0)]
    public class SawProperties : ScriptableObject
    {
        public float speed;
        [Tooltip("If Loop Bounces loop starts moving backwards after reaching last waypoint (n->n-1->n-2...0), Otherwise last waypoint's Next is first waypoint (n->0->1...->n)")]
        public bool shouldLoopBounce = false;
        public float distError = 0.02f;
    }
}
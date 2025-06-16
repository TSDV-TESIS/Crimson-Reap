using UnityEngine;

namespace TimeScripts
{
    [CreateAssetMenu(fileName = "TimeStop Properties", menuName = "Time/TimeStopProperties", order = 0)]
    public class TimeFreezeProperties : ScriptableObject
    {
        public float duration;
        [Tooltip("0 represents an absolute freeze and 1 the default timeScale")]
        [Range(0, 1)]
        public float slowDown;
    }
}
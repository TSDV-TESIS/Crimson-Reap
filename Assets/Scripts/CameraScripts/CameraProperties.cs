using UnityEngine;

namespace CameraScripts
{
    [CreateAssetMenu(fileName = "CameraProperties", menuName = "Camera/Properties", order = 0)]
    public class CameraProperties : ScriptableObject
    {
        [Header("Position Composer Properties\n")]
        public Vector2 screenPosition;
        public bool deadZone;
        public Vector2 deadZoneSize;

        public Vector3 targetOffset;
        public Vector3 damping;

        public bool lookAhead;
        [Range(0, 1)]
        public float lookAheadTime;
        [Range(0, 30)]
        public float lookAheadSmoothing;
        public bool ignoreY;
    }
}
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

        [Header("Pivot Follow")]
        public bool drawPivotGizmos;
        [Range(0, 100)]
        public float pivotSpeed;
        [Range(0, 10)]
        public float pivotMinDistance;

        [Header("Cursor Influence Over Pivot")]
        public bool shouldInfluence;
        [Range(0, 1)]
        public float cursorInfluence;
        public bool freeCamera;
        public float pivotMaxDistance = 5;
    }
}
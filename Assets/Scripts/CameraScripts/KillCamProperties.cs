using UnityEngine;

namespace CameraScripts
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Killcam Properties", fileName = "KillCamProperties")]
    public class KillCamProperties : ScriptableObject
    {
        [Header("General Properties")] 
        public float killCamDuration = 2f;
        
        [Header("Camera properties")] 
        [Tooltip("The max camera distance zoom that the kill-cam will do")]
        public float maxCameraZoom = 5f;
        [Tooltip("The camera values through the kill-cam")]
        public AnimationCurve cameraZoomAnimation = AnimationCurve.Constant(0, 1, 1);
        [Tooltip("The max camera FOV that the kill-cam will do")]
        public float maxCameraFov = 45f;
        [Tooltip("The FOV values through the kill-cam")]
        public AnimationCurve cameraFovAnimation = AnimationCurve.Constant(0, 1, 1);

        [Header("Time properties")] 
        [Tooltip("The min slowdown time that the kill-cam will do")]
        [Range(0f, 1f)] public float minDeltaTime = 0.5f;
        [Tooltip("The delta time values through the killcam")]
        public AnimationCurve deltaTimeAnimation = AnimationCurve.Constant(0, 1, 1);
    }
}
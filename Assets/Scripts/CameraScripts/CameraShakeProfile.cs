using Unity.Cinemachine;
using UnityEngine;

namespace CameraScripts
{
    [CreateAssetMenu(fileName = "CameraShakeProfile", menuName = "Camera/ShakeProfile", order = 0)]
    public class CameraShakeProfile : ScriptableObject
    {
        public float duration;
        public NoiseSettings.TransformNoiseParams noiseParams;
    }
}
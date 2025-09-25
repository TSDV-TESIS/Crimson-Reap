using System.Collections;
using Events.Scriptables;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace CameraScripts
{
    public class CameraController : MonoBehaviour
    {
        [FormerlySerializedAs("_properties")] [SerializeField] private CameraProperties properties;
        [FormerlySerializedAs("camera")] [SerializeField] private CinemachineCamera mainCamera;
        [SerializeField] private CinemachinePositionComposer composer;
        [SerializeField] private CinemachineBasicMultiChannelPerlin shakeController;
        [SerializeField] private NoiseSettings shakeSettings;

        [SerializeField] private ShakeProfileEventChannel onCameraShakeEventChannelSo;

        private Coroutine _cameraShake;

        private void Awake()
        {
            SetComposerSettings();
        }

        private void Start()
        {
            onCameraShakeEventChannelSo.onTypedEvent.AddListener(HandleCameraShake);
        }

        private void OnDestroy()
        {
            onCameraShakeEventChannelSo.onTypedEvent.RemoveListener(HandleCameraShake);
        }

        private void SetComposerSettings()
        {
            mainCamera.Lens.FieldOfView = properties.FOV;
            composer.CameraDistance = properties.cameraDistance;

            composer.Composition.ScreenPosition = properties.screenPosition;
            composer.Composition.DeadZone.Enabled = properties.deadZone;
            composer.Composition.DeadZone.Size = properties.deadZoneSize;

            composer.TargetOffset = properties.targetOffset;
            composer.Damping = properties.damping;
        }

        private void HandleCameraShake(CameraShakeProfile shakeProfile)
        {
            if (_cameraShake != null)
                StopCoroutine(_cameraShake);

            _cameraShake = StartCoroutine(CameraShake(shakeProfile.noiseParams, shakeProfile.duration));
        }

        private IEnumerator CameraShake(NoiseSettings.TransformNoiseParams noiseParams, float duration)
        {
            shakeController.enabled = true;
            shakeSettings.PositionNoise[0] = noiseParams;
            shakeController.NoiseProfile = shakeSettings;
            yield return new WaitForSeconds(duration);
            shakeController.NoiseProfile = null;
            shakeController.enabled = false;
        }
    }
}
using System.Collections;
using Events.Scriptables;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace CameraScripts
{
    public class CameraController : MonoBehaviour
    {
        [FormerlySerializedAs("camera")] [SerializeField] private CinemachineCamera mainCamera;
        [SerializeField] private CinemachinePositionComposer composer;
        [SerializeField] private CinemachineBasicMultiChannelPerlin shakeController;
        [SerializeField] private NoiseSettings shakeSettings;

        [SerializeField] private ShakeProfileEventChannel onCameraShakeEventChannelSo;

        private CameraProperties _cameraProperties;
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
            mainCamera.Lens.FieldOfView = _cameraProperties.FOV;
            composer.CameraDistance = _cameraProperties.cameraDistance;

            composer.Composition.ScreenPosition = _cameraProperties.screenPosition;
            composer.Composition.DeadZone.Enabled = _cameraProperties.deadZone;
            composer.Composition.DeadZone.Size = _cameraProperties.deadZoneSize;

            composer.TargetOffset = _cameraProperties.targetOffset;
            composer.Damping = _cameraProperties.damping;
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

        public void SetNewFov(CameraProperties cameraProperties)
        {
            _cameraProperties = cameraProperties;
            SetComposerSettings();
        }
    }
}
using System;
using System.Collections;
using Events.Scriptables;
using Unity.Cinemachine;
using UnityEngine;

namespace CameraScripts
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CameraProperties _properties;
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
            composer.Composition.ScreenPosition = _properties.screenPosition;
            composer.Composition.DeadZone.Enabled = _properties.deadZone;
            composer.Composition.DeadZone.Size = _properties.deadZoneSize;

            composer.TargetOffset = _properties.targetOffset;
            composer.Damping = _properties.damping;
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
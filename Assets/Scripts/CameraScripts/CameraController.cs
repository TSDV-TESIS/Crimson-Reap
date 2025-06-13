using System;
using Unity.Cinemachine;
using UnityEngine;

namespace CameraScripts
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CameraProperties _properties;
        [SerializeField] private CinemachinePositionComposer composer;

        private void Awake()
        {
            SetComposerSettings();
        }

        private void SetComposerSettings()
        {
            composer.Composition.ScreenPosition = _properties.screenPosition;
            composer.Composition.DeadZone.Enabled = _properties.deadZone;
            composer.Composition.DeadZone.Size = _properties.deadZoneSize;

            composer.TargetOffset = _properties.targetOffset;
            composer.Damping = _properties.damping;

            composer.Lookahead.Enabled = _properties.lookAhead;
            composer.Lookahead.Time = _properties.lookAheadTime;
            composer.Lookahead.Smoothing = _properties.lookAheadSmoothing;
            composer.Lookahead.IgnoreY = _properties.ignoreY;
        }
    }
}
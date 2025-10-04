using System;
using System.Collections.Generic;
using Events;
using Events.Scriptables;
using Player;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace CameraScripts
{
    [RequireComponent(typeof(CameraController))]
    [RequireComponent(typeof(KillCamController))]
    public class FovController : MonoBehaviour
    {
        [SerializeField] private FovsData camerasData;
        [SerializeField] private List<GameObject> postProcessingObjs;
        [SerializeField] private InputHandler input;
        
        [Header("Events")] 
        [SerializeField] private CameraZoomZoneEventSO onCameraZoomZoneEnter;
        [SerializeField] private VoidEventChannelSO onCameraZoomZoneLeave;
        
        private CameraController _cameraController;
        private KillCamController _killCamController;
        private float _lastFocusDistance;
        private float _lastFocalLength;
        
        void OnEnable()
        {
            input?.onChangeFov.AddListener(HandleChangeFov);

            _cameraController ??= GetComponent<CameraController>();
            _killCamController ??= GetComponent<KillCamController>();
            
            foreach (GameObject postProcessingObj in postProcessingObjs)
            {
                postProcessingObj.SetActive(false);
            }
            
            postProcessingObjs[camerasData.activeCameraPropertyIndex].SetActive(true);
            SetFovs();
            
            onCameraZoomZoneEnter?.onTypedEvent.AddListener(HandleCameraZoomZoneEnter);
            onCameraZoomZoneLeave?.onEvent.AddListener(HandleCameraZoomZoneLeave);
        }

        private void OnDisable()
        {
            input?.onChangeFov.RemoveListener(HandleChangeFov);
            
            onCameraZoomZoneEnter?.onTypedEvent.RemoveListener(HandleCameraZoomZoneEnter);
            onCameraZoomZoneLeave?.onEvent.RemoveListener(HandleCameraZoomZoneLeave);
        }
        
        private void HandleCameraZoomZoneLeave()
        {
            if(!TryGetDepthOfFieldComponent(out DepthOfField depthValues)) return;

            depthValues.focusDistance.value = _lastFocusDistance;
            depthValues.focalLength.value = _lastFocalLength;
        }

        private void HandleCameraZoomZoneEnter(CameraZoomZoneProperties properties)
        {
            if(!TryGetDepthOfFieldComponent(out DepthOfField depthValues)) return;

            depthValues.focusDistance.value = properties.focalDistanceInZoom;
            depthValues.focalLength.value = properties.focalLengthInZoom;
        }

        private bool TryGetDepthOfFieldComponent(out DepthOfField depthOfFieldComponent)
        {
            if (!postProcessingObjs[camerasData.activeCameraPropertyIndex].TryGetComponent<Volume>(out Volume volume))
            {
                Debug.LogError("Volume not found");
                depthOfFieldComponent = null;
                return false;
            }

            if (!volume.sharedProfile.TryGet<DepthOfField>(out DepthOfField obtainedValue))
            {
                Debug.LogError("depth of field not found");
                depthOfFieldComponent = null;
                return false;
            }

            depthOfFieldComponent = obtainedValue;
            return true;
        }

        private void HandleChangeFov()
        {
            postProcessingObjs[camerasData.activeCameraPropertyIndex].SetActive(false);
            
            camerasData.activeCameraPropertyIndex++;
            if (camerasData.activeCameraPropertyIndex >= camerasData.cameraProperties.Count)
            {
                camerasData.activeCameraPropertyIndex = 0;
            }
            
            postProcessingObjs[camerasData.activeCameraPropertyIndex].SetActive(true);
            SetFovs();
        }

        private void SetFovs()
        {
            _cameraController.SetNewFov(camerasData.cameraProperties[camerasData.activeCameraPropertyIndex]);
            _killCamController.SetNewFov(camerasData.cameraProperties[camerasData.activeCameraPropertyIndex]);

            if (!TryGetDepthOfFieldComponent(out DepthOfField depthOfFieldComponent)) return;

            _lastFocalLength = depthOfFieldComponent.focalLength.value;
            _lastFocusDistance = depthOfFieldComponent.focusDistance.value;
        }
    }
}

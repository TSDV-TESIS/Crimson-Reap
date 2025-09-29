using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace CameraScripts
{
    [RequireComponent(typeof(CameraController))]
    [RequireComponent(typeof(KillCamController))]
    public class FovController : MonoBehaviour
    {
        [SerializeField] private FovsData camerasData;
        [SerializeField] private List<GameObject> postProcessingObjs;
        [SerializeField] private InputHandler input;

        private CameraController _cameraController;
        private KillCamController _killCamController;
        
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
        }

        private void OnDisable()
        {
            input?.onChangeFov.RemoveListener(HandleChangeFov);
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
            Debug.Log($"HI? {camerasData.activeCameraPropertyIndex}");
            SetFovs();
        }

        private void SetFovs()
        {
            _cameraController.SetNewFov(camerasData.cameraProperties[camerasData.activeCameraPropertyIndex]);
            _killCamController.SetNewFov(camerasData.cameraProperties[camerasData.activeCameraPropertyIndex]);
        }
    }
}

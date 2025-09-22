using System.Collections;
using Events;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;
using Utils;

namespace CameraScripts
{
    public class KillCamController : MonoBehaviour
    {
        [Header("Properties")] [SerializeField]
        private KillCamProperties killCamProperties;
        
        [Header("Camera objects")] 
        [SerializeField] private CinemachineCamera mainCamera;
        [SerializeField] private CinemachinePositionComposer composer;
        [SerializeField] private CameraPivotController pivotController;
        [SerializeField] private CinemachineConfiner2D confiner;
        
        [Header("Events")] [SerializeField] private VoidEventChannelSO onLastEnemyKilled;
        [SerializeField] private VoidEventChannelSO onWinDoorEnable;

        private Sequence _killCamSequence;
        private Coroutine _killCamCoroutine;
        private CameraProperties _cameraProperties;

        private void OnEnable()
        {
            CreateKillCamSequence();
            onLastEnemyKilled?.onEvent.AddListener(HandleKillCam);
        }

        private void OnDisable()
        {
            onLastEnemyKilled?.onEvent.RemoveListener(HandleKillCam);
        }

        private void CreateKillCamSequence()
        {
            _killCamSequence = new Sequence();
            _killCamSequence.SetAction(KillCamSequence());
            _killCamSequence.AddPostAction(HandleWinDoor());
        }

        private IEnumerator HandleWinDoor()
        {
            onWinDoorEnable?.RaiseEvent();
            yield return null;
        }

        private IEnumerator KillCamSequence()
        {
            pivotController.ShouldPivot = false;
            confiner.enabled = false;
            
            float timeInKillCam = 0f;
            while (timeInKillCam < killCamProperties.killCamDuration)
            {
                float killCamTime = timeInKillCam / killCamProperties.killCamDuration;
                Debug.Log($"KILL CAM TIME {killCamTime} {killCamProperties.killCamDuration}");
                Time.timeScale = ChangeValueByLerp(killCamTime, killCamProperties.deltaTimeAnimation,
                    killCamProperties.minDeltaTime, 1f);

                mainCamera.Lens.FieldOfView = ChangeValueByLerp(killCamTime, killCamProperties.cameraFovAnimation,
                    killCamProperties.maxCameraFov, _cameraProperties.FOV);

                composer.CameraDistance = ChangeValueByLerp(killCamTime, killCamProperties.cameraZoomAnimation,
                    killCamProperties.maxCameraZoom, _cameraProperties.cameraDistance);
                
                timeInKillCam += Time.unscaledDeltaTime;
                yield return null;
            }

            Debug.Log($"Out of kill cam");
            mainCamera.Lens.FieldOfView = _cameraProperties.FOV;
            composer.CameraDistance = _cameraProperties.cameraDistance;
            
            confiner.enabled = true;
            pivotController.ShouldPivot = true;
        }

        private float ChangeValueByLerp(float time, AnimationCurve curve, float minValue, float maxValue)
        {
            float timeToUse = curve.Evaluate(time);

            return Mathf.Lerp(minValue, maxValue, timeToUse);
        }

        private void HandleKillCam()
        {
            if (_killCamCoroutine != null) StopCoroutine(_killCamCoroutine);
            _killCamCoroutine = StartCoroutine(_killCamSequence.Execute());
        }

        public void SetNewFov(CameraProperties cameraProperty)
        {
            _cameraProperties = cameraProperty;
        }

        [ContextMenu("execute kill cam")]
        private void HandleKillcamTest()
        {
            if (_killCamCoroutine != null) StopCoroutine(_killCamCoroutine);
            _killCamCoroutine = StartCoroutine(KillCamSequence());
        }
    }
}
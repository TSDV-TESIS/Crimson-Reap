using System;
using System.Collections;
using UnityEngine;

namespace CameraScripts
{
    public class CameraPivotController : MonoBehaviour
    {
        [SerializeField] private CameraProperties cameraProperties;
        [SerializeField] private GameObject target;

        private Vector2 _dirTowardsTarget;
        private bool _isTracking = false;

        private void Start()
        {
            transform.position = target.transform.position;
        }

        void Update()
        {
            if (GetDistance() >= cameraProperties.pivotMinDistance && !_isTracking)
            {
                _isTracking = true;
            }

            if (_isTracking)
            {
                MoveTowardsTarget();
                if (GetDistance() < Single.Epsilon)
                    _isTracking = false;
            }
        }

        private void MoveTowardsTarget()
        {
            transform.Translate(GetDir() * (cameraProperties.pivotSpeed * Time.deltaTime));
        }

        private float GetDistance()
        {
            return Vector2.Distance(transform.position, target.transform.position);
        }

        private Vector2 GetDir()
        {
            return (target.transform.position - transform.position).normalized;
        }

        private void OnDrawGizmos()
        {
            if (!cameraProperties.drawPivotGizmos)
                return;

            Color prevGizmosColor = Gizmos.color;
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(transform.position, new Vector3(0.5f, 0.5f, 0.5f));
            Gizmos.color = prevGizmosColor;
        }
    }
}
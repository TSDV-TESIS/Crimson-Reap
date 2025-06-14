using System;
using System.Collections;
using Player;
using UnityEngine;

namespace CameraScripts
{
    public class CameraPivotController : MonoBehaviour
    {
        [SerializeField] private CameraProperties cameraProperties;
        [SerializeField] private GameObject target;
        [SerializeField] private InputHandler inputHandler;

        private Vector2 _dirTowardsTarget;
        private bool _isTracking = false;

        private Vector2 _cursorPosition;

        private void Start()
        {
            transform.position = target.transform.position;
            inputHandler.OnPlayerLook.AddListener(HandleLook);
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
            return Vector2.Distance(transform.position, GetTargetPosition());
        }

        private Vector2 GetTargetPosition()
        {
            Vector2 pos = target.transform.position;
            if (cameraProperties.shouldInfluence)
                pos = Vector2.Lerp(pos, _cursorPosition, cameraProperties.cursorInfluence);

            return pos;
        }

        private Vector2 GetDir()
        {
            Vector2 pivotPosition = transform.position;
            return (GetTargetPosition() - pivotPosition).normalized;
        }

        private void HandleLook(Vector2 mousePos)
        {
            float cameraZ = -Camera.main.transform.position.z;
            _cursorPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cameraZ));
            if (cameraProperties.freeCamera)
                return;

            Vector2 lowerLeftLimit = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, cameraZ));
            Vector2 upperRightLimit = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, cameraZ));
            float halfScreenWidth = (upperRightLimit.x - lowerLeftLimit.x) / 2;
            float halfScreenHeight = (upperRightLimit.y - lowerLeftLimit.y) / 2;

            float xPos = Mathf.Clamp(_cursorPosition.x, target.transform.position.x - halfScreenWidth, target.transform.position.x + halfScreenWidth);
            float yPos = Mathf.Clamp(_cursorPosition.y, target.transform.position.y - halfScreenHeight, target.transform.position.y + halfScreenHeight);
            _cursorPosition = new Vector2(xPos, yPos);
        }

        private void OnDrawGizmos()
        {
            if (!cameraProperties.drawPivotGizmos)
                return;

            Color prevGizmosColor = Gizmos.color;
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(transform.position, new Vector3(0.5f, 0.5f, 0.5f));

            Gizmos.color = Color.green;
            Gizmos.DrawCube(_cursorPosition, new Vector3(0.5f, 0.5f, 0.5f));
            Gizmos.color = prevGizmosColor;
        }
    }
}
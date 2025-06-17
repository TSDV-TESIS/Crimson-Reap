using System;
using System.Collections;
using Player;
using Player.Properties;
using UnityEngine;

namespace CameraScripts
{
    public class CameraPivotController : MonoBehaviour
    {
        [SerializeField] private CameraProperties cameraProperties;
        [SerializeField] private GameObject target;
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private PlayerMovementProperties movementProperties;
        
        private Vector2 _dirTowardsTarget;
        private bool _isTracking = false;

        private Vector2 _cursorPosition;
        private Vector2 _cursorOffset;
        private const float DIST_TOLERANCE = 0.1f;

        private Vector2 _prevCursorPosition;

        private void Start()
        {
            transform.position = target.transform.position;
            inputHandler.OnPlayerLook.AddListener(HandleLook);
        }

        void Update()
        {
            if ((GetDistance() >= cameraProperties.pivotMinDistance && !_isTracking) || (cameraProperties.shouldInfluence && _cursorPosition != _prevCursorPosition))
            {
                _isTracking = true;
            }

            if (_isTracking)
            {
                if (GetDistance() > DIST_TOLERANCE)
                    MoveTowardsTarget();
                else
                    _isTracking = false;
            }
        }

        private void MoveTowardsTarget()
        {
            transform.Translate(GetDir() * (GetDistance() >= cameraProperties.pivotMaxDistance ? movementProperties.maxSpeed  * Time.deltaTime : cameraProperties.pivotSpeed * Time.deltaTime));
        }

        private float GetDistance()
        {
            return Vector2.Distance(transform.position, GetTargetPosition());
        }

        private Vector2 GetTargetPosition()
        {
            Vector2 targetPos = target.transform.position;
            if (cameraProperties.shouldInfluence)
            {
                targetPos = Vector2.Lerp(targetPos, targetPos + _cursorOffset, cameraProperties.cursorInfluence);
            }

            return targetPos;
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
            _cursorOffset = _cursorPosition - new Vector2(target.transform.position.x, target.transform.position.y);

            if (cameraProperties.freeCamera)
                return;

            Vector2 lowerLeftLimit = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, cameraZ));
            Vector2 upperRightLimit = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, cameraZ));
            float halfScreenWidth = (upperRightLimit.x - lowerLeftLimit.x) / 2;
            float halfScreenHeight = (upperRightLimit.y - lowerLeftLimit.y) / 2;

            float xPos = Mathf.Clamp(_cursorPosition.x, target.transform.position.x - halfScreenWidth, target.transform.position.x + halfScreenWidth);
            float yPos = Mathf.Clamp(_cursorPosition.y, target.transform.position.y - halfScreenHeight, target.transform.position.y + halfScreenHeight);
            _cursorPosition = new Vector2(xPos, yPos);
            _cursorOffset = _cursorPosition - new Vector2(target.transform.position.x, target.transform.position.y);
        }

        private void OnDrawGizmos()
        {
            if (!cameraProperties.drawPivotGizmos)
                return;

            Color prevGizmosColor = Gizmos.color;
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(transform.position, new Vector3(0.5f, 0.5f, 0.5f));

            Gizmos.color = Color.green;
            Vector2 targetPos = target.transform.position;
            Gizmos.DrawCube(targetPos + _cursorOffset, new Vector3(0.5f, 0.5f, 0.5f));
            Gizmos.color = prevGizmosColor;
        }
    }
}
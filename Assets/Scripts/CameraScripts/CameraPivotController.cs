using System;
using System.Collections;
using Events;
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

        [Header("Events")] [SerializeField] private VoidEventChannelSO onGamepadUsed;
        [SerializeField] private VoidEventChannelSO onKeyboardUsed;

        private Vector2 _dirTowardsTarget;
        private bool _isTracking = false;

        private Vector2 _cursorPosition;
        private Vector2 _cursorOffset;
        private const float DIST_TOLERANCE = 0.1f;

        private Vector2 _prevCursorPosition;

        private bool isCursorBased = false;
        private Action onCursorEnterDeadZone;
        private Coroutine pivotToPlayer;

        private bool _isGamepadUsed;

        private void Start()
        {
            transform.position = target.transform.position;
            onCursorEnterDeadZone += OnCursorDeadZoneEnter;
        }

        private void OnEnable()
        {
            inputHandler.OnPlayerLook.AddListener(HandleLook);
            onGamepadUsed?.onEvent.AddListener(HandleGamepadUsed);
            onKeyboardUsed?.onEvent.AddListener(HandleKeyboardUsed);
        }

        private void OnDisable()
        {
            inputHandler.OnPlayerLook.RemoveListener(HandleLook);
            onGamepadUsed?.onEvent.RemoveListener(HandleGamepadUsed);
            onKeyboardUsed?.onEvent.RemoveListener(HandleKeyboardUsed);
        }

        void Update()
        {
            if (!isCursorBased)
            {
                transform.position = target.transform.position;
                return;
            }

            if ((GetDistance() >= cameraProperties.pivotMinDistance && !_isTracking) ||
                (cameraProperties.shouldInfluence && _cursorPosition != _prevCursorPosition))
                _isTracking = true;

            if (_isTracking)
            {
                if (GetDistance() > DIST_TOLERANCE)
                {
                    MoveTowardsTarget();
                }
                else
                    _isTracking = false;
            }
        }

        private void HandleKeyboardUsed()
        {
            _isGamepadUsed = false;
        }

        private void HandleGamepadUsed()
        {
            _isGamepadUsed = true;
        }

        private void MoveTowardsTarget()
        {
            transform.Translate(GetDir() * (GetDistance() >= cameraProperties.pivotMaxDistance
                ? movementProperties.maxSpeed * Time.deltaTime
                : cameraProperties.pivotSpeed * Time.deltaTime));
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

        private void OnCursorDeadZoneEnter()
        {
            if (pivotToPlayer != null)
                StopCoroutine(pivotToPlayer);

            pivotToPlayer = StartCoroutine(PivotToPlayer());
        }

        private IEnumerator PivotToPlayer()
        {
            float duration = Vector3.Distance(transform.position, target.transform.position) /
                             cameraProperties.pivotSpeed;
            float timer = 0;
            float startTime = Time.time;
            while (timer < duration)
            {
                timer = Time.time - startTime;
                transform.Translate((target.transform.position - transform.position).normalized *
                                    (cameraProperties.pivotSpeed * Time.deltaTime));
                yield return null;
            }

            isCursorBased = false;
        }

        private Vector2 GetDir()
        {
            Vector2 pivotPosition = transform.position;
            return (GetTargetPosition() - pivotPosition).normalized;
        }

        private void HandleLook(Vector2 mousePos)
        {
            if (_isGamepadUsed)
                HandleLookWithGamepad(mousePos);
            else
                HandleLookWithCursor(mousePos);
        }

        private void HandleLookWithGamepad(Vector2 joystickPos)
        {
            float cameraZ = -Camera.main.transform.position.z;

            Vector2 lowerLeftLimit = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, cameraZ));
            Vector2 upperRightLimit = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, cameraZ));

            float posX = joystickPos.x / 2f + 0.5f;
            float posY = joystickPos.y / 2f + 0.5f;

            _cursorPosition = new Vector2(Mathf.Lerp(lowerLeftLimit.x, upperRightLimit.x, posX),
                Mathf.Lerp(lowerLeftLimit.y, upperRightLimit.y, posY));

            _cursorOffset = _cursorPosition - new Vector2(target.transform.position.x, target.transform.position.y);

            if (cameraProperties.freeCamera) return;
            
            Vector2 trackIntensity = GetTrackingForce(new Vector2(posX, posY), cameraProperties.cursorDeadZone);

            if (trackIntensity != Vector2.zero)
            {
                isCursorBased = true;
                return;
            }

            if (isCursorBased)
                onCursorEnterDeadZone?.Invoke();
            _cursorPosition = target.transform.position;
            _cursorOffset = new Vector2(0f, 0f);
        }

        private void HandleLookWithCursor(Vector2 mousePos)
        {
            float cameraZ = -Camera.main.transform.position.z;
            _cursorPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cameraZ));
            Vector2 cursorViewPortPos = Camera.main.ScreenToViewportPoint(new Vector3(mousePos.x, mousePos.y, cameraZ));
            _cursorOffset = _cursorPosition - new Vector2(target.transform.position.x, target.transform.position.y);

            if (cameraProperties.freeCamera)
                return;

            Vector2 trackIntensity = GetTrackingForce(cursorViewPortPos, cameraProperties.cursorDeadZone);

            if (trackIntensity != Vector2.zero)
                isCursorBased = true;
            else if (isCursorBased)
                onCursorEnterDeadZone?.Invoke();

            Vector2 lowerLeftLimit = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, cameraZ));
            Vector2 upperRightLimit = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, cameraZ));
            float halfScreenWidth = (upperRightLimit.x - lowerLeftLimit.x) / 2;
            float halfScreenHeight = (upperRightLimit.y - lowerLeftLimit.y) / 2;

            float xPos = Mathf.Clamp(_cursorPosition.x, target.transform.position.x - halfScreenWidth,
                target.transform.position.x + halfScreenWidth);
            float yPos = Mathf.Clamp(_cursorPosition.y, target.transform.position.y - halfScreenHeight,
                target.transform.position.y + halfScreenHeight);
            if (trackIntensity.x == 0)
                xPos = target.transform.position.x;
            if (trackIntensity.y == 0)
                yPos = target.transform.position.y;

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

            Gizmos.color = Color.red;
            Vector3 cursorDeadZone = cameraProperties.cursorDeadZone;
            Vector3 rectCornerUpperRight = Camera.main.ViewportToWorldPoint(new Vector3(cursorDeadZone.x,
                cursorDeadZone.y, -Camera.main.transform.position.z));
            Vector3 rectCornerLowerLeft = Camera.main.ViewportToWorldPoint(new Vector3(1 - cursorDeadZone.x,
                1 - cursorDeadZone.y, -Camera.main.transform.position.z));
            Vector3 rectCornerUpperLeft = new Vector3(rectCornerLowerLeft.x, rectCornerUpperRight.y);
            Vector3 rectCornerLowerRight = new Vector3(rectCornerUpperRight.x, rectCornerLowerLeft.y);

            Gizmos.DrawLine(rectCornerLowerLeft, rectCornerLowerRight);
            Gizmos.DrawLine(rectCornerUpperLeft, rectCornerUpperRight);
            Gizmos.DrawLine(rectCornerLowerLeft, rectCornerUpperLeft);
            Gizmos.DrawLine(rectCornerLowerRight, rectCornerUpperRight);
        }
        
        private Vector2 GetTrackingForce(Vector2 cursorViewPortPos, Vector2 deadZone)
        {
            Vector2 trackIntensity = Vector2.zero;

            if (cursorViewPortPos.x >= deadZone.x)
                trackIntensity.x = 1;
            else if (cursorViewPortPos.x <= 1 - deadZone.x)
                trackIntensity.x = -1;

            if (cursorViewPortPos.y >= deadZone.y)
                trackIntensity.y = 1;
            else if (cursorViewPortPos.y <= 1 - deadZone.y)
                trackIntensity.y = -1;

            return trackIntensity;
        }
    }
}
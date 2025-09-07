using System;
using Events.Scriptables;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Player
{
    public class MouseLook : MonoBehaviour
    {
        [SerializeField] private InputHandler handler;
        [SerializeField] private GameObject visorPivot;
        [SerializeField] private PlayerLookProperties lookProperties;
        [SerializeField] private MouseDataFromPlayer mouseDataFromPlayer;
        
        [SerializeField] private FloatEventChannel onNewAngle;
        
        private float _angle;

        private Vector2 _viewPortPos;
        private Vector2 _cursorDir;
        public Vector2 CursorDir => _cursorDir.normalized;

        private void Update()
        {
            Vector2 cursorPos = Mouse.current.position.ReadValue();
            _viewPortPos = Camera.main.ScreenToViewportPoint(cursorPos);
            Vector2 playerPosOnViewport = Camera.main.WorldToViewportPoint(transform.position);

            _cursorDir = _viewPortPos - new Vector2(playerPosOnViewport.x, playerPosOnViewport.y);
            _cursorDir.Normalize();
            mouseDataFromPlayer.mouseDirection = _cursorDir;

            _angle = Mathf.Atan2(_cursorDir.x, _cursorDir.y) * Mathf.Rad2Deg;
            visorPivot.transform.rotation = Quaternion.AngleAxis(-_angle + 90, Vector3.forward) * transform.rotation;
            onNewAngle?.RaiseEvent(_angle);
        }

        private void OnDrawGizmos()
        {
            if (lookProperties.showDeadzone)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, lookProperties.deadZoneRadius);
            }
        }
    }
}
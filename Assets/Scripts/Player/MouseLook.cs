using System;
using Events;
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
        
        [Header("Events")]
        [SerializeField] private VoidEventChannelSO onGamepadControlUsed;
        [SerializeField] private VoidEventChannelSO onKeyboardControlUsed;
        
        private float _angle;

        private Vector2 _viewPortPos;
        private Vector2 _dir;
        public Vector2 CursorDir => _dir.normalized;

        private bool _isGamepad;

        private void OnEnable()
        {
            onGamepadControlUsed?.onEvent.AddListener(HandleUseGamepad);
            onKeyboardControlUsed?.onEvent.AddListener(HandleUseKeyboard);
            handler?.OnPlayerMove.AddListener(HandleSetLook);
        }

        private void OnDisable()
        {
            onGamepadControlUsed?.onEvent.RemoveListener(HandleUseGamepad);
            onKeyboardControlUsed?.onEvent.RemoveListener(HandleUseKeyboard);
            handler?.OnPlayerMove.RemoveListener(HandleSetLook);
        }
        
        private void HandleSetLook(Vector2 lookDirection)
        {
            if (Mathf.Approximately(lookDirection.magnitude, 0f)) return;
            _dir = lookDirection.normalized;
        }
        
        private void Update()
        {
            if (!_isGamepad)
            {
                Vector2 cursorPos = Mouse.current.position.ReadValue();
                _viewPortPos = Camera.main.ScreenToViewportPoint(cursorPos);
                Vector2 playerPosOnViewport = Camera.main.WorldToViewportPoint(transform.position);

                _dir = _viewPortPos - new Vector2(playerPosOnViewport.x, playerPosOnViewport.y);
                _dir.Normalize();
                mouseDataFromPlayer.mouseDirection = _dir;
            }
            
            _angle = Mathf.Atan2(_dir.x, _dir.y) * Mathf.Rad2Deg;
            visorPivot.transform.rotation = Quaternion.AngleAxis(-_angle + 90, Vector3.forward) * transform.rotation;
        }
        
        private void HandleUseKeyboard()
        {
            _isGamepad = false;
        }

        private void HandleUseGamepad()
        {
            _isGamepad = true;
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
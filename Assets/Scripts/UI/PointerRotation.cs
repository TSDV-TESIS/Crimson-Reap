using System;
using Events;
using Events.Scriptables;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class PointerRotation : MonoBehaviour
    {
        [SerializeField] private MouseDataFromPlayer mouseDataFromPlayer;
        [SerializeField] private Transform rotationPoint;
        [SerializeField] private InputHandler handler;
        
        [Header("Configuration")]
        [SerializeField] private float radius;
        [SerializeField] private float moveLerpVelocity;
        [SerializeField] private float rotationLerpVelocity;
        
        [Header("Events")] 
        [SerializeField] private VoidEventChannelSO onGamepadControlUsed;
        [SerializeField] private VoidEventChannelSO onKeyboardControlUsed;

        private bool _isGamepad;
        private Vector2 _lookDir;
        
        private void OnEnable()
        {
            _isGamepad = false;
            
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
            _lookDir = lookDirection.normalized;
        }

        private void HandleUseKeyboard()
        {
            Debug.Log("KEYBOARD");
            _isGamepad = false;
        }

        private void HandleUseGamepad()
        {
            Debug.Log("GAMEPAD");
            _isGamepad = true;
        }

        private void Update()
        {
            if (_isGamepad)
                HandleRotate(_lookDir);
            else
                HandleRotate(mouseDataFromPlayer.mouseDirection);
        }

        private void HandleRotate(Vector2 direction)
        {
            Vector3 desiredPosition = rotationPoint.position + (Vector3)(direction * radius);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * moveLerpVelocity);
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion desiredRot = Quaternion.AngleAxis(angle + 270f, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, Time.deltaTime * rotationLerpVelocity);
        }
    }
}

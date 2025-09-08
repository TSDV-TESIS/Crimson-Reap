using System;
using Events;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class CursorManager : MonoBehaviour
    {
        [SerializeField] private Texture2D cursorTexture;
        [SerializeField] private Texture2D clickTexture;
        [SerializeField] private Vector2 hotspot;
        [SerializeField] private CursorMode mode;

        [Header("Events")] 
        [SerializeField] private VoidEventChannelSO onGamepadUsed;
        [SerializeField] private VoidEventChannelSO onKeyboardUsed;
        
        private void OnEnable()
        {
            Cursor.SetCursor(cursorTexture, hotspot, mode);
            
            onGamepadUsed?.onEvent.AddListener(HandleGamepad);
            onKeyboardUsed?.onEvent.AddListener(HandleKeyboard);
        }

        private void OnDisable()
        {
            onGamepadUsed?.onEvent.RemoveListener(HandleGamepad);
            onKeyboardUsed?.onEvent.RemoveListener(HandleKeyboard);
        }

        void Update()
        {
            if(Mouse.current.leftButton.wasPressedThisFrame)
                Cursor.SetCursor(clickTexture, hotspot, mode);
            else if(Mouse.current.leftButton.wasReleasedThisFrame)
                Cursor.SetCursor(cursorTexture, hotspot, mode);
        }
        
        private void HandleGamepad()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void HandleKeyboard()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
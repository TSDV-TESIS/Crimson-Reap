using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace TestRoom
{
    [CreateAssetMenu(fileName = "TestRoomInputHandler", menuName = "Scriptable Objects/TestRoom InputHandler")]
    public class TestRoomInputHandler : ScriptableObject
    {
        public UnityEvent onTeleportToTutorial;
        public UnityEvent onTeleportToMovement;
        public UnityEvent onTeleportToEnemy;
        public UnityEvent onTeleportToDoor;
        
        public void HandleTeleportToTutorial(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                onTeleportToTutorial?.Invoke();
            }
        }
        
        public void HandleTeleportToMovement(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                onTeleportToMovement?.Invoke();
            }
        }

        public void HandleTeleportToEnemy(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                onTeleportToEnemy?.Invoke();
            }
        }
        
        public void HandleTeleportToDoor(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                onTeleportToDoor?.Invoke();
            }
        }
    }
}

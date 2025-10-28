using Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class GamepadManager : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO onGamepadControlUsed;
        [SerializeField] private VoidEventChannelSO onKeyboardControlUsed;

        public void HandleControlsChanged(PlayerInput input)
        {
            if (input == null || input.currentControlScheme == null)
                return;

            if (input.currentControlScheme.Equals("Gamepad"))
            {
                onGamepadControlUsed?.RaiseEvent();
            }
            else
            {
                onKeyboardControlUsed?.RaiseEvent();
            }
        }
    }
}
using Events;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO onPaused;
    [SerializeField] private VoidEventChannelSO onUnPaused;
    [SerializeField] private PlayerInput pausedInput;

    private PlayerInput _input;

    private void OnEnable()
    {
        _input ??= GetComponent<PlayerInput>();
        onPaused.onEvent.AddListener(HandlePause);
        onUnPaused.onEvent.AddListener(HandleUnPause);
    }

    private void OnDisable()
    {
        onPaused.onEvent.RemoveListener(HandlePause);
        onUnPaused.onEvent.RemoveListener(HandleUnPause);
    }

    private void HandlePause()
    {
        _input.enabled = false;
        pausedInput.enabled = true;
    }

    private void HandleUnPause()
    {
        _input.enabled = true;
        pausedInput.enabled = false;
    }
}
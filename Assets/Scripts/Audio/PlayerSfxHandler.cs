using System;
using Events;
using Events.ScriptableObjects;
using Events.Scriptables;
using UnityEngine;

public class PlayerSfxHandler : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event stepsSFX;
    [SerializeField] private AK.Wwise.Switch woodStepsSwitch;
    [SerializeField] private AK.Wwise.Switch stoneStepsSwitch;
    [SerializeField] private AK.Wwise.Event dashSFX;
    [SerializeField] private AK.Wwise.Event attackSFX;
    [SerializeField] private AK.Wwise.Event jumpSFX;
    [SerializeField] private AK.Wwise.RTPC healthRTPC;

    [SerializeField] private AkWwiseEventChannelSO playEvent;
    [SerializeField] private AkWwiseEventChannelSO stopEvent;

    [Header("Level Event Channels")]
    [SerializeField] private VoidEventChannelSO onDash;
    [SerializeField] private VoidEventChannelSO onJump;
    [SerializeField] private VoidEventChannelSO onAttack;
    [SerializeField] private StepsMaterialEventChannelSO onWalk;
    [SerializeField] private VoidEventChannelSO onStopWalking;

    private bool isWalking = false;

    private void Start()
    {
        onDash?.onEvent.AddListener(HandleDashSFX);
        onJump?.onEvent.AddListener(HandleJumpSFX);
        onAttack?.onEvent.AddListener(HandleAttackSFX);
        onWalk?.onTypedEvent.AddListener(HandleWalkSFX);
        onStopWalking?.onEvent.AddListener(HandleStopWalk);
    }

    private void HandleDashSFX()
    {
        playEvent?.RaiseEvent(attackSFX);
    }

    private void HandleJumpSFX()
    {
        playEvent?.RaiseEvent(jumpSFX);
    }

    private void HandleAttackSFX()
    {
        playEvent?.RaiseEvent(attackSFX);
    }

    private void HandleWalkSFX(FloorMaterials floorMaterial)
    {
        switch (floorMaterial)
        {
            case FloorMaterials.Wood:
                Debug.Log("WoodenFloor");
                break;
            case FloorMaterials.Stone:
                Debug.Log("StoneFloor");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(floorMaterial), floorMaterial, null);
        }

        if (!isWalking)
        {
            isWalking = true;
            playEvent?.RaiseEvent(stepsSFX);
        }
    }

    private void HandleStopWalk()
    {
        isWalking = false;
        stopEvent?.RaiseEvent(stepsSFX);
    }
}
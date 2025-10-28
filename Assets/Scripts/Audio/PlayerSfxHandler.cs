using System;
using System.Collections;
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

    [Header("WwiseEvents")]
    [SerializeField] private AkWwiseEventChannelSO playEvent;
    [SerializeField] private AkWwiseEventChannelSO stopEvent;
    [SerializeField] private AkWwiseSwitchEventChannelSO switchEvent;
    [SerializeField] private AkWwiseRTPCEventChannelSO rtpcEvent;

    [Header("PlayerEvents")]
    [SerializeField] private VoidEventChannelSO onDash;
    [SerializeField] private VoidEventChannelSO onJump;
    [SerializeField] private VoidEventChannelSO onAttack;
    [SerializeField] private StepsMaterialEventChannelSO onWalk;
    [SerializeField] private VoidEventChannelSO onStopWalking;
    [SerializeField] private IntEventChannelSO onTakeDamage;
    [SerializeField] private IntEventChannelSO onHeal;
    [Header("VFX Properties")]
    [SerializeField] private float stepsDelay;

    private bool _isWalking = false;

    private Coroutine _stepCoolDown;
    private bool _shouldStep = true;

    private FloorMaterials _currentFloorMaterial = FloorMaterials.NONE;

    private void Start()
    {
        onDash?.onEvent.AddListener(HandleDashSFX);
        onJump?.onEvent.AddListener(HandleJumpSFX);
        onAttack?.onEvent.AddListener(HandleAttackSFX);
        onWalk?.onTypedEvent.AddListener(HandleWalkSFX);
        onStopWalking?.onEvent.AddListener(HandleStopWalk);
        onTakeDamage?.onIntEvent.AddListener(HandleLife);
        onHeal?.onIntEvent.AddListener(HandleLife);
    }

    [ContextMenu("DASH")]
    private void HandleDashSFX()
    {
        playEvent?.RaiseEvent(dashSFX);
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
        if (!_shouldStep)
            return;

        if (floorMaterial != _currentFloorMaterial)
        {
            _currentFloorMaterial = floorMaterial;
            switch (floorMaterial)
            {
                case FloorMaterials.Wood:
                    switchEvent?.RaiseEvent(woodStepsSwitch);
                    break;
                case FloorMaterials.Stone:
                    switchEvent?.RaiseEvent(stoneStepsSwitch);
                    break;
            }
        }

        playEvent?.RaiseEvent(stepsSFX);
        if (_stepCoolDown != null)
            StopCoroutine(_stepCoolDown);
        _stepCoolDown = StartCoroutine(StepCoolDown());
    }

    public void HandleStopWalk()
    {
        _isWalking = false;
        stopEvent?.RaiseEvent(stepsSFX);
    }

    private void HandleLife(int currentHealth)
    {
        rtpcEvent?.RaiseEvent((healthRTPC, currentHealth));
    }

    private IEnumerator StepCoolDown()
    {
        _shouldStep = false;
        yield return new WaitForSeconds(stepsDelay);
        _shouldStep = true;
    }
}
using System;
using Events;
using Events.Scriptables;
using Health;
using Managers;
using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event musicEvent;
    [SerializeField] private AK.Wwise.Event pauseEvent;
    [SerializeField] private AK.Wwise.Event winEvent;
    [SerializeField] private AK.Wwise.Event deathEvent;

    [SerializeField] private AkWwiseEventChannelSO playChannel;
    [SerializeField] private AkWwiseEventChannelSO stopChannel;

    [Header("LevelStates")] [SerializeField] private VoidEventChannelSO onGamePaused;
    [SerializeField] private VoidEventChannelSO onGameUnPaused;
    [SerializeField] private VoidEventChannelSO onPlayerWin;
    [SerializeField] private DeathEventChannelSO onPlayerDeath;

    private void OnEnable()
    {
        onGamePaused.onEvent.AddListener(TriggerPauseMusic);
        onGameUnPaused.onEvent.AddListener(TriggerMusic);
        onPlayerWin.onEvent.AddListener(TriggerWin);
        onPlayerDeath.onTypedEvent.AddListener(TriggerDeathMusic);
    }

    void Start()
    {
        TriggerMusic();
    }

    private void OnDestroy()
    {
        onGamePaused.onEvent.RemoveListener(TriggerPauseMusic);
        onGameUnPaused.onEvent.RemoveListener(TriggerMusic);
        onPlayerWin.onEvent.RemoveListener(TriggerWin);
        onPlayerDeath.onTypedEvent.RemoveListener(TriggerDeathMusic);
    }

    private void TriggerPauseMusic()
    {
        playChannel.RaiseEvent(pauseEvent);
    }

    private void TriggerMusic()
    {
        stopChannel.RaiseEvent(musicEvent);
        playChannel.RaiseEvent(musicEvent);
    }

    private void TriggerWin()
    {
        playChannel.RaiseEvent(winEvent);
    }

    private void TriggerDeathMusic(DeathCauses cause)
    {
        playChannel.RaiseEvent(deathEvent);
    }
}
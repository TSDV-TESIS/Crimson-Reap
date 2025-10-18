using System;
using Events;
using Events.Scriptables;
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

    [Header("LevelStates")]
    [SerializeField] private VoidEventChannelSO onGamePaused;
    [SerializeField] private VoidEventChannelSO onGameUnPaused;
    [SerializeField] private VoidEventChannelSO onPlayerWin;
    [SerializeField] private VoidEventChannelSO onPlayerDeath;

    private void OnEnable()
    {
        onGamePaused.onEvent.AddListener(TriggerPauseMusic);
        onGameUnPaused.onEvent.AddListener(TriggerMusic);
        onPlayerWin.onEvent.AddListener(TriggerWin);
        onPlayerDeath.onEvent.AddListener(TriggerDeathMusic);
    }

    void Start()
    {
        TriggerMusic();
    }

    private void TriggerPauseMusic()
    {
        playChannel.RaiseEvent(pauseEvent);
    }
    private void TriggerMusic()
    {
        playChannel.RaiseEvent(musicEvent);
    }

    private void TriggerWin()
    {
        playChannel.RaiseEvent(winEvent);
    }
    private void TriggerDeathMusic()
    {
        playChannel.RaiseEvent(deathEvent);
    }
}
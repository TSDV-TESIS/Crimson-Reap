using System;
using Events;
using Events.Scriptables;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO onPlayerWin;
    [SerializeField] private VoidEventChannelSO onPlayerDeath;
    [SerializeField] private FloatEventChannel onTimerFinish;
    [SerializeField] private FloatEventChannel onTimerTick;

    [SerializeField] private GlobalTime globalTimeSO;
    [SerializeField] private bool isFirstLevel = false;

    private float time = 0;
    private bool shouldCountTime;

    private void OnEnable()
    {
        onPlayerWin?.onEvent.AddListener(OnWinEndTimer);
        onPlayerDeath?.onEvent.AddListener(OnLoseEndTimer);
        StartTimer();
        if (isFirstLevel)
            ResetTotalTime();
    }

    private void OnDisable()
    {
        onPlayerWin?.onEvent.RemoveListener(OnWinEndTimer);
        onPlayerDeath?.onEvent.RemoveListener(OnLoseEndTimer);
    }

    private void StartTimer()
    {
        shouldCountTime = true;
        time = 0;
    }

    private void OnWinEndTimer()
    {
        shouldCountTime = false;
        onTimerFinish?.RaiseEvent(time);
        globalTimeSO.time += time;
    }

    private void OnLoseEndTimer()
    {
        shouldCountTime = false;
        globalTimeSO.time += time;
    }

    private void Update()
    {
        if (!shouldCountTime)
            return;

        time += Time.deltaTime;
        onTimerTick.RaiseEvent(time);
    }

    private void ResetTotalTime()
    {
        globalTimeSO.time = 0;
    }
}
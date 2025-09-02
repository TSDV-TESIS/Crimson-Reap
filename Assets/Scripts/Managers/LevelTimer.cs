using Events;
using Events.Scriptables;
using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO onLevelStart;
    [SerializeField] private VoidEventChannelSO onPlayerWin;
    [SerializeField] private FloatEventChannel onTimerFinish;
    [SerializeField] private FloatEventChannel onTimerTick;

    private float time = 0;
    private bool shouldCountTime;

    private void OnEnable()
    {
        onPlayerWin?.onEvent.AddListener(EndTimer);
        //onLevelStart?.onEvent.AddListener(StartTimer);
        StartTimer();
    }

    private void StartTimer()
    {
        shouldCountTime = true;
        time = 0;
    }

    private void EndTimer()
    {
        shouldCountTime = false;
        onTimerFinish?.RaiseEvent(time);
    }

    private void Update()
    {
        if (!shouldCountTime)
            return;

        time += Time.deltaTime;
        onTimerTick.RaiseEvent(time);
    }
}
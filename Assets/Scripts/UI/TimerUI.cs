using System;
using Events.Scriptables;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private FloatEventChannel onTimerTick;
    [SerializeField] private GlobalTime globalTimer;

    [SerializeField] private string suffix = "";

    private void OnEnable()
    {
        onTimerTick?.onFloatEvent.AddListener(UpdateTimer);
        if (globalTimer != null)
            UpdateTimer(globalTimer.time);
    }

    private void OnDisable()
    {
        onTimerTick?.onFloatEvent.RemoveListener(UpdateTimer);
    }

    private void UpdateTimer(float time)
    {
        timerText.text = suffix + TimeFormatting.GetFormattedTime(time);
    }
}
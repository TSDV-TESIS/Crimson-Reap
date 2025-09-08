using System;
using Events.Scriptables;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private FloatEventChannel onTimerTick;

    private void OnEnable()
    {
        onTimerTick?.onFloatEvent.AddListener(UpdateTimer);
    }

    private void OnDisable()
    {
        onTimerTick?.onFloatEvent.RemoveListener(UpdateTimer);
    }

    private void UpdateTimer(float time)
    {
        timerText.text = TimeFormatting.GetFormattedTime(time);
    }
}
using System;
using System.Collections;
using Events;
using Events.Scriptables;
using TMPro;
using UnityEngine;

public class WinPanel : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO onWinPanel;
    [SerializeField] private VoidEventChannelSO onChangeLevel;
    [SerializeField] private FloatEventChannel onTimerFinish;

    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI timeTMPro;
    [SerializeField] private TextMeshProUGUI timePersonalBestTMPro;
    [SerializeField] private TextMeshProUGUI NextLevelCountDown;
    [SerializeField] private float countDownDuration = 3;

    private string timePersonalBest = "timePersonalBest";

    private string timeSuffix = "Level Cleared in: ";
    private string recordSuffix = "Best Time: ";
    private string recordText = "New Personal Best!";

    private Coroutine countDown;

    private void OnEnable()
    {
        panel.SetActive(false);
        onWinPanel.onEvent.AddListener(OnWin);
        onTimerFinish?.onFloatEvent.AddListener(HandleTimerFinish);
    }

    private void OnDisable()
    {
        onWinPanel.onEvent.RemoveListener(OnWin);
        onTimerFinish?.onFloatEvent.RemoveListener(HandleTimerFinish);
    }

    private void OnWin()
    {
        panel.SetActive(true);
        Time.timeScale = 0;
        if (countDown != null)
            StopCoroutine(countDown);

        countDown = StartCoroutine(NextLevelCoroutine());
    }

    private void HandleTimerFinish(float time)
    {
        bool shouldDisplayPersonalBest = false;
        if (!PlayerPrefs.HasKey(timePersonalBest) || PlayerPrefs.GetFloat(timePersonalBest) > time)
        {
            PlayerPrefs.SetFloat(timePersonalBest, time);
            shouldDisplayPersonalBest = true;
        }

        timeTMPro.text = timeSuffix + TimeFormatting.GetFormattedTime(time);

        // if (shouldDisplayPersonalBest)
        //     timePersonalBestTMPro.text = recordText;
        // else
        //     timePersonalBestTMPro.text = recordSuffix + TimeFormatting.GetFormattedTime(PlayerPrefs.GetFloat(timePersonalBest));
    }

    private IEnumerator NextLevelCoroutine()
    {
        float startTime = Time.unscaledTime;
        float timer = 0;
        while (timer < countDownDuration)
        {
            timer = Time.unscaledTime - startTime;
            int countDownNumber = Mathf.CeilToInt(Mathf.Clamp((int)countDownDuration - Mathf.CeilToInt(timer), 0f, countDownDuration));
            NextLevelCountDown.text = countDownNumber.ToString();
            yield return null;
        }

        NextLevel();
    }

    public void NextLevel()
    {
        Debug.Log("NEXT LEVEL PRESSED");
        onChangeLevel?.RaiseEvent();
        Time.timeScale = 1;
    }
}
using System.Collections;
using Events;
using Events.Scriptables;
using TMPro;
using UnityEngine;

public class WinPanel : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO onChangeLevel;
    [SerializeField] private VoidEventChannelSO onLevelReset;
    [SerializeField] private FloatEventChannel onTimerFinish;

    [SerializeField] private GameObject panel;
    [SerializeField] private TieredTimes medals;
    [SerializeField] private TextMeshProUGUI timeTMPro;
    [SerializeField] private TextMeshProUGUI timePersonalBestTMPro;
    [SerializeField] private TextMeshProUGUI NextLevelCountDown;
    [SerializeField] private float countDownDuration = 3;

    private string timePersonalBest = "timePersonalBest";

    private string timeSuffix = "Level Cleared in: ";
    private string recordSuffix = "Best Time: ";
    private string recordText = "New Personal Best!";

    private Coroutine countDown;

    private float levelClearTime;

    private void OnEnable()
    {
        panel.SetActive(false);
        onTimerFinish?.onFloatEvent.AddListener(HandleTimerFinish);
    }

    private void OnDisable()
    {
        onTimerFinish?.onFloatEvent.RemoveListener(HandleTimerFinish);
    }

    private void HandleTimerFinish(float time)
    {
        panel.SetActive(true);
        Time.timeScale = 0;
        if (countDown != null)
            StopCoroutine(countDown);

        countDown = StartCoroutine(NextLevelCoroutine(time));

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

    private IEnumerator NextLevelCoroutine(float levelClearTime)
    {
        yield return medals.UnlockMedalsCoroutine(levelClearTime);
    }

    public void NextLevel()
    {
        Debug.Log("NEXT LEVEL PRESSED");
        onChangeLevel?.RaiseEvent();
        Time.timeScale = 1;
    }

    public void ResetLevel()
    {
        Debug.Log("Restart LEVEL PRESSED");
        onLevelReset?.RaiseEvent();
        Time.timeScale = 1;
    }
}
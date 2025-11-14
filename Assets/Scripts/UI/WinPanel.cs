using System.Collections;
using Events;
using Events.Scriptables;
using TMPro;
using UI.Leaderboard;
using UnityEngine;
using UnityEngine.Serialization;

public class WinPanel : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO onChangeLevel;
    [SerializeField] private VoidEventChannelSO onLevelReset;
    [SerializeField] private FloatEventChannel onTimerFinish;
    [SerializeField] private VoidEventChannelSO onGamePaused;

    [SerializeField] private GameObject panel;
    [SerializeField] private LevelCompleteAnimation levelCompletePhase;
    [SerializeField] private TieredTimes medals;
    [SerializeField] private TextMeshProUGUI timeTMPro;
    [SerializeField] private TextMeshProUGUI timePersonalBestTMPro;
    [SerializeField] private LeaderboardRequestHandler leaderboardRequestHandler;
    [SerializeField] private float countDownDuration = 3;

    [SerializeField] private string levelPBKey;

    private string timeSuffix = "Level Cleared in: ";
    private string recordSuffix = "Best Time: ";
    private string recordText = "New Personal Best!";

    private Coroutine countDown;
    private Coroutine completePhase;

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
        if (completePhase != null)
            StopCoroutine(completePhase);

        completePhase = StartCoroutine(levelCompletePhase.LevelCompleteStart(time));

        leaderboardRequestHandler.HandleSetTime((int)(time * 1000));
        TimeManager.Instance.PauseTime(true);
        onGamePaused?.RaiseEvent();
        if (countDown != null)
            StopCoroutine(countDown);

        countDown = StartCoroutine(NextLevelCoroutine(time));

        if (!PlayerPrefs.HasKey(levelPBKey) || PlayerPrefs.GetFloat(levelPBKey) > time)
            PlayerPrefs.SetFloat(levelPBKey, time);

        timeTMPro.text = timeSuffix + TimeFormatting.GetFormattedTime(time);
        timePersonalBestTMPro.text = TimeFormatting.GetFormattedTime(PlayerPrefs.GetFloat(levelPBKey));
    }

    private IEnumerator NextLevelCoroutine(float levelClearTime)
    {
        yield return medals.UnlockMedalsCoroutine(levelClearTime);
    }

    public void NextLevel()
    {
        Debug.Log("NEXT LEVEL PRESSED");
        onChangeLevel?.RaiseEvent();
        TimeManager.Instance.PauseTime(false);
    }

    public void ResetLevel()
    {
        Debug.Log("Restart LEVEL PRESSED");
        onLevelReset?.RaiseEvent();
        TimeManager.Instance.PauseTime(false);
    }
}
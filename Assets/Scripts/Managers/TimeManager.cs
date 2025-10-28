using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    public bool isPaused;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void TrySetTimeScale(float value)
    {
        if (isPaused)
            return;

        Time.timeScale = value;
    }

    public void PauseTime(bool isPaused)
    {
        this.isPaused = isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }
}
using System;
using Events;
using Events.Scriptables;
using Player;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private InputHandler input;

    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject settingsScreen;

    [Header("Main Menu")]
    [SerializeField] private string mainMenuScene;
    [SerializeField] private StringEventChannelSO onMainMenu;

    private bool isPaused = false;

    private void OnEnable()
    {
        input.onPauseToggle.AddListener(TogglePause);
        Time.timeScale = 1;
    }

    private void OnDisable()
    {
        input.onPauseToggle.RemoveListener(TogglePause);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        panel.SetActive(isPaused);
        if (isPaused)
        {
            pauseScreen.SetActive(true);
            settingsScreen.SetActive(false);
        }

        Time.timeScale = isPaused ? 0 : 1;
    }

    public void ToggleSettingsScreen()
    {
        pauseScreen.SetActive(!pauseScreen.activeSelf);
        settingsScreen.SetActive(!settingsScreen.activeSelf);
    }

    public void GoToMainMenu()
    {
        onMainMenu?.RaiseEvent(mainMenuScene);
    }
}
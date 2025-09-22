using System;
using Events;
using Player;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private InputHandler input;

    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject settingsScreen;

    private bool isPaused = false;

    private void OnEnable()
    {
        input.onPauseToggle.AddListener(TogglePause);
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
}
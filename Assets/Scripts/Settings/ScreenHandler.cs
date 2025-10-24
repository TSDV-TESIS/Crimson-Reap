using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ScreenHandler : MonoBehaviour
{
    [SerializeField] private List<Vector2Int> resolutions;
    [SerializeField] private Vector2Int defaultResolution;

    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private Vector2Int currentRes;

    void Start()
    {
        currentRes = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);
        int resValue = -1;
        for (int i = 0; i < resolutions.Count; i++)
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData());
            resolutionDropdown.options[i].text = $"{resolutions[i].x}x{resolutions[i].y}";
            if (resolutions[i] == currentRes)
                resValue = i;
        }

        if (resolutionDropdown.value == -1)
        {
            for (int i = 0; i < resolutions.Count; i++)
            {
                if (resolutions[i] == defaultResolution)
                    resValue = i;
            }
        }

        resolutionDropdown.value = resValue;
    }

    public void SetResolution()
    {
        if (resolutionDropdown.value > resolutions.Count)
            throw new ArgumentOutOfRangeException();

        if (resolutions[resolutionDropdown.value] == currentRes)
            return;

        Debug.Log($"New Resolution {resolutions[resolutionDropdown.value].ToString()}");
        currentRes = resolutions[resolutionDropdown.value];
        Screen.SetResolution(resolutions[resolutionDropdown.value].x, resolutions[resolutionDropdown.value].y, FullScreenMode.ExclusiveFullScreen);
    }
}
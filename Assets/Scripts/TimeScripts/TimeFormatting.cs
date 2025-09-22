using System;
using UnityEngine;

public class TimeFormatting
{
    public static string GetFormattedTime(float timeInSeconds)
    {
        return TimeSpan.FromSeconds(timeInSeconds).ToString(@"mm\:ss\:fff");
    }

    public static int ToMs(float time)
    {
        return Mathf.FloorToInt(time * 1000);
    }
}
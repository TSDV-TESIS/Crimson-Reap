using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TieredTimes : MonoBehaviour
{
    [SerializeField] private List<Medal> medals;
    [SerializeField] private Image bestMedal;

    [SerializeField] private float delayBetweenUnlocks;

    private Coroutine unlockCoroutine = null;

    public IEnumerator UnlockMedalsCoroutine(float levelClearTime)
    {
        foreach (Medal medal in medals)
        {
            Debug.Log($"Time was: {TimeFormatting.ToMs(levelClearTime)}, Medal:{medal.name} required: {medal.timeInMs} ");
            if (TimeFormatting.ToMs(levelClearTime) < medal.timeInMs)
            {
                medal.Unlock();
                bestMedal.sprite = medal.medalSprite;
            }

            yield return new WaitForSecondsRealtime(delayBetweenUnlocks);
        }
    }
}
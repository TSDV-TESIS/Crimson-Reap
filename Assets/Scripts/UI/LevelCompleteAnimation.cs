using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SearchService;
using UnityEngine.UI;

public class LevelCompleteAnimation : MonoBehaviour
{
    [SerializeField] private Image flash;
    [SerializeField] private float flashDuration;
    [SerializeField] private float inMovementDuration;
    [SerializeField] private float holdDuration;
    [SerializeField] private float outMovementDuration;

    [SerializeField] private float textPopDuration;

    [SerializeField] private RectTransform leftPosition;
    [SerializeField] private RectTransform rightPosition;

    [SerializeField] private RectTransform jester;
    [SerializeField] private RectTransform bar;

    [SerializeField] private TextMeshProUGUI levelCompleteText;
    [SerializeField] private TextMeshProUGUI timerText;

    public IEnumerator LevelCompleteStart(float time)
    {
        yield return StartCoroutine(Flash());
        yield return StartCoroutine(InMovement());
        yield return StartCoroutine(PopText(time));
        yield return new WaitForSecondsRealtime(holdDuration);
        yield return StartCoroutine(FadeText());
        yield return StartCoroutine(OutMovement());
        gameObject.SetActive(false);
    }

    private IEnumerator Flash()
    {
        float timer = 0;
        float startTime = Time.unscaledTime;
        flash.color = Color.white;
        flash.gameObject.SetActive(true);

        while (timer < flashDuration)
        {
            timer = Time.unscaledTime - startTime;
            flash.color = new Color(flash.color.r, flash.color.g, flash.color.b, 1 - timer / flashDuration);
            yield return null;
        }

        flash.gameObject.SetActive(false);
    }

    private IEnumerator InMovement()
    {
        float timer = 0;
        float startTime = Time.unscaledTime;
        Vector2 jesterAnchoredPos = jester.anchoredPosition;
        Vector2 barAnchoredPos = bar.anchoredPosition;
        while (timer < inMovementDuration)
        {
            timer = Time.unscaledTime - startTime;
            jester.anchoredPosition = Vector2.Lerp(rightPosition.anchoredPosition, jesterAnchoredPos, timer / inMovementDuration);
            bar.anchoredPosition = Vector2.Lerp(leftPosition.anchoredPosition, barAnchoredPos, timer / inMovementDuration);
            yield return null;
        }
    }

    private IEnumerator PopText(float time)
    {
        float timer = 0;
        float startTime = Time.unscaledTime;
        timerText.text = TimeFormatting.GetFormattedTime(time);
        while (timer < textPopDuration)
        {
            timer = Time.unscaledTime - startTime;
            levelCompleteText.color = new Color(levelCompleteText.color.r, levelCompleteText.color.g, levelCompleteText.color.b, timer / textPopDuration);
            timerText.color = new Color(timerText.color.r, timerText.color.g, timerText.color.b, timer / textPopDuration);
            yield return null;
        }
    }

    private IEnumerator FadeText()
    {
        float timer = 0;
        float startTime = Time.unscaledTime;
        while (timer < textPopDuration)
        {
            timer = Time.unscaledTime - startTime;
            levelCompleteText.color = new Color(levelCompleteText.color.r, levelCompleteText.color.g, levelCompleteText.color.b, 1 - timer / textPopDuration);
            timerText.color = new Color(timerText.color.r, timerText.color.g, timerText.color.b, 1 - timer / textPopDuration);
            yield return null;
        }
    }

    private IEnumerator OutMovement()
    {
        float timer = 0;
        float startTime = Time.unscaledTime;
        Vector2 jesterAnchoredPos = jester.anchoredPosition;
        Vector2 barAnchoredPos = bar.anchoredPosition;
        while (timer < outMovementDuration)
        {
            timer = Time.unscaledTime - startTime;
            jester.anchoredPosition = Vector2.Lerp(jesterAnchoredPos, leftPosition.anchoredPosition, timer / outMovementDuration);
            bar.anchoredPosition = Vector2.Lerp(barAnchoredPos, rightPosition.anchoredPosition, timer / outMovementDuration);
            yield return null;
        }
    }
}
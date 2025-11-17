using System.Collections;
using TMPro;
using UnityEngine;

public class Medal : MonoBehaviour
{
    [SerializeField] public int timeInMs;
    [SerializeField] private GameObject fill;
    public Sprite medalSprite;
    [SerializeField] private TextMeshProUGUI timeText;

    [Header("Unlock Anim")]
    [SerializeField] private float animDuration;
    [SerializeField] private AnimationCurve sizeCurve;

    private Coroutine unlockCoroutine = null;

    private void OnEnable()
    {
        timeText.text = TimeFormatting.GetFormattedTime((float)timeInMs / 1000f);
    }

    public void Unlock()
    {
        if (unlockCoroutine != null)
            StopCoroutine(unlockCoroutine);

        unlockCoroutine = StartCoroutine(UnlockAnim());
    }

    private IEnumerator UnlockAnim()
    {
        fill.SetActive(true);

        float timer = 0;
        float startTime = Time.unscaledTime;
        while (timer < animDuration)
        {
            timer = Time.unscaledTime - startTime;
            float scale = sizeCurve.Evaluate(timer / animDuration);
            fill.transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        fill.transform.localScale = Vector3.one;
    }
}
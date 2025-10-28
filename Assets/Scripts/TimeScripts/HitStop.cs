using System.Collections;
using Events.Scriptables;
using UnityEngine;

namespace TimeScripts
{
    public class HitStop : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private TimeStopEventChannelSO onHitStop;

        private Coroutine _hitStopCoroutine;

        private void OnEnable()
        {
            onHitStop?.onTypedEvent.AddListener(HandleHitStop);
        }

        private void OnDisable()
        {
            onHitStop?.onTypedEvent.RemoveListener(HandleHitStop);
        }

        private void HandleHitStop(TimeFreezeProperties properties)
        {
            if (_hitStopCoroutine != null) StopCoroutine(_hitStopCoroutine);
            StartCoroutine(HitStopCoroutine(properties));
        }

        private IEnumerator HitStopCoroutine(TimeFreezeProperties properties)
        {
            float timer = 0;
            TimeManager.Instance.TrySetTimeScale(properties.slowDown);
            while (timer < properties.duration)
            {
                timer += Time.unscaledDeltaTime;
                yield return null;
            }

            TimeManager.Instance.TrySetTimeScale(1);
        }
    }
}
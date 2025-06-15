using System.Collections;
using Events.Scriptables;
using UnityEngine;
using Event = AK.Wwise.Event;

namespace Objects.HatchDoor
{
    [RequireComponent(typeof(Collider))]
    public class HatchController : MonoBehaviour, IOpenable
    {
        [SerializeField] private float hatchTimeUntilOpening;
        [SerializeField] private float hatchTimeUntilClosing;

        [SerializeField] private float openCloseAnimDuration;

        [Header("Sounds")]
        [SerializeField] private AkWwiseEventChannelSO onPlayEvent;
        [SerializeField] private Event openHatchEvent;

        private Coroutine _hatchCoroutine;
        private Collider _collider;
        private bool _isMoving;

        private void OnEnable()
        {
            _collider ??= GetComponent<Collider>();
        }

        public void Open()
        {
            if (_isMoving)
                return;

            _isMoving = true;

            if (_hatchCoroutine != null)
                StopCoroutine(_hatchCoroutine);

            _hatchCoroutine = StartCoroutine(HatchOpenCloseCoroutine());
        }

        private IEnumerator HatchOpenCloseCoroutine()
        {
            yield return new WaitForSeconds(hatchTimeUntilOpening);
            _collider.enabled = false;
            yield return HatchRotationAnim(Quaternion.Euler(-90, 0, 0));

            yield return new WaitForSeconds(hatchTimeUntilClosing);
            yield return HatchRotationAnim(Quaternion.Euler(0, 0, 0));
            _collider.enabled = true;
            _isMoving = false;
        }

        private IEnumerator HatchRotationAnim(Quaternion targetRotation)
        {
            onPlayEvent.onTypedEvent.Invoke(openHatchEvent);
            float startTime = Time.time;
            float timer = 0;
            while (timer < openCloseAnimDuration)
            {
                timer = Time.time - startTime;
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, timer / openCloseAnimDuration);
                yield return null;
            }
        }
    }
}
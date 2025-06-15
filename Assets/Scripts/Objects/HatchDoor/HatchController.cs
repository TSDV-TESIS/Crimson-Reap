using System.Collections;
using Events.Scriptables;
using UnityEngine;
using UnityEngine.Serialization;

namespace Objects.HatchDoor
{
    public class HatchController : MonoBehaviour, IOpenable
    {
        [SerializeField] private float hatchTimeUntilOpening;
        [SerializeField] private float hatchTimeUntilClosing;

        [SerializeField] private float openCloseAnimDuration;
        [SerializeField] private HatchContactHandler hatchContactHandler;

        [Header("Sounds")]
        [SerializeField] private AkWwiseEventChannelSO onPlayEvent;
        [SerializeField] private AK.Wwise.Event openHatchEvent;

        private Coroutine _hatchCoroutine;

        public void Open()
        {
            if (_hatchCoroutine != null)
                StopCoroutine(_hatchCoroutine);

            _hatchCoroutine = StartCoroutine(HatchOpenCloseCoroutine());
        }

        private IEnumerator HatchOpenCloseCoroutine()
        {
            yield return new WaitForSeconds(hatchTimeUntilOpening);
            hatchContactHandler.enabled = false;
            yield return HatchRotationAnim(Quaternion.Euler(-90, 0, 0));

            yield return new WaitForSeconds(hatchTimeUntilClosing);
            yield return HatchRotationAnim(Quaternion.Euler(0, 0, 0));
            hatchContactHandler.enabled = true;
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
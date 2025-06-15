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

        [SerializeField] private GameObject model;
        
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
            model.SetActive(false);
            onPlayEvent.onTypedEvent.Invoke(openHatchEvent);
            yield return new WaitForSeconds(hatchTimeUntilClosing);
            onPlayEvent.onTypedEvent.Invoke(openHatchEvent);
            model.SetActive(true);
        }
    }
}
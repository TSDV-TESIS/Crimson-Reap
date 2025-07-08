using System.Collections;
using Events.Scriptables;
using Objects.Door;
using Sounds;
using UnityEngine;
using Event = AK.Wwise.Event;

namespace Objects
{
    [RequireComponent(typeof(Collider))]
    public class DoorHandler : MonoBehaviour, IOpenable
    {
        private static readonly int OpenParameter = Animator.StringToHash("Open");
        private static readonly int DoorOpenSpeed = Animator.StringToHash("DoorOpenSpeed");

        [SerializeField] private DoorProperties doorProperties;
        [SerializeField] private SoundCollisionHandler soundCollisionHandler;
        [SerializeField] private GameObject doorModel;
        [SerializeField] private DoorContactKill doorContactKill;

        [Header("Sounds")]
        [SerializeField] private AkWwiseEventChannelSO onPlayEvent;
        [SerializeField] private Event openDoorEvent;

        private Collider _collider;
        private Coroutine _doorOpenCoroutine;
        private bool _isAttacked;

        private void OnEnable()
        {
            _collider = GetComponent<Collider>();
            _isAttacked = false;
            soundCollisionHandler.SoundRadius = doorProperties.doorSoundRadius;
        }

        public void Open()
        {
            _isAttacked = true;
            OnInteract();
        }

        private IEnumerator DoorOpen()
        {
            onPlayEvent.onTypedEvent.Invoke(openDoorEvent);

            Animator doorAnim = doorModel.GetComponent<Animator>();
            doorAnim.SetFloat(DoorOpenSpeed, doorProperties.doorOpenTime);
            doorAnim.SetBool(OpenParameter, true);
            gameObject.layer = LayerMask.NameToLayer("OpenedDoor");
            doorContactKill.gameObject.layer = LayerMask.NameToLayer("OpenedDoor");
            doorModel.layer = LayerMask.NameToLayer("OpenedDoor");
            _collider.enabled = false;
            doorContactKill.SetTrigger();

            soundCollisionHandler.SoundRadius = doorProperties.doorSoundRadius;
            soundCollisionHandler.EnableSound();

            yield return new WaitForSeconds(1 / doorProperties.doorOpenTime);

            soundCollisionHandler.DisableSound();
            doorContactKill.TurnOff();
        }

        public void OnInteract()
        {
            if (_doorOpenCoroutine != null) StopCoroutine(_doorOpenCoroutine);
            _doorOpenCoroutine = StartCoroutine(DoorOpen());
        }
    }
}
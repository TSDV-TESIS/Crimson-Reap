using System;
using System.Collections;
using Events.Scriptables;
using Health;
using Sounds;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Objects
{
    [RequireComponent(typeof(BoxCollider))]
    public class DoorHandler : MonoBehaviour, IOpenable

    {
        private static readonly int OpenParameter = Animator.StringToHash("Open");
        private static readonly int DoorOpenSpeed = Animator.StringToHash("DoorOpenSpeed");

        [SerializeField] private DoorProperties doorProperties;
        [SerializeField] private SoundCollisionHandler soundCollisionHandler;
        [SerializeField] private GameObject doorModel;
        
        [Header("Sounds")]
        [SerializeField] private AkWwiseEventChannelSO onPlayEvent;
        [SerializeField] private AK.Wwise.Event openDoorEvent;
        
        private BoxCollider _boxCollider;
        private Coroutine _doorOpenCoroutine;
        private bool _isAttacked;
        
        private void OnEnable()
        {
            _isAttacked = false;
            _boxCollider = GetComponent<BoxCollider>();
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
            _boxCollider.isTrigger = true;
            gameObject.layer = LayerMask.NameToLayer("OpenedDoor");
            doorModel.layer = LayerMask.NameToLayer("OpenedDoor");

            soundCollisionHandler.SoundRadius = doorProperties.doorSoundRadius;
            soundCollisionHandler.EnableSound();

            yield return new WaitForSeconds(1 / doorProperties.doorOpenTime);
            
            soundCollisionHandler.DisableSound();
            _boxCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isAttacked && (doorProperties.enemyLayer & (1 << other.gameObject.layer)) != 0)
            {
                other?.GetComponent<ITakeDamage>()?.TryTakeDamage(doorProperties.openDamage);
            }
        }

        public void OnCollisionEnter(Collision other)
        {
            Debug.Log(other.gameObject.tag);
        }

        public void OnInteract()
        {
            if(_doorOpenCoroutine != null) StopCoroutine(_doorOpenCoroutine);
            _doorOpenCoroutine = StartCoroutine(DoorOpen());
        }
    }
}

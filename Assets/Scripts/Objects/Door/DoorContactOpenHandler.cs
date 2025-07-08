using UnityEngine;

namespace Objects.Door
{
    [RequireComponent(typeof(BoxCollider))]
    public class DoorContactOpenHandler : MonoBehaviour
    {
        [SerializeField] private DoorHandler handler;

        private BoxCollider _collider;
        private bool _hasBeenTriggered;

        void OnEnable()
        {
            _collider ??= GetComponent<BoxCollider>();
            _hasBeenTriggered = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !_hasBeenTriggered)
            {
                handler.Open();
                _hasBeenTriggered = true;
                _collider.enabled = false;
            }
        }
    }
}
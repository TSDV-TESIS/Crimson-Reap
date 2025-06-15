
using UnityEngine;

namespace Objects.HatchDoor
{
    public class HatchContactHandler : MonoBehaviour
    {
        [SerializeField] private HatchController handler;
        
        private bool _hasBeenTriggered;
        void OnEnable()
        {
            _hasBeenTriggered = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !_hasBeenTriggered)
            {
                handler.Open();
                _hasBeenTriggered = true;
            }
        }
    }
}

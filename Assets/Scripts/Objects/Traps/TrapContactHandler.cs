using UnityEngine;

namespace Objects.Traps
{
    public class TrapContactHandler : MonoBehaviour
    {
        [SerializeField] private BaseTrap baseTrap;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
                baseTrap.onTrapContact?.Invoke();
        }
    }
}
using Events.Scriptables;
using UnityEngine;

namespace Enemy.Soul
{
    public class SoulsManager : MonoBehaviour
    {
        [SerializeField] private GameObject soulVfx;
        [SerializeField] private Vector3ChannelSO onEnemyDeathPosition;

        // Update is called once per frame
        void OnEnable()
        {
            onEnemyDeathPosition?.onTypedEvent.AddListener(HandleNewSoul);
        }

        private void OnDisable()
        {
            onEnemyDeathPosition?.onTypedEvent.RemoveListener(HandleNewSoul);
        }

        private void HandleNewSoul(Vector3 deathPosition)
        {
            GameObject soul = Instantiate(soulVfx, transform);
            soul.transform.position = deathPosition;
        }
    }
}

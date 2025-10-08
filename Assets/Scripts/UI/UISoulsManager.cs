using Events.Scriptables;
using UnityEngine;

namespace UI
{
    public class UISoulsManager : MonoBehaviour
    {
        [SerializeField] private GameObject soulVfx;
        [SerializeField] private GameObject handle;
        [SerializeField] private Camera uiCamera;
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
            Vector3 test = uiCamera.WorldToScreenPoint(deathPosition);

            GameObject testVfx = Instantiate(soulVfx, transform);
            testVfx.GetComponent<SoulHandler>().TimerHandler = handle;
            
            RectTransform rTransform = testVfx.GetComponent<RectTransform>();
            rTransform.localPosition = new Vector3(test.x - Screen.width / 2f, test.y - Screen.height / 2f, rTransform.localPosition.z);
        }
    }
}

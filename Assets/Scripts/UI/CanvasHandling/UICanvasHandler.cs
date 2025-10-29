using Events.Scriptables;
using UnityEngine;

namespace UI
{
    public class UICanvasHandler : MonoBehaviour
    {
        [SerializeField] private GameObjectEventChannelSO onNewSelectedObjectEvent;
        [SerializeField] private GameObject enabledPreselectedGameObject;
        [SerializeField] private GameObject disabledPreselectedGameObject;
        
        private void OnEnable()
        {
            if (enabledPreselectedGameObject != null)
            {
                onNewSelectedObjectEvent?.RaiseEvent(enabledPreselectedGameObject);
            }
        }
        private void OnDisable()
        {
            if (disabledPreselectedGameObject != null)
            {
                onNewSelectedObjectEvent?.RaiseEvent(disabledPreselectedGameObject);
            }
        }
    }
}

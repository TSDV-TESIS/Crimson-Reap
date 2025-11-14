using Events.Scriptables;
using UnityEngine;

namespace UI
{
    public class UICanvasHandler : MonoBehaviour
    {
        [SerializeField] private GameObjectEventChannelSO onNewSelectedObjectEvent;
        [SerializeField] private GameObject enabledPreselectedGameObject;
        [SerializeField] private GameObject disabledPreselectedGameObject;
        [SerializeField] private PanelHandling panelHandler;

        private void OnEnable()
        {
            panelHandler.SetPanel(gameObject);
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
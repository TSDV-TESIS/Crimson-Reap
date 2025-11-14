using Events.Scriptables;
using UnityEngine;

namespace UI
{
    public class PlayHandler : MonoBehaviour
    {
        [SerializeField] private GameObject inputPanel;
        [SerializeField] private GameObject inputField;
        [SerializeField] private GameObjectEventChannelSO onNewSelectedObject;
        [SerializeField] private PanelHandling panelHandler;

        public void OnClick()
        {
            inputPanel.SetActive(true);
            panelHandler.SetPanel(inputPanel);
            onNewSelectedObject?.RaiseEvent(inputField);
        }
    }
}
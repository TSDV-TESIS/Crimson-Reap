using System;
using Events.Scriptables;
using Player;
using UnityEngine;

namespace UI
{
    public class PlayHandler : MonoBehaviour
    {
        [SerializeField] private GameObject inputPanel;
        [SerializeField] private GameObject inputField;
        [SerializeField] private GameObjectEventChannelSO onNewSelectedObject;
        [SerializeField] private InputHandler input;

        private void OnEnable()
        {
            input.onCancel.AddListener(HandleCancel);
        }

        public void OnClick()
        {
            inputPanel.SetActive(true);
            onNewSelectedObject?.RaiseEvent(inputField);
        }

        private void HandleCancel()
        {
            inputPanel.SetActive(false);
            onNewSelectedObject?.RaiseEvent(gameObject);
        }
    }
}
using System;
using Events.Scriptables;
using Player;
using UnityEngine;

namespace UI
{
    public class PlayHandler : MonoBehaviour
    {
        [SerializeField] private GameObject inputPane_main;
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
            inputPane_main.SetActive(true);
            inputPanel.SetActive(true);
            onNewSelectedObject?.RaiseEvent(inputField);
        }

        private void HandleCancel()
        {
            inputPane_main.SetActive(false);
            inputPanel.SetActive(false);
            onNewSelectedObject?.RaiseEvent(gameObject);
        }
    }
}
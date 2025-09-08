using System;
using System.Collections;
using Events;
using Events.Scriptables;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private VoidEventChannelSO onPlayerDeath;
        [SerializeField] private VoidEventChannelSO onRestart;
        [SerializeField] private Vector3ChannelSO onDoorPosition;
        [SerializeField] private RectTransform goObject;
        
        [Header("Go Sign properties")]
        [SerializeField] private Vector2 maxGoPositions;
        [SerializeField] private Vector2 minGoPositions;
        [SerializeField] private Vector2 minInScreenPosition;
        [SerializeField] private Vector2 maxInScreenPosition;
    
        void OnEnable()
        {
            onPlayerDeath.onEvent.AddListener(HandlePlayerDeath);
            onDoorPosition.onTypedEvent.AddListener(HandleDoorPosition);
            onRestart.onEvent.AddListener(HandleRestart);
        }

        private void OnDisable()
        {
            onPlayerDeath?.onEvent.RemoveListener(HandlePlayerDeath);
            onDoorPosition.onTypedEvent.RemoveListener(HandleDoorPosition);
            onRestart.onEvent.RemoveListener(HandleRestart);
        }

        private void HandleRestart()
        {
            gameOverPanel.SetActive(false);
        }
        
        private void HandleDoorPosition(Vector3 position)
        {
            if (DoorIsInScreen(position))
            {
                goObject.gameObject.SetActive(false);
                return;
            }
            
            goObject.gameObject.SetActive(true);

            var vector3 = goObject.localPosition;
            vector3.x = Mathf.Clamp(position.x, minGoPositions.x, maxGoPositions.x);
            vector3.y = Mathf.Clamp(position.y, minGoPositions.y, maxGoPositions.y);
            goObject.localPosition = vector3;
        }

        private bool DoorIsInScreen(Vector3 position)
        {
            return position.x > minInScreenPosition.x && position.x < maxInScreenPosition.x &&
                   position.y > minInScreenPosition.y && position.y < maxInScreenPosition.y;
        }

        private void HandlePlayerDeath()
        {
            gameOverPanel.SetActive(true);
        }
    }
}

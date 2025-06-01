using System;
using System.Collections;
using Events;
using Events.Scriptables;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class SceneReloader : MonoBehaviour
    {
        [SerializeField] private InputHandler input;
        
        [SerializeField] private VoidEventChannelSO onPlayerDeath;
        [SerializeField] private VoidEventChannelSO onRestart;
        [SerializeField] private float panelDuration;
        
        [SerializeField] private StringEventChannelSO onLoadScene;
        
        void OnEnable()
        {
            input.OnRestartScene.AddListener(RestartScene);
            onPlayerDeath.onEvent.AddListener(HandleRestartScene);
        }

        private void OnDisable()
        {
            input.OnRestartScene.AddListener(RestartScene);
            onPlayerDeath.onEvent.AddListener(HandleRestartScene);
        }

        private void HandleRestartScene()
        {
            StartCoroutine(GameOverCoroutine());
        }

        private IEnumerator GameOverCoroutine()
        {
            yield return new WaitForSeconds(panelDuration);
            onRestart?.RaiseEvent();
            RestartScene();
        }

        public void RestartScene()
        {
            onLoadScene?.RaiseEvent(SceneManager.GetActiveScene().name);
        }
    }
}

using System.Collections;
using Events;
using Events.Scriptables;
using Player;
using UnityEngine;

namespace Managers
{
    public class SceneReloader : MonoBehaviour
    {
        [SerializeField] private InputHandler input;
        
        [SerializeField] private VoidEventChannelSO onPlayerDeath;
        [SerializeField] private VoidEventChannelSO onRestart;
        [SerializeField] private float panelDuration;
        
        [SerializeField] private StringEventChannelSO onLoadScene;
        [SerializeField] private string levelScene;

        private Coroutine _handleRestartSceneCoroutine;
        
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
            if (_handleRestartSceneCoroutine != null) StopCoroutine(_handleRestartSceneCoroutine);
            _handleRestartSceneCoroutine = StartCoroutine(GameOverCoroutine());
        }

        private IEnumerator GameOverCoroutine()
        {
            yield return new WaitForSeconds(panelDuration);
            onRestart?.RaiseEvent();
            RestartScene();
        }

        public void RestartScene()
        {
            onLoadScene?.RaiseEvent(levelScene);
        }
    }
}

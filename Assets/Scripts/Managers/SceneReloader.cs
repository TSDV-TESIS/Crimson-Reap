using System.Collections;
using Events;
using Events.Scriptables;
using Health;
using Player;
using UnityEngine;

namespace Managers
{
    public class SceneReloader : MonoBehaviour
    {
        [SerializeField] private InputHandler input;

        [SerializeField] private DeathEventChannelSO onPlayerDeath;
        [SerializeField] private VoidEventChannelSO onRestart;
        [SerializeField] private VoidEventChannelSO onShouldRestart;
        [SerializeField] private float panelDuration;

        [SerializeField] private StringEventChannelSO onLoadScene;
        [SerializeField] private string levelScene;

        private Coroutine _handleRestartSceneCoroutine;

        void OnEnable()
        {
            input.OnRestartScene.AddListener(RestartScene);
            onShouldRestart.onEvent.AddListener(RestartScene);
            onPlayerDeath.onTypedEvent.AddListener(HandleRestartScene);
        }

        private void OnDisable()
        {
            input.OnRestartScene.RemoveListener(RestartScene);
            onShouldRestart.onEvent.RemoveListener(RestartScene);
            onPlayerDeath.onTypedEvent.RemoveListener(HandleRestartScene);
        }

        private void HandleRestartScene(DeathCauses cause)
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
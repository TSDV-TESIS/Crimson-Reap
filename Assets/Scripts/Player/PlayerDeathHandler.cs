using Events;
using Health;
using Player.Controllers;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(HealthPoints))]
    [RequireComponent(typeof(PlayerAgent))]
    [RequireComponent(typeof(PlayerAnimationController))]
    public class PlayerDeathHandler : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO onPlayerDeath;
        [SerializeField] private float waitSeconds = 0.5f;
        
        private HealthPoints _healthPoints;
        private PlayerAgent _agent;
        private PlayerAnimationController _controller;

        private void OnEnable()
        {
            _healthPoints ??= GetComponent<HealthPoints>();
            _agent ??= GetComponent<PlayerAgent>();
            _controller ??= GetComponent<PlayerAnimationController>();

            onPlayerDeath?.onEvent?.AddListener(HandleDeath);
        }

        private void OnDisable()
        {
            onPlayerDeath?.onEvent.RemoveListener(HandleDeath);
        }

        private void HandleDeath()
        {
            _healthPoints.SetCanTakeDamage(false);
            _agent.StopFsm();
            _controller.HandleDeath();
        }
    }
}

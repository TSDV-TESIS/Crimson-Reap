using Events;
using Events.Scriptables;
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
        [SerializeField] private DeathEventChannelSO onPlayerDeath;
        [SerializeField] private VoidEventChannelSO onPlayerKilled;
        [SerializeField] private VoidEventChannelSO onPlayerTimeDeath;
        [SerializeField] private VoidEventChannelSO onPlayerEnviromentDeath;

        [SerializeField] private float waitSeconds = 0.5f;
        private HealthPoints _healthPoints;
        private PlayerAgent _agent;
        private PlayerAnimationController _controller;

        private void OnEnable()
        {
            _healthPoints ??= GetComponent<HealthPoints>();
            _agent ??= GetComponent<PlayerAgent>();
            _controller ??= GetComponent<PlayerAnimationController>();

            onPlayerDeath?.onTypedEvent?.AddListener(HandleDeath);
        }

        private void OnDisable()
        {
            onPlayerDeath?.onTypedEvent?.RemoveListener(HandleDeath);
        }

        private void HandleDeath(DeathCauses cause)
        {
            _healthPoints.SetCanTakeDamage(false);
            _agent.StopFsm();
            _controller.HandleDeath();
            switch (cause)
            {
                case DeathCauses.Environment:
                    onPlayerEnviromentDeath?.onEvent.Invoke();
                    break;
                case DeathCauses.External:
                    onPlayerKilled?.onEvent.Invoke();
                    break;
                case DeathCauses.Internal:
                    onPlayerTimeDeath?.onEvent.Invoke();
                    break;
            }
        }
    }
}
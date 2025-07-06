using System;
using Health;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Controllers
{
    [RequireComponent(typeof(HealthPoints))]
    public class CheatsController : MonoBehaviour
    {
        [SerializeField] private InputHandler handler;
        [SerializeField] private PlayerCheatProperties properties;
        
        private HealthPoints _health;
        private bool _lastInvincibleValue;
        
        void OnEnable()
        {
            handler.onInvincible.AddListener(HandleInvincible);
            _health ??= GetComponent<HealthPoints>();
            _lastInvincibleValue = false;
        }

        private void OnDisable()
        {
            handler.onInvincible.RemoveListener(HandleInvincible);
        }

        void HandleInvincible()
        {
            if (!properties.cheatsEnabled) return;
            Debug.Log("INVINCIBLE!");
            _lastInvincibleValue = !_lastInvincibleValue;
            _health.SetIsInvincible(_lastInvincibleValue);
        }
    }
}

using System;
using Events;
using Health;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Bars
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private bool shouldStartHided;
        [SerializeField] private Material healthMaterial;
        
        [Header("Events")]
        [SerializeField] private IntEventChannelSO onTakeDamage;
        [SerializeField] private IntEventChannelSO onSumHealth;
        [SerializeField] private IntEventChannelSO onResetDamage;
        [SerializeField] private IntEventChannelSO onInitializeSlider;
        
        private bool _wasTriggered = false;
        private int _maxHealthValue;
        private static readonly int HealthParam = Shader.PropertyToID("_Health");

        private void Awake()
        {
            onInitializeSlider?.onIntEvent.AddListener(HandleInit);
        }

        void Start()
        {
            healthMaterial.SetFloat(HealthParam, 1);
            onSumHealth?.onIntEvent.AddListener(HandleTakeDamage);
            onTakeDamage?.onIntEvent.AddListener(HandleTakeDamage);
            onResetDamage?.onIntEvent.AddListener(HandleReset);
        }

        private void OnDestroy()
        {
            onSumHealth?.onIntEvent?.RemoveListener(HandleTakeDamage);
            onTakeDamage?.onIntEvent.RemoveListener(HandleTakeDamage);
            onInitializeSlider?.onIntEvent.RemoveListener(HandleInit);
            onResetDamage?.onIntEvent.RemoveListener(HandleReset);
        }

        public void HandleReset(int currentHp)
        {
            _maxHealthValue = currentHp;
            healthMaterial.SetFloat(HealthParam, 1);
        }
    
        public void HandleInit(int maxValue)
        {
            _maxHealthValue = maxValue;
            healthMaterial.SetFloat(HealthParam, 1);
        }
        
        public void HandleTakeDamage(int currentHealth)
        {
            if (!_wasTriggered)
            {
                _wasTriggered = true;
            }
            healthMaterial.SetFloat(HealthParam, (float)currentHealth / _maxHealthValue);
        }
    }
}

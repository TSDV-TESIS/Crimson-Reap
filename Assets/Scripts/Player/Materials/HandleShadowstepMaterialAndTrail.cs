using System;
using Events.Scriptables;
using Player.Properties;
using UnityEngine;

namespace Player.Materials
{
    public class HandleShadowstepMaterialAndTrail : MonoBehaviour
    {
        [SerializeField] private Material material;
        [SerializeField] private TrailRenderer trailRenderer;
        [SerializeField] private TrailRenderer otherTrailRenderer;
        [SerializeField] private PlayerMovementProperties playerMovementProperties;
        
        [Header("Events")] [SerializeField] private FloatEventChannel onShadowStepCooldown; 
        
        private static readonly int EmissionBool = Shader.PropertyToID("_Emission_Bool");
        private static readonly int IsFrenetic = Shader.PropertyToID("_is_Frenetic");

        private void OnEnable()
        {
            SetShadowStepMaterial(false);
            trailRenderer.emitting = false;
            otherTrailRenderer.emitting = false;
            
            onShadowStepCooldown.onFloatEvent.AddListener(HandleShadowStepCooldown);
        }

        private void OnDisable()
        {
            onShadowStepCooldown.onFloatEvent.RemoveListener(HandleShadowStepCooldown);
        }

        private void HandleShadowStepCooldown(float percentageFilled)
        {
            if (Mathf.Approximately(percentageFilled, 1.0f))
            {
                SetShadowStepMaterial(false);
                trailRenderer.emitting = false;
                otherTrailRenderer.emitting = false;
                return;
            }
            
            float timeValue = playerMovementProperties.shadowStepShowTrailTime * (1 - percentageFilled);
            trailRenderer.time = timeValue;
            otherTrailRenderer.time = timeValue;
        }

        public void SetShadowStepMaterial(bool value)
        {
            int valueToUse = value ? 1 : 0;
            material.SetInt(EmissionBool, valueToUse);
            material.SetInt(IsFrenetic, valueToUse);

            if (value)
            {
                trailRenderer.emitting = true;
                otherTrailRenderer.emitting = true;
                trailRenderer.time = playerMovementProperties.shadowStepShowTrailTime;
                otherTrailRenderer.time = playerMovementProperties.shadowStepShowTrailTime;
            }
        }
    }
}

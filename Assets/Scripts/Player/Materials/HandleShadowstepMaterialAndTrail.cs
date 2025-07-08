using System;
using UnityEngine;

namespace Player.Materials
{
    public class HandleShadowstepMaterialAndTrail : MonoBehaviour
    {
        [SerializeField] private Material material;
        [SerializeField] private TrailRenderer trailRenderer;
        [SerializeField] private TrailRenderer otherTrailRenderer;
        
        private static readonly int EmissionBool = Shader.PropertyToID("_Emission_Bool");
        private static readonly int IsFrenetic = Shader.PropertyToID("_is_Frenetic");

        private void OnEnable()
        {
            SetShadowStepMaterial(false);
            trailRenderer.emitting = false;
            otherTrailRenderer.emitting = false;
        }

        public void SetShadowStepMaterial(bool value)
        {
            Debug.LogWarning($"SET SHADOW STEP MATERIAL {value}");
            int valueToUse = value ? 1 : 0;
            material.SetInt(EmissionBool, valueToUse);
            material.SetInt(IsFrenetic, valueToUse);
            trailRenderer.emitting = value;
            otherTrailRenderer.emitting = value;
        }
    }
}

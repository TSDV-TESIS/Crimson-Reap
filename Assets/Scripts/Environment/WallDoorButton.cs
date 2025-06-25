using System;
using UnityEngine;

namespace Environment
{
    public class WallDoorButton : MonoBehaviour, IInteractable
    {
        [SerializeField] private Material highlightMaterial;
        [SerializeField] private GameObject door;
        [SerializeField] private float handleAnimationSpeed;
        [SerializeField] private Animator handleAnimator;
        [SerializeField] private MeshRenderer modelMesh;
        
        private Material _defaultMaterial;
        private static readonly int AnimationSpeed = Animator.StringToHash("AnimationSpeed");
        private static readonly int HandleDown = Animator.StringToHash("HandleDown");

        private bool _hasInteracted;
        private void OnEnable()
        {
            _defaultMaterial = modelMesh.material;
            _hasInteracted = false;
            handleAnimator.SetFloat(AnimationSpeed, handleAnimationSpeed);
        }

        public void OnInteract()
        {
            Debug.Log("PlayerInteracted");
            door.SetActive(false);
            _hasInteracted = true;
            modelMesh.material = _defaultMaterial;
            handleAnimator.SetBool(HandleDown, true);
        }

        public void Highlight(bool shouldHighlight)
        {
            modelMesh.material = shouldHighlight && !_hasInteracted ? highlightMaterial : _defaultMaterial;
        }
    }
}
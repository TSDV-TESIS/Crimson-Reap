using UnityEngine;

namespace Environment
{
    public abstract class Button : MonoBehaviour, IInteractable
    {
        [SerializeField] private Material highlightMaterial;
        [SerializeField] private float handleAnimationSpeed;
        [SerializeField] private Animator handleAnimator;
        [SerializeField] private MeshRenderer modelMesh;
        [SerializeField] private bool canInteractAgain;
        [SerializeField] private bool initStatus;
        
        private Material _defaultMaterial;
        private static readonly int AnimationSpeed = Animator.StringToHash("AnimationSpeed");
        private static readonly int HandleDown = Animator.StringToHash("HandleDown");

        private bool _value;
        private void OnEnable()
        {
            _defaultMaterial = modelMesh.material;
            _value = false;
            handleAnimator.SetFloat(AnimationSpeed, handleAnimationSpeed);

            _value = initStatus;
            Interacted(_value);
        }

        public void OnInteract()
        {
            Debug.Log("PlayerInteracted");
            _value = !_value;
            Interacted(_value);
            if(!canInteractAgain)
                modelMesh.material = _defaultMaterial;
            
            handleAnimator.SetBool(HandleDown, _value);
        }

        public bool GetValue()
        {
            return _value;
        }

        public void Highlight(bool shouldHighlight)
        {
            modelMesh.material = shouldHighlight && (canInteractAgain || !_value) ? highlightMaterial : _defaultMaterial;
        }
        
        protected abstract void Interacted(bool value);
    }
}
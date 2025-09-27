using UnityEngine;

namespace Utils
{
    public class BlockColliderHider : MonoBehaviour
    {
        [SerializeField] private BlockCollidersOptions options;
        [SerializeField] private Material materialToReplace;
        [SerializeField] private MeshRenderer meshRenderer;

        private Material _previousMaterial;
    
        void Awake()
        {
            if (options.shouldReplaceWithTheirMaterials)
            {
                _previousMaterial = meshRenderer.material;
                meshRenderer.material = materialToReplace;
                return;
            }
        
            if (options.shouldHideOnAwake)
                meshRenderer.enabled = false;
        }

        private void OnDestroy()
        {
            if (options.shouldReplaceWithTheirMaterials)
            {
                meshRenderer.material = _previousMaterial;
                return;
            }
        
            if (options.shouldHideOnAwake)
                meshRenderer.enabled = true;
        }

        public void Hide()
        {
            meshRenderer.enabled = false;
        }

        public void Show()
        {
            meshRenderer.enabled = true;
        }
    }
}

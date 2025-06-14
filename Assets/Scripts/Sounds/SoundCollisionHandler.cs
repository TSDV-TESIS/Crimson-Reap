using System;
using UnityEngine;

namespace Sounds
{
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public class SoundCollisionHandler : MonoBehaviour
    {
        [SerializeField] private GameObject screamingPoint;
        [SerializeField] private bool enableOnAwake = false;
        
        private float _soundRadius;
        private bool _shouldShowMesh;
        private SphereCollider _collider;
        private MeshRenderer _meshRenderer;

        public float SoundRadius
        {
            set => SetSoundRadius(value);
            get => _soundRadius;
        }

        public void EnableSound()
        {
            _collider.enabled = true;
        }

        public void DisableSound()
        {
            _collider.enabled = false;
        }

        public void EnableSound(bool debug)
        {
            EnableSound();
            if (_meshRenderer) _meshRenderer.enabled = debug;
        }

        public GameObject GetScreamingPoint()
        {
            return screamingPoint;
        }
        
        private void SetSoundRadius(float newRadius)
        {
            _soundRadius = newRadius;
            transform.localScale = new Vector3(_soundRadius, _soundRadius, _soundRadius);
        }

        private void Awake()
        {
            _collider ??= GetComponent<SphereCollider>();
            _collider.enabled = enableOnAwake;

            _meshRenderer ??= GetComponent<MeshRenderer>();
        }
    }
}

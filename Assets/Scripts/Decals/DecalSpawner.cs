using System;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Decals
{
    [RequireComponent(typeof(DecalProjector))]
    public class DecalDissolver : MonoBehaviour
    {
        [NonSerialized] public float TimeToDissolve;
        [NonSerialized] public AnimationCurve DissolveAnim;
        
        private DecalProjector _projector;
        private float _timePassed;
        private void Start()
        {
            _projector = GetComponent<DecalProjector>();
            _timePassed = 0f;
        }

        private void Update()
        {
            if (_timePassed < TimeToDissolve)
            {
                _projector.fadeFactor = DissolveAnim.Evaluate(_timePassed / TimeToDissolve);
                _timePassed += Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    
    public class DecalSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject decal;
        [SerializeField] private Vector3 rotationOffset;
        [SerializeField] private bool shouldDissolve;
        [SerializeField] private float dissolveTime;
        [SerializeField] private AnimationCurve dissolveAnim;
        
        public void Spawn(Vector3 position, Quaternion rotation)
        {
            GameObject decalObject = Instantiate(decal);
            decalObject.transform.position = position;
            decalObject.transform.rotation = rotation * Quaternion.Euler(rotationOffset);

            if (shouldDissolve)
            {
                DecalDissolver dissolver = decalObject.AddComponent<DecalDissolver>();
                dissolver.TimeToDissolve = dissolveTime;
                dissolver.DissolveAnim = dissolveAnim;
            }
        }
    }
}

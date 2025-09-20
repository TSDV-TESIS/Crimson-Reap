using System;
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
        public bool IsDissolved { get; private set; }

        private void Start()
        {
            _projector = GetComponent<DecalProjector>();
            _timePassed = 0f;
            IsDissolved = false;
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
                IsDissolved = true;
            }
        }
    }
}
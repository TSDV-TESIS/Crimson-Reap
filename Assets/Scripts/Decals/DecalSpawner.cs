using System;
using System.Diagnostics.Tracing;
using UnityEngine;

namespace Decals
{
    public class DecalSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject decal;
        [SerializeField] private Vector3 rotationOffset;
        [SerializeField] private bool shouldDissolve;
        [SerializeField] private float dissolveTime;
        [SerializeField] private AnimationCurve dissolveAnim;

        private DecalDissolver _dissolver;
        
        public void Spawn(Vector3 position, Quaternion rotation)
        {
            GameObject decalObject = Instantiate(decal);
            decalObject.transform.position = position;
            decalObject.transform.rotation = rotation * Quaternion.Euler(rotationOffset);

            if (shouldDissolve)
            {
                _dissolver = decalObject.AddComponent<DecalDissolver>();
                _dissolver.TimeToDissolve = dissolveTime;
                _dissolver.DissolveAnim = dissolveAnim;
            }
        }

        private void Update()
        {
            if (_dissolver.IsDissolved)
            {
                Destroy(gameObject);
            }
        }
    }
}

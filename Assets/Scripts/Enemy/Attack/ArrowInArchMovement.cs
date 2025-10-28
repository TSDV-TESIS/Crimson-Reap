using System;
using UnityEngine;

namespace Enemy.Attack
{
    public class ArrowInArchMovement : MonoBehaviour
    {
        [SerializeField] private float movementPercentage;
        [SerializeField] private Vector3 positionInBow;
        
        private Vector3 _initialPosition;
        public SkinnedMeshRenderer BowMesh { get; set; }

        private void OnEnable()
        {
            transform.localPosition = positionInBow;
            _initialPosition = transform.localPosition;
        }

        private void Update()
        {
            if (!BowMesh) return;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, _initialPosition.z - BowMesh.GetBlendShapeWeight(0) * movementPercentage);
        }
    }
}

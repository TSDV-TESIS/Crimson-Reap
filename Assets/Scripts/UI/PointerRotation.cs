using System;
using Events.Scriptables;
using Player;
using UnityEngine;

namespace UI
{
    public class PointerRotation : MonoBehaviour
    {
        [SerializeField] private MouseDataFromPlayer mouseDataFromPlayer;
        [SerializeField] private Transform rotationPoint;
        
        [Header("Configuration")]
        [SerializeField] private float radius;
        [SerializeField] private float moveLerpVelocity;
        [SerializeField] private float rotationLerpVelocity;
        
        private void Update()
        {
            HandleRotatePointer();
        }

        private void HandleRotatePointer()
        {
            Vector3 desiredPosition = rotationPoint.position + (Vector3)(mouseDataFromPlayer.mouseDirection * radius);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * moveLerpVelocity);
            
            float angle = Mathf.Atan2(mouseDataFromPlayer.mouseDirection.y, mouseDataFromPlayer.mouseDirection.x) * Mathf.Rad2Deg;
            Quaternion desiredRot = Quaternion.AngleAxis(angle + 270f, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, Time.deltaTime * rotationLerpVelocity);
        }
    }
}

using System;
using Player.Properties;
using UnityEngine;

namespace Player
{
    public class PlayerRotation : MonoBehaviour
    {
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private GameObject model;
        [SerializeField] private PlayerMovementProperties properties;

        public bool LockRotation { set; get; }

        private void OnEnable()
        {
            LockRotation = false;
        }

        void Update()
        {
            if (LockRotation || Mathf.Abs(playerMovement.Velocity.x) < properties.maxSpeedIdle) return; 
           
            // TODO rotate more gracefully
            if (playerMovement.Velocity.x > 0)
            {
                var rotation = model.transform.rotation;
                rotation.eulerAngles = new Vector3(model.transform.rotation.eulerAngles.x, 90,
                    model.transform.eulerAngles.z);
                model.transform.rotation = rotation;
            }
            else
            {
                var rotation = model.transform.rotation;
                rotation.eulerAngles = new Vector3(model.transform.rotation.eulerAngles.x, 270,
                    model.transform.eulerAngles.z);
                model.transform.rotation = rotation;
            }
        }
    }
}

using System;
using Events;
using Player.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace Player
{
    public class PlayerRotation : MonoBehaviour
    {
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private GameObject[] modelsToRotate;
        [SerializeField] private PlayerMovementProperties properties;

        [Header("Events")] [SerializeField] private VoidEventChannelSO onFinishRotating;

        public bool LockRotation { set; get; }

        private void OnEnable()
        {
            LockRotation = false;
            onFinishRotating?.onEvent.AddListener(HandleUnlockAndRotate);
        }

        private void OnDisable()
        {
            onFinishRotating?.onEvent.RemoveListener(HandleUnlockAndRotate);
        }

        private void Update()
        {
            Rotate();
        }

        private void HandleUnlockAndRotate()
        {
            LockRotation = false;
            Rotate();
        }

        private void Rotate()
        {
            if (LockRotation || Mathf.Abs(playerMovement.Velocity.x) < properties.maxSpeedIdle) return;

            // TODO rotate more gracefully
            if (playerMovement.Velocity.x > 0)
            {
                foreach (GameObject model in modelsToRotate)
                {
                    var rotation = model.transform.rotation;
                    rotation.eulerAngles = new Vector3(model.transform.rotation.eulerAngles.x, 90,
                        model.transform.eulerAngles.z);
                    model.transform.rotation = rotation;
                }
            }
            else
            {
                foreach (GameObject model in modelsToRotate)
                {
                    var rotation = model.transform.rotation;
                    rotation.eulerAngles = new Vector3(model.transform.rotation.eulerAngles.x, 270,
                        model.transform.eulerAngles.z);
                    model.transform.rotation = rotation;
                }
            }
        }
    }
}
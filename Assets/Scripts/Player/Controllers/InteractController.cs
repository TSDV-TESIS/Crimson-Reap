using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player.Controllers
{
    public class InteractController : MonoBehaviour
    {
        [SerializeField] private InputHandler _inputHandler;
        private List<IInteractable> _interactableObjects;

        private void OnEnable()
        {
            _inputHandler.OnInteract.AddListener(TryInteract);
            _interactableObjects = new List<IInteractable>();
        }

        private void OnDisable()
        {
            _inputHandler.OnInteract.RemoveListener(TryInteract);
        }

        private void OnTriggerEnter(Collider other)
        {
            IInteractable interactable = other.GetComponent<IInteractable>();
            if (interactable != null && !_interactableObjects.Contains(interactable))
            {
                _interactableObjects.Add(other.GetComponent<IInteractable>());
                interactable.Highlight(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            IInteractable interactable = other.GetComponent<IInteractable>();

            if (interactable != null && _interactableObjects.Contains(interactable))
            {
                interactable.Highlight(false);
                _interactableObjects.Remove(interactable);
            }
        }

        private void TryInteract()
        {
            Debug.Log($"COUNT {_interactableObjects.Count}");
            foreach (IInteractable interactableObject in _interactableObjects)
            {
                Debug.Log($"Interacting with {interactableObject}");
                interactableObject.OnInteract();
            }
        }
    }
}
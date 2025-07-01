using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Controllers
{
    public class InteractController : MonoBehaviour
    {
        [SerializeField] private InputHandler _inputHandler;
        private List<IInteractable> _interactableObjects;

        private void Start()
        {
            _inputHandler.OnInteract.AddListener(TryInteract);
            _interactableObjects = new List<IInteractable>();
        }

        private void OnTriggerEnter(Collider other)
        {
            IInteractable interactable = other.GetComponent<IInteractable>();
            if (interactable != null)
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
            foreach (IInteractable interactableObject in _interactableObjects)
            {
                interactableObject.OnInteract();
            }
        }
    }
}
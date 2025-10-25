using System;
using Events.Scriptables;
using UnityEngine;

namespace Player.Animations
{
    public class SpikesDeathVelocity : MonoBehaviour
    {
        [SerializeField] private float downVelocity = 0.5f;
        [SerializeField] private StringEventChannelSO onLethalDamageBySpikes;

        private bool _shouldGoDown;
    
        void OnEnable()
        {
            _shouldGoDown = false;
            onLethalDamageBySpikes?.onTypedEvent.AddListener(HandleShouldGoDown);
        }

        private void OnDisable()
        {
            onLethalDamageBySpikes?.onTypedEvent.RemoveListener(HandleShouldGoDown);
        }

        private void HandleShouldGoDown(string _)
        {
            _shouldGoDown = true;
        }

        void Update()
        {
            if (!_shouldGoDown) return;

            var vector3 = transform.position;
            vector3.y -= downVelocity * Time.deltaTime;
            transform.position = vector3;
        }
    }
}

using System;
using Events.Scriptables;
using UnityEngine;

namespace Player.Animations
{

    public class SpikesDeathVelocity : MonoBehaviour
    {
        [SerializeField] private SpikeDeathProperties spikeDeathProperties;
        [SerializeField] private StringEventChannelSO onLethalDamageBySpikes;

        private bool _shouldGoDown;
        private float _timeWaiting;
        private float _timeInserting;
    
        void OnEnable()
        {
            _timeWaiting = 0f;
            _timeInserting = 0f;
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

            if(_timeWaiting <= spikeDeathProperties.timeToInsertInSpike)
            {
                _timeWaiting += Time.deltaTime;
                return;
            }

            if(_timeInserting <= spikeDeathProperties.timeInserting)
            {
                var vector3 = transform.position;
                vector3.y -= spikeDeathProperties.downVelocity * Time.deltaTime;
                transform.position = vector3;

                _timeInserting += Time.deltaTime;
            }

        }
    }
}

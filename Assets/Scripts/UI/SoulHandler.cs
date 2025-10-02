using System;
using System.Collections;
using UnityEngine;

namespace UI
{
    public class SoulHandler : MonoBehaviour
    {
        [SerializeField] private float secondsUntilMove;
        [SerializeField] private float secondsInMove;
        [SerializeField] private AnimationCurve velocityCurve;
        [SerializeField] private float distanceToHandler;
        
        [NonSerialized] public GameObject TimerHandler;

        private float _secondsAlive;
        private Coroutine _movingCoroutine;

        private void OnEnable()
        {
            _secondsAlive = 0f;
            _movingCoroutine = null;
        }

        private void Update()
        {
            if (_movingCoroutine != null) return;
            
            _secondsAlive += Time.deltaTime;
            if (_secondsAlive < secondsUntilMove) return;

            _movingCoroutine = StartCoroutine(MoveSoul());
        }

        private IEnumerator MoveSoul()
        {
            _secondsAlive = 0f;

            while (_secondsAlive < secondsInMove)
            {
                float percentage = velocityCurve.Evaluate(_secondsAlive / secondsInMove);
                
                transform.position =
                    Vector3.LerpUnclamped(transform.position, TimerHandler.transform.position, percentage);

                _secondsAlive += Time.deltaTime;
                yield return null;
            }
            
            Destroy(gameObject);
        }
    }
}
using System.Collections;
using Events;
using Player.Properties;
using UnityEngine;

namespace Enemy.Soul
{
    public class SoulHandler : MonoBehaviour
    {
        [SerializeField] private float secondsUntilMove;
        [SerializeField] private float secondsInMove;
        [SerializeField] private AnimationCurve velocityCurve;
        [SerializeField] private float distanceToPlayer = 0.5f;
        [SerializeField] private PlayerTransform playerTransform;
        [SerializeField] private VoidEventChannelSO onSoulClose;
        
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
            
            while (_secondsAlive < secondsInMove &&
                   (transform.position - playerTransform.playerTransform.position).magnitude > distanceToPlayer)
            {
                float percentage = velocityCurve.Evaluate(_secondsAlive / secondsInMove);

                transform.position =
                    Vector3.LerpUnclamped(transform.position, playerTransform.playerTransform.position, percentage);

                _secondsAlive += Time.deltaTime;
                yield return null;
            }
            
            onSoulClose?.RaiseEvent();
            Destroy(gameObject);
        }
    }
}
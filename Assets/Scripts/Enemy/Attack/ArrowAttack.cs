using System;
using System.Collections;
using Health;
using UnityEngine;

namespace Enemy.Attack
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class ArrowAttack : MonoBehaviour
    {
        [SerializeField] private int damage = 1000;
        [SerializeField] private ArrowAttackProperties properties;
        [SerializeField] private Light pointLight;
        [SerializeField] private float dimDuration;

        private CapsuleCollider _collider;
        private Coroutine _arrowDestroyCoroutine;
        private Coroutine _lightDimCoroutine;

        private bool _isTraveling;
        private float _velocity;
        private Vector3 _direction;

        private void OnEnable()
        {
            _isTraveling = true;
            _collider ??= GetComponent<CapsuleCollider>();
        }

        private void Update()
        {
            if (_isTraveling)
                transform.position += _velocity * _direction * Time.deltaTime;
        }

        public void SetVelocityAndDirection(float velocity, Vector3 target)
        {
            _direction = (target - transform.position).normalized;
            _velocity = velocity;
            transform.LookAt(target);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.TryGetComponent<ITakeDamage>(out ITakeDamage takeDamageObject))
            {
                takeDamageObject.TryTakeDamage(damage);
                Destroy(gameObject);
                return;
            }

            if ((properties.whatIsStoppableColliders & (1 << other.gameObject.layer)) != 0)
            {
                _isTraveling = false;
                GlueAndDestroy(other.gameObject);
            }
        }

        private void GlueAndDestroy(GameObject otherGameObject)
        {
            _collider.enabled = false;
            gameObject.transform.parent = otherGameObject.transform;

            if (_lightDimCoroutine != null) StopCoroutine(_lightDimCoroutine);
            _lightDimCoroutine = StartCoroutine(LightDim());

            if (_arrowDestroyCoroutine != null) StopCoroutine(_arrowDestroyCoroutine);
            _arrowDestroyCoroutine = StartCoroutine(WaitAndDestroy());
        }

        private IEnumerator WaitAndDestroy()
        {
            yield return new WaitForSeconds(properties.destroySeconds);
            Destroy(gameObject);
        }

        private IEnumerator LightDim()
        {
            float timer = 0;
            float startTime = Time.time;
            float initialIntensity = pointLight.intensity;
            while (timer < dimDuration)
            {
                timer = Time.time - startTime;
                pointLight.intensity = Mathf.Lerp(initialIntensity, 0, timer / dimDuration);
                yield return null;
            }
        }
    }
}
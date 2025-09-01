using System.Collections;
using CameraScripts;
using Events.Scriptables;
using FSM;
using Player;
using Player.Properties;
using UnityEngine;


namespace Player.Controllers
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerRotation))]
    public class KnockBackController : Controller<PlayerAgent>
    {
        [SerializeField] private PlayerMovementProperties playerMovementProperties;
        [SerializeField] private CameraShakeProfile knockBackShake;
        [SerializeField] private ShakeProfileEventChannel onKnockBack;

        private PlayerMovement _playerMovement;
        private PlayerRotation _playerRotation;
        private Coroutine _knockBackCoroutine = null;

        private void OnEnable()
        {
            _playerMovement ??= GetComponent<PlayerMovement>();
            _playerRotation ??= GetComponent<PlayerRotation>();
        }

        public void GetKnocked()
        {
            if (_knockBackCoroutine != null)
                StopCoroutine(_knockBackCoroutine);

            _knockBackCoroutine = StartCoroutine(KnockBackCoroutine());
        }

        private IEnumerator KnockBackCoroutine()
        {
            onKnockBack?.RaiseEvent(knockBackShake);
            _playerRotation.LockRotation = true;
            agent.MovementChecks.WallRaycast(out int dir);
            _playerMovement.KnockBack(dir);
            agent.ChangeStateToFalling();
            yield return new WaitForSeconds(playerMovementProperties.knockBackLockDuration);
            _playerRotation.LockRotation = false;
        }

        public override void OnUpdate()
        {
        }
    }
}
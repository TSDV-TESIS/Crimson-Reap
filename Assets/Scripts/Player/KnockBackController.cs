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
    public class KnockBackController : Controller<PlayerAgent>
    {
        [SerializeField] private PlayerMovementProperties playerMovementProperties;
        [SerializeField] private CameraShakeProfile knockBackShake;
        [SerializeField] private ShakeProfileEventChannel onKnockBack;

        private PlayerMovement _playerMovement;
        private Coroutine _knockBackCoroutine = null;

        private void OnEnable()
        {
            _playerMovement ??= GetComponent<PlayerMovement>();
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
            agent.MovementChecks.WallRaycast(out int dir);
            _playerMovement.KnockBack(dir);
            Debug.Log("Bonk");
            agent.ChangeStateToFalling();
            yield return new WaitForSeconds(playerMovementProperties.knockBackLockDuration);
        }

        public override void OnUpdate()
        {
        }
    }
}
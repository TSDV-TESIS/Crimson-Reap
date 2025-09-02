using System;
using Decals;
using UnityEngine;

namespace Player
{
    public class PlayerVfxController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem walkingParticles;
        [SerializeField] private ParticleSystem wallrideParticles;
        [SerializeField] private GameObject jumpParticles;
        [SerializeField] private GameObject groundedParticles;
        [SerializeField] private float wallrideParticleAngle = 69f;
        [SerializeField] private Vector3 jumpVfxRotation;

        [Header("Pivots")] [SerializeField] private GameObject floorPivot;
        [SerializeField] private GameObject leftPivot;
        [SerializeField] private GameObject rightPivot;

        private float _lastSign;
        private Vector3 _lastWallridePosition;
        private Quaternion? _overrideJumpParticleAngle;
        private GameObject _overrideJumpParticlePosition;

        private void OnEnable()
        {
            _overrideJumpParticlePosition = null;
            _overrideJumpParticleAngle = null;
        }

        public void HandleJump()
        {
            GameObject pivotToUse = _overrideJumpParticlePosition ?? floorPivot;
            InstantiateInPosition(jumpParticles, pivotToUse, _overrideJumpParticleAngle ?? Quaternion.Euler(jumpVfxRotation));
            _overrideJumpParticlePosition = null;
            _overrideJumpParticleAngle = null;
        }

        public void HandleGrounded()
        {
            GameObject groundedVfxHandler = InstantiateInPosition(groundedParticles, floorPivot, Quaternion.Euler(jumpVfxRotation));
            groundedVfxHandler.GetComponent<DecalSpawner>().Spawn(floorPivot.transform.position, Quaternion.Euler(jumpVfxRotation));
            
            _overrideJumpParticlePosition = null;
            _overrideJumpParticleAngle = null;
        }

        public void OnWalking(float velocity)
        {
            float sign = Mathf.Sign(velocity);

            if (walkingParticles.isStopped || !Mathf.Approximately(_lastSign, sign))
            {
                walkingParticles.Stop();
                walkingParticles.transform.eulerAngles = new Vector3(0, sign > 0 ? 270 : 90, 0);
                walkingParticles.Play();
                _lastSign = sign;
            }
        }

        public void OnWalkingStop()
        {
            if (walkingParticles.isPlaying)
            {
                walkingParticles.Stop();
            }
        }

        public void OnWallrideStart(Vector3 position, int sign)
        {
            if (wallrideParticles.isStopped || position != _lastWallridePosition)
            {
                wallrideParticles.Stop();
                wallrideParticles.transform.position = position;
                wallrideParticles.transform.eulerAngles =
                    new Vector3(sign > 0 ? wallrideParticleAngle * 2 : wallrideParticleAngle, 90, 0);
                wallrideParticles.Play();
                _lastWallridePosition = position;

                _overrideJumpParticlePosition = sign > 0 ? rightPivot : leftPivot;
                _overrideJumpParticleAngle = Quaternion.Euler(0, sign > 0 ? -90 : 90, 0);
            }
        }

        public void OnWallrideStop()
        {
            if (wallrideParticles.isPlaying)
            {
                wallrideParticles.Stop();
            }
        }

        private GameObject InstantiateInPosition(GameObject particlePrefab, GameObject objectOfPosition, Quaternion rotation)
        {
            GameObject particles = Instantiate(particlePrefab);
            particles.transform.position = objectOfPosition.transform.position;
            particles.transform.rotation = rotation;

            return particles;
        }
    }
}
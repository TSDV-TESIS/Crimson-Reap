using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace Enemy
{
    enum VisionRaycastStates
    {
        NoVision = 0,
        SeeingPlayer = 1,
        Blocked = 2
    }

    public class VisionHandler : MonoBehaviour
    {
        [SerializeField] private EnemyVisionProperties enemyVisionProperties;
        [SerializeField] private Transform pivot;

        private RaycastHit _playerHit;
        private bool _stillSeesPlayer = false;
        private Coroutine _stillSeeingPlayerCoroutine;

        private Dictionary<Vector3, VisionRaycastStates> _raycasts;

        public bool CanSeeObjective()
        {
            _raycasts = new Dictionary<Vector3, VisionRaycastStates>();

            RaycastHit lastHit = _playerHit;

            float angleToUseInX = -enemyVisionProperties.visionAngleInX / 2;
            float anglePerRaycastToUseInX = Mathf.Max(enemyVisionProperties.anglePerRaycastInX,
                enemyVisionProperties.minAnglePerRaycast);

            while (angleToUseInX <= enemyVisionProperties.visionAngleInX / 2)
            {
                float angleToUseInY = -enemyVisionProperties.visionAngleInY / 2;
                float anglePerRaycastToUseInY = Mathf.Max(enemyVisionProperties.anglePerRaycastInY,
                    enemyVisionProperties.minAnglePerRaycast);

                while (angleToUseInY <= enemyVisionProperties.visionAngleInY / 2)
                {
                    Vector3 raycastDirection = Quaternion.AngleAxis(angleToUseInY, pivot.right) *
                                               Quaternion.AngleAxis(angleToUseInX, pivot.up) * pivot.forward;
                    Vector3 raycastToAdd = pivot.position + raycastDirection * enemyVisionProperties.visionLength;
                    
                    _raycasts.Add(raycastToAdd, VisionRaycastStates.NoVision);

                    bool isSeeingPlayer = Physics.Raycast(pivot.position, raycastDirection, out _playerHit,
                        enemyVisionProperties.visionLength, enemyVisionProperties.whatIsObjective);

                    bool hasObstruction = Physics.Raycast(pivot.position, raycastDirection,
                        enemyVisionProperties.visionLength, enemyVisionProperties.whatIsObstruction);

                    bool isSeeingPlayerWithoutObstruction = isSeeingPlayer && !Physics.Raycast(pivot.position,
                        raycastDirection, (_playerHit.transform.position - pivot.position).magnitude,
                        enemyVisionProperties.whatIsObstruction);

                    if (isSeeingPlayerWithoutObstruction)
                    {
                        if (_stillSeeingPlayerCoroutine != null) StopCoroutine(_stillSeeingPlayerCoroutine);
                        _stillSeeingPlayerCoroutine = StartCoroutine(StillSeeingPlayerCoroutine());
                        _raycasts[raycastToAdd] = VisionRaycastStates.SeeingPlayer;
                        return true;
                    }

                    if (hasObstruction)
                    {
                        angleToUseInY += anglePerRaycastToUseInY;
                        _raycasts[raycastToAdd] = VisionRaycastStates.Blocked;
                        continue;
                    }

                    ;

                    angleToUseInY += anglePerRaycastToUseInY;
                }

                angleToUseInX += anglePerRaycastToUseInX;
            }

            if (_stillSeesPlayer)
            {
                _playerHit = lastHit;
                return true;
            }

            return false;
        }

        private IEnumerator StillSeeingPlayerCoroutine()
        {
            _stillSeesPlayer = true;
            yield return new WaitForSeconds(enemyVisionProperties.stillSeeingPlayerSeconds);
            _stillSeesPlayer = false;
        }

        public bool CanSeeObjective(out GameObject objectiveObject)
        {
            objectiveObject = null;
            if (CanSeeObjective())
            {
                objectiveObject = _playerHit.transform?.gameObject;
                return true;
            }

            return false;
        }

        private void OnDrawGizmos()
        {
            if (!enemyVisionProperties.shouldDrawGizmos || !Application.isPlaying || _raycasts == null) return;

            foreach (var (key, value) in _raycasts)
            {
                Gizmos.color = RaycastStateToColor(value);
                Gizmos.DrawLine(pivot.position, key);
            }

            /*
            Gizmos.color = Color.red;

            float angleToUse = -enemyVisionProperties.visionAngle / 2;
            float anglePerRaycastToUse = Mathf.Max(enemyVisionProperties.anglePerRaycast, enemyVisionProperties.minAnglePerRaycast);

            while (angleToUse <= enemyVisionProperties.visionAngle / 2)
            {
                Vector3 raycastDirection = Quaternion.AngleAxis(angleToUse, pivot.right) * pivot.forward;
                Gizmos.DrawLine(pivot.position, pivot.position + raycastDirection * enemyVisionProperties.visionLength);
                angleToUse += anglePerRaycastToUse;
            }
            */
        }

        private Color RaycastStateToColor(VisionRaycastStates value)
        {
            switch (value)
            {
                case VisionRaycastStates.NoVision:
                    return Color.blue;
                case VisionRaycastStates.Blocked:
                    return Color.red;
                case VisionRaycastStates.SeeingPlayer:
                    return Color.green;
                default:
                    return Color.blue;
            }
        }
    }
}
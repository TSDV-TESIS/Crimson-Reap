using System;
using System.Collections;
using Unity.Collections;
using UnityEngine;

namespace Enemy
{
    public class VisionHandler : MonoBehaviour
    {
        [SerializeField] private EnemyVisionProperties enemyVisionProperties;
        [SerializeField] private Transform pivot;

        private RaycastHit _playerHit;
        private bool _stillSeesPlayer = false;
        private Coroutine _stillSeeingPlayerCoroutine;
        
        public bool CanSeeObjective()
        {
            RaycastHit lastHit = _playerHit;
            
            float angleToUse = -enemyVisionProperties.visionAngle / 2;
            float anglePerRaycastToUse = Mathf.Max(enemyVisionProperties.anglePerRaycast, enemyVisionProperties.minAnglePerRaycast);
            
            while (angleToUse <= enemyVisionProperties.visionAngle / 2)
            {
                Vector3 raycastDirection = Quaternion.AngleAxis(angleToUse, pivot.right) * pivot.forward;
                if (Physics.Raycast(pivot.position, raycastDirection, enemyVisionProperties.visionLength, enemyVisionProperties.whatIsObstruction))
                {
                    angleToUse += anglePerRaycastToUse;
                    continue;
                };
                if (Physics.Raycast(pivot.position, raycastDirection, out _playerHit, enemyVisionProperties.visionLength, enemyVisionProperties.whatIsObjective))
                {
                    Debug.Log("SEEING PLAYER FROM RAYCAST!");
                    if(_stillSeeingPlayerCoroutine != null) StopCoroutine(_stillSeeingPlayerCoroutine);
                    _stillSeeingPlayerCoroutine = StartCoroutine(StillSeeingPlayerCoroutine());
                    return true;
                } 
    
                angleToUse += anglePerRaycastToUse;
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
            if (!enemyVisionProperties.shouldDrawGizmos) return;
            
            Gizmos.color = Color.red;
            
            float angleToUse = -enemyVisionProperties.visionAngle / 2;
            float anglePerRaycastToUse = Mathf.Max(enemyVisionProperties.anglePerRaycast, enemyVisionProperties.minAnglePerRaycast);

            while (angleToUse <= enemyVisionProperties.visionAngle / 2)
            {
                Vector3 raycastDirection = Quaternion.AngleAxis(angleToUse, pivot.right) * pivot.forward;
                Gizmos.DrawLine(pivot.position, pivot.position + raycastDirection * enemyVisionProperties.visionLength);
                angleToUse += anglePerRaycastToUse;
            }
        }
    }
}

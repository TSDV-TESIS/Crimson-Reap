using System;
using System.Collections;
using Events;
using Events.Scriptables;
using Objects;
using Player.Properties;
using UnityEngine;

namespace Player.Checks
{
    public class PlayerMovementChecks : MonoBehaviour
    {
        [Header("Input handler")] 
        [SerializeField] private InputHandler inputHandler;
        
        [Header("Movement Properties")] 
        [SerializeField] private PlayerMovementProperties playerMovementProperties;

        [Header("Feet pivot")] [SerializeField]
        private Transform feetPivot;

        [Header("Head pivot")] [SerializeField]
        private Transform headPivot;
        [Header("WallRide pivot")] [SerializeField]
        private Transform wallRidePivot;

        [Header("Events")] 
        [SerializeField] private FloatEventChannel onShadowstepCooldownValueEvent;
        [SerializeField] private VoidEventChannelSO onDoingDropdown;
        [SerializeField] private VoidEventChannelSO onStopDropdown;
        
        [NonSerialized] public Vector3 WallrideHitPosition;
        [NonSerialized] public bool IsOnDropdown;
        [NonSerialized] public int WallSlideDirection;
        [NonSerialized] public bool IsShadowStepOnCooldown;
        
        private RaycastHit _groundHit;
        private RaycastHit _ceilingHit;
        private RaycastHit _wallHit;

        private bool _isWallSliding;

        private bool _shouldCheckWall;
        private bool _shouldCheckCeiling;
        private bool _shouldUnboundWall;

        private float _wallRideInCoyoteSeconds;
        private bool _inWallrideCoyoteTime;

        private float _groundedCoyoteTimeSeconds;
        private float _offGroundGraceTimeSeconds;

        private Coroutine _shouldCheckWallCoroutine;
        private Coroutine _unboundWallCoroutine;
        private Coroutine _shadowstepCooldownCoroutine;
        private Coroutine _shouldCheckCeilingCoroutine;

        private int _shadowstepsOnAirLeft;
        private LayerMask _whatIsGround;
        private Coroutine _stopCheckingPlatformCooldownCoroutine;
        private bool _shouldCheckPlatforms;

        private void OnEnable()
        {
            _shouldCheckCeiling = true;
            _shouldCheckWall = true;
            _shouldUnboundWall = false;
            _inWallrideCoyoteTime = false;
            _shouldCheckPlatforms = true;

            _whatIsGround = playerMovementProperties.whatIsGround;
            onDoingDropdown.onEvent.AddListener(HandleDropdownOn);
            onStopDropdown.onEvent.AddListener(HandleDropdownOff);
            ResetShadowStepsOnAir();
        }

        private void OnDisable()
        {
            onDoingDropdown.onEvent.RemoveListener(HandleDropdownOn);
            onStopDropdown.onEvent.RemoveListener(HandleDropdownOff);
        }

        private void Update()
        {
            CheckDropdown();
        }

        private void HandleDropdownOff()
        {
            IsOnDropdown = false;
        }

        private void HandleDropdownOn()
        {
            IsOnDropdown = true;
        }

        public bool CanShadowStepOnAir()
        {
            return _shadowstepsOnAirLeft > 0;
        }

        public void SetShadowStepOnAirUsed()
        {
            _shadowstepsOnAirLeft--;
        }

        public void ResetShadowStepsOnAir()
        {
            _shadowstepsOnAirLeft = playerMovementProperties.maxShadowStepsOnAir;
        }

        public bool IsGrounded()
        {
            if (IsOnRaycastGround())
            {
                if (_groundHit.transform.TryGetComponent(out IOpenable openable))
                    openable.Open();

                _groundedCoyoteTimeSeconds = 0f;
                _offGroundGraceTimeSeconds = 0f;
                return true;
            }

            return false;
        }

        public bool IsInOffGroundGraceTime()
        {
            _offGroundGraceTimeSeconds += Time.deltaTime;

            return _offGroundGraceTimeSeconds < playerMovementProperties.offGroundMaxGraceTimeSeconds;
        }

        public bool IsInGroundedCoyoteTime()
        {
            _groundedCoyoteTimeSeconds += Time.deltaTime;

            return _groundedCoyoteTimeSeconds < playerMovementProperties.coyoteTimeSeconds;
        }

        public bool IsOnRaycastGround()
        {
            return Physics.Raycast(feetPivot.position, Vector3.down, out _groundHit,
            playerMovementProperties.checkDistance, _whatIsGround);
        }

        public bool IsOnPlatform()
        {
            return Physics.Raycast(feetPivot.position, Vector3.down, out _groundHit,
            playerMovementProperties.checkDistance, playerMovementProperties.whatIsPlatform);
        }

        public bool IsFalling(Vector3 moveDirection)
        {
            return moveDirection.y < 0;
        }

        private void StopUnbounding()
        {
            if (_unboundWallCoroutine != null)
                StopCoroutine(_unboundWallCoroutine);
            _unboundWallCoroutine = null;
        }

        public bool ShouldUnboundWallslide(Vector3 moveDirection, Vector2 movementVelocity)
        {
            return ShouldUnboundByInput(moveDirection) || ShouldUnboundByCoyote(movementVelocity);
        }

        private bool ShouldUnboundByCoyote(Vector2 movementVelocity)
        {
            if (!WallRaycast(WallSlideDirection))
            {
                if (movementVelocity.y > 0)
                {
                    _wallRideInCoyoteSeconds = 0;
                    _inWallrideCoyoteTime = false;
                    return false;
                }

                if (!_inWallrideCoyoteTime)
                {
                    _inWallrideCoyoteTime = true;
                    _wallRideInCoyoteSeconds = 0;
                }

                _wallRideInCoyoteSeconds += Time.deltaTime;

                if (_wallRideInCoyoteSeconds > playerMovementProperties.wallRideMaxCoyoteSeconds)
                {
                    _wallRideInCoyoteSeconds = 0;
                    _inWallrideCoyoteTime = false;
                    return true;
                }

                return false;
            }

            _inWallrideCoyoteTime = false;
            _wallRideInCoyoteSeconds = 0;
            return false;
        }

        private bool ShouldUnboundByInput(Vector3 moveDirection)
        {
            if (!_shouldCheckWall)
            {
                StopUnbounding();
                _shouldUnboundWall = false;
                return true;
            }
            
            if (Mathf.Sign(moveDirection.x) == Mathf.Sign(WallSlideDirection))
            {
                StopUnbounding();
                return false;
            }

            if (_shouldUnboundWall)
            {
                _shouldUnboundWall = false;
                _isWallSliding = false;
                StopUnbounding();
                return true;
            }

            _unboundWallCoroutine ??= StartCoroutine(UnboundWallCoroutine());
            return false;
        }

        private IEnumerator UnboundWallCoroutine()
        {
            Debug.Log($"UNBOUND COROUTINE CALLED");
            yield return new WaitForSeconds(playerMovementProperties.unboundTime);
            _shouldUnboundWall = true;
        }

        public bool ShouldWallSlide(Vector3 moveDirection, Vector2 velocity)
        {
            if (!_shouldCheckWall) return false;

            int signToCheck = Math.Sign(Math.Abs(velocity.x) > playerMovementProperties.wallVelocityCheck
                ? velocity.x
                : moveDirection.x);

            _isWallSliding = WallRaycast(signToCheck);

            WallSlideDirection = _isWallSliding ? signToCheck : 0;
            return _isWallSliding;
        }

        public bool IsNearCeiling()
        {
            if (_shouldCheckCeiling && Physics.Raycast(headPivot.position, Vector3.up, out _ceilingHit,
                playerMovementProperties.checkDistance, playerMovementProperties.whatIsCeiling))
            {
                Debug.Log($"Normal: {_ceilingHit.normal} is equal to V3.Down {_ceilingHit.normal == Vector3.down}");

                if (_shouldCheckCeilingCoroutine != null) StopCoroutine(_shouldCheckCeilingCoroutine);
                _shouldCheckCeilingCoroutine = StartCoroutine(ShouldCheckCeilingCoroutine());
                return _ceilingHit.normal == Vector3.down;
            }

            return false;
        }

        private IEnumerator ShouldCheckCeilingCoroutine()
        {
            _shouldCheckCeiling = false;
            yield return new WaitForSeconds(playerMovementProperties.ceilingCheckWaitTime);
            _shouldCheckCeiling = true;
        }

        public bool IsNearCorner(out float cornerDisplace)
        {
            Vector3 displacement = Vector3.right * playerMovementProperties.cornerCorrectionMaxDistance;
            if (Physics.Raycast(headPivot.position - displacement, Vector3.up, out RaycastHit leftCornerHit,
                playerMovementProperties.checkDistance) ^
                Physics.Raycast(headPivot.position + displacement, Vector3.up, out RaycastHit rightCornerHit,
                playerMovementProperties.checkDistance))
            {
                if (leftCornerHit.normal == Vector3.down)
                {
                    cornerDisplace = transform.position.x - leftCornerHit.point.x;
                    return true;
                }

                if (rightCornerHit.normal == Vector3.down)
                {
                    cornerDisplace = transform.position.x - rightCornerHit.point.x;
                    return true;
                }
            }

            cornerDisplace = 0;
            return false;
        }

        public bool WallRaycast(int signToCheck)
        {
            bool hasRaycast = Physics.Raycast(wallRidePivot.position, Vector3.right * signToCheck, out _wallHit,
            playerMovementProperties.wallCheckDistance,
            playerMovementProperties.whatIsWall);

            if (hasRaycast)
            {
                WallrideHitPosition = wallRidePivot.position + Vector3.right * signToCheck;
            }

            return hasRaycast;
        }

        public bool WallRaycast(out int signThatHits)
        {
            if (WallRaycast(-1))
            {
                signThatHits = -1;
                return true;
            }

            if (WallRaycast(1))
            {
                signThatHits = 1;
                return true;
            }

            signThatHits = 0;
            return false;
        }

        public bool IsOnSlope()
        {
            if (!IsGrounded()) return false;

            float angle = Vector3.Angle(Vector3.up, _groundHit.normal);

            return angle < playerMovementProperties.maxSlopeAngle && !Mathf.Approximately(angle, 0);
        }

        public Vector3 GetSlopeMovementDirection(Vector3 moveDirection)
        {
            return Vector3.ProjectOnPlane(moveDirection, _groundHit.normal).normalized;
        }

        public float GetGroundClearance()
        {
            if (!IsGrounded()) return 0;
            float groundY = GetGroundHitPoint().y;
            return Mathf.Abs(feetPivot.localPosition.y) + groundY + playerMovementProperties.feetOffset;
        }

        public Vector3 GetGroundHitPoint()
        {
            IsGrounded();
            return _groundHit.point;
        }

        public void StopCheckingWall()
        {
            if (_shouldCheckWallCoroutine != null) StopCoroutine(_shouldCheckWallCoroutine);
            StopUnbounding();
            _shouldCheckWallCoroutine = StartCoroutine(HandleStopCheckWall());
        }

        private IEnumerator HandleStopCheckWall()
        {
            _shouldCheckWall = false;
            _isWallSliding = false;
            yield return new WaitForSeconds(playerMovementProperties.stopCheckWallSeconds);
            _shouldCheckWall = true;
        }

        private void OnDrawGizmos()
        {
            if (playerMovementProperties.shouldDrawGizmos)
            {
                // Draw feet raycast
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(feetPivot.position,
                feetPivot.position + Vector3.down * playerMovementProperties.checkDistance);
                Gizmos.DrawLine(headPivot.position,
                headPivot.position + Vector3.up * playerMovementProperties.checkDistance);

                // Wall raycast
                Gizmos.color = Color.green;
                Gizmos.DrawLine(feetPivot.position,
                feetPivot.position + Vector3.right * playerMovementProperties.wallCheckDistance);
                Gizmos.DrawLine(feetPivot.position,
                feetPivot.position + Vector3.left * playerMovementProperties.wallCheckDistance);
            }
        }

        public bool IsNearWall()
        {
            return WallRaycast(out WallSlideDirection);
        }

        public void SetShadowstepOnCooldown()
        {
            if (_shadowstepCooldownCoroutine != null) StopCoroutine(_shadowstepCooldownCoroutine);
            _shadowstepCooldownCoroutine = StartCoroutine(ShadowStepOnCooldown());
        }

        private IEnumerator ShadowStepOnCooldown()
        {
            float timer = 0;
            IsShadowStepOnCooldown = true;

            while (timer < playerMovementProperties.shadowStepCooldown)
            {
                onShadowstepCooldownValueEvent?.RaiseEvent(
                (float)(timer / playerMovementProperties.shadowStepCooldown));
                timer += Time.deltaTime;
                yield return null;
            }

            onShadowstepCooldownValueEvent?.RaiseEvent(1);
            IsShadowStepOnCooldown = false;
        }

        public bool IsNearNonAvoidableWall()
        {
            bool test = WallRaycast(out WallSlideDirection);
            if (test)
            {
                Debug.Log($"CHECKING IF WALL: ${(playerMovementProperties.avoidableObjects & (1 << _wallHit.transform.gameObject.layer)) == 0}");
            }

            return WallRaycast(out WallSlideDirection) &&
                   (playerMovementProperties.avoidableObjects & (1 << _wallHit.transform.gameObject.layer)) == 0;
        }
        
        private void CheckDropdown()
        {
            if (IsOnDropdown)
            {
                StopCheckingPlatforms();
            }
            else if (_shouldCheckPlatforms)
            {
                RestartCheckingPlatforms();
            }
        }

        public void StopCheckingPlatforms()
        {
            _whatIsGround &= ~playerMovementProperties.whatIsPlatform;

            if (IsOnPlatform())
            {
                if(_stopCheckingPlatformCooldownCoroutine != null) StopCoroutine(_stopCheckingPlatformCooldownCoroutine);
                _stopCheckingPlatformCooldownCoroutine = StartCoroutine(StopCheckingPlatformsCooldownCoroutine());   
            }
        }

        private IEnumerator StopCheckingPlatformsCooldownCoroutine()
        {
            _shouldCheckPlatforms = false;
            yield return new WaitForSeconds(playerMovementProperties.stopCheckingPlatformSeconds);
            _shouldCheckPlatforms = true;
        }

        public void RestartCheckingPlatforms()
        {
            _whatIsGround |= playerMovementProperties.whatIsPlatform;
        }

        public void ClearUnbound()
        {
            StopUnbounding();
            _shouldUnboundWall = false;
            _isWallSliding = false;
        }
    }
}
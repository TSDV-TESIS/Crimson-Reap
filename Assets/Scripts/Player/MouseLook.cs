using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Player
{
    public class MouseLook : MonoBehaviour
    {
        [SerializeField] private InputHandler handler;
        [SerializeField] private GameObject visorPivot;
        [SerializeField] private PlayerLookProperties lookProperties;
        
        private float _angle;

        private Vector2 _viewPortPos;
        private Vector2 _cursorDir;
        public Vector2 CursorDir => _cursorDir.normalized;
        void OnEnable()
        {
            handler.OnPlayerLook.AddListener(HandleLookDir);
        }

        private void OnDisable()
        {
            handler.OnPlayerLook.RemoveListener(HandleLookDir);
        }

        private void HandleLookDir(Vector2 cursorPos)
        {
            Vector3 worldDistance =
                Camera.main.ScreenToWorldPoint(new Vector3(cursorPos.x, cursorPos.y,
                    -Camera.main.transform.position.z)) - transform.position;

            if (worldDistance.magnitude < lookProperties.deadZoneRadius) return;
            
            _viewPortPos = Camera.main.ScreenToViewportPoint(cursorPos);
            Vector2 playerPosOnViewport = Camera.main.WorldToViewportPoint(transform.position);
            
            _cursorDir = _viewPortPos - new Vector2(playerPosOnViewport.x, playerPosOnViewport.y);
            
            _angle = Mathf.Atan2(_cursorDir.x, _cursorDir.y) * Mathf.Rad2Deg;
            visorPivot.transform.rotation = Quaternion.AngleAxis(-_angle + 90, Vector3.forward) * transform.rotation;
        }

        private void OnDrawGizmos()
        {
            if (lookProperties.showDeadzone)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, lookProperties.deadZoneRadius);
            }
        }
    }
}

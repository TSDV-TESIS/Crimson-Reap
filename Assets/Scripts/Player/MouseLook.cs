using Player.Attacks;
using UnityEngine;

namespace Player
{
    public class MouseLook : MonoBehaviour
    {
        [SerializeField] private InputHandler handler;
        [SerializeField] private bool is2D;
        [SerializeField] private GameObject visorPivot;

        private float _angle;

        private Vector2 _viewPortPos;
        private Vector2 cursorDir;
        public Vector2 CursorDir => cursorDir.normalized;

        private AttackPivot _attackPivot;        
        void OnEnable()
        {
            handler.OnPlayerLook.AddListener(HandleLookDir);
            _attackPivot ??= visorPivot.GetComponent<AttackPivot>();
        }

        private void OnDisable()
        {
            handler.OnPlayerLook.RemoveListener(HandleLookDir);
        }

        private void HandleLookDir(Vector2 cursorPos)
        {
            _viewPortPos = Camera.main.ScreenToViewportPoint(cursorPos);
            Vector2 playerPosOnViewport = Camera.main.WorldToViewportPoint(transform.position);
            cursorDir = _viewPortPos - new Vector2(playerPosOnViewport.x, playerPosOnViewport.y);

            _angle = Mathf.Atan2(cursorDir.x, cursorDir.y) * Mathf.Rad2Deg;
            if (is2D)
            {
                _attackPivot.RotateByPivot(Quaternion.AngleAxis(-_angle + 90, Vector3.forward) * transform.rotation);
                return;
            }

            transform.rotation = Quaternion.AngleAxis(_angle, Vector3.up);
        }
    }
}

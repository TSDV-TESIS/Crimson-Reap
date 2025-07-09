using System;
using UnityEngine;

namespace Player.Attacks
{
    public class AttackPivot : MonoBehaviour
    {
        private bool _canRotate;

        private void OnEnable()
        {
            _canRotate = true;
        }

        public void RotateByPivot(Quaternion rotation)
        {
            if(_canRotate) transform.rotation = rotation;
        }

        public void SetCanRotateFlag(bool value)
        {
            _canRotate = value;
        }
    }
}

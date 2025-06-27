using System;
using System.Collections;
using System.Collections.Generic;
using Objects.Traps.Saw;
using UnityEngine;

namespace Objects.Traps
{
    public class SawTrap : BaseTrap
    {
        [SerializeField] private SawProperties _properties;
        [SerializeField] private GameObject saw;
        [SerializeField] private GameObject model;
        [SerializeField] private GameObject WaypointsParent;

        private List<Vector3> _waypoints = new List<Vector3>();
        private int _targetIndex = 0;
        private bool _isAscending = true;

        private float sawRotationSpeed = 100000;

        private void Start()
        {
            GetWaypoints();
            if (_waypoints.Count > 0)
                saw.transform.position = _waypoints[0];
        }

        private void Update()
        {
            SpinSawAnim();
            
            if (_waypoints.Count <= 1)
                return;

            CheckNearTarget();
            MoveTowardsWaypoint();
        }

        private void MoveTowardsWaypoint()
        {
            Vector3 dir = _waypoints[_targetIndex] - saw.transform.position;
            saw.transform.Translate(dir.normalized * (_properties.speed * Time.deltaTime));
        }

        private void CheckNearTarget()
        {
            float distance = Vector3.Distance(saw.transform.position, _waypoints[_targetIndex]);
            if (distance < _properties.distError)
            {
                GetNextWaypoint();
                Debug.Log($"Next Waypoint is {_targetIndex}");
            }
        }

        private void GetNextWaypoint()
        {
            if ((_isAscending && _targetIndex + 1 >= _waypoints.Count) || (!_isAscending && _targetIndex - 1 < 0))
            {
                if (!_properties.shouldLoopBounce)
                {
                    _targetIndex = 0;
                    return;
                }

                _isAscending = !_isAscending;
            }

            if (_isAscending)
                _targetIndex++;
            else
                _targetIndex--;
        }

        private void GetWaypoints()
        {
            for (int i = 0; i < WaypointsParent.transform.childCount; i++)
            {
                _waypoints.Add(WaypointsParent.transform.GetChild(i).position);
            }
        }

        //Should have an animation
        private void SpinSawAnim()
        {
            model.transform.Rotate(Vector3.forward, sawRotationSpeed * Time.deltaTime);
        }
    }
}
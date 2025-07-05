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
        private bool _shouldMove = true;
        private float sawRotationSpeed = 100000;

        private Coroutine _pauseAtCoroutine;

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

            if (_shouldMove)
            {
                CheckNearTarget();
                MoveTowardsWaypoint();
            }
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
                Stop(_properties.timeAtLastWaypoint);
                if (!_properties.shouldLoopBounce)
                {
                    _targetIndex = 0;
                    return;
                }

                _isAscending = !_isAscending;
            }
            else
                Stop(_properties.timeAtWaypoint);

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

        private void Stop(float duration)
        {
            if (_pauseAtCoroutine != null)
                StopCoroutine(_pauseAtCoroutine);

            _pauseAtCoroutine = StartCoroutine(StopAt(duration));
        }

        private IEnumerator StopAt(float duration)
        {
            _shouldMove = false;
            yield return new WaitForSeconds(duration);
            _shouldMove = true;
        }
    }
}
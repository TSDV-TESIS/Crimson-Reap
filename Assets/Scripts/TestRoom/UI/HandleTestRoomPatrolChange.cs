using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TestRoom.UI
{
    public class HandleTestRoomPatrolChange : MonoBehaviour
    {
        private const string PatrolString = "patrol";
        private const string OnGuardString = "onGuard";
        private const string ScanningString = "scanning";

        [SerializeField] private GameObject patrolText;
        [SerializeField] private GameObject chaseText;
        [SerializeField] private GameObject scanningText;
        [SerializeField] private UnityEvent<String> onNewSelectedPatrol;

        private List<string> _stringsToRotate = new List<string>() { PatrolString, OnGuardString, ScanningString };
        private int _index = 0;

        public void Awake()
        {
            ResetWithActive(patrolText);
        }

        public void OnInteract(bool value)
        {
            _index++;
            if (_index >= _stringsToRotate.Count) _index = 0;

            switch (_stringsToRotate[_index])
            {
                case PatrolString:
                    ResetWithActive(patrolText);
                    break;
                case OnGuardString:
                    ResetWithActive(chaseText);
                    break;
                case ScanningString:
                    ResetWithActive(scanningText);
                    break;
            }

            onNewSelectedPatrol?.Invoke(_stringsToRotate[_index]);
        }

        private void ResetWithActive(GameObject objectToActivate)
        {
            patrolText.SetActive(false);
            chaseText.SetActive(false);
            scanningText.SetActive(false);

            objectToActivate.SetActive(true);
        }
    }
}
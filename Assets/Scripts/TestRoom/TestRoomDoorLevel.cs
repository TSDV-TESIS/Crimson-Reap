using System;
using Unity.Behavior;
using Unity.Mathematics;
using UnityEngine;

namespace TestRoom
{
    public class TestRoomDoorLevel : MonoBehaviour
    {
        private const string EnemyIdleTypeParameterName = "EnemyIdleType";
        private const string StartingDirectionParameterName = "StartingDirection";

        [SerializeField] private Transform nearPosition;
        [SerializeField] private Transform farPosition;
        [SerializeField] private Transform doorPosition;
        [SerializeField] private GameObject enemyKnightPrefab;
        [SerializeField] private GameObject doorPrefab;

        private Transform _selectedPosition;
        private GameObject _actualEnemyInstance;
        private GameObject _actualDoorInstance;

        public void OnEnable()
        {
            _selectedPosition = farPosition;
            Reset();
        }

        public void SetNewPosition(bool isFar)
        {
            _selectedPosition = isFar ? farPosition : nearPosition;
        }

        public void Reset()
        {
            SpawnEnemy();
            ResetDoor();
        }

        private void ResetDoor()
        {
            if (_actualDoorInstance != null) Destroy(_actualDoorInstance);

            _actualDoorInstance = Instantiate(doorPrefab);
            _actualDoorInstance.transform.position = doorPosition.position;
        }

        [ContextMenu("SpawnEnemy")]
        public void SpawnEnemy()
        {
            if (_actualEnemyInstance != null) Destroy(_actualEnemyInstance);

            _actualEnemyInstance = Instantiate(enemyKnightPrefab, _selectedPosition.position, quaternion.identity);

            BehaviorGraphAgent behaviorGraphAgent = _actualEnemyInstance.GetComponent<BehaviorGraphAgent>();
            behaviorGraphAgent.BlackboardReference.SetVariableValue(EnemyIdleTypeParameterName, IdleType.OnGuard);
            behaviorGraphAgent.BlackboardReference.SetVariableValue(StartingDirectionParameterName, Direction.Right);
        }
    }
}
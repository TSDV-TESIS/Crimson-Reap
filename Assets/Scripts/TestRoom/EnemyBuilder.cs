using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

public class EnemyBuilder : MonoBehaviour
{
    private const string PatrolString = "patrol";
    private const string OnGuardString = "onGuard";
    private const string ScanningString = "scanning";
    private const string EnemyIdleTypeParameterName = "EnemyIdleType";
    private const string PatrolPointsParameterName = "PatrolPoints";

    [SerializeField] private GameObject knightPrefab;
    [SerializeField] private GameObject archerPrefab;
    [SerializeField] private Transform initPosition;
    [SerializeField] private List<GameObject> patrolPoints;

    private string _patrolSelected;
    private bool _isArrowAttack;
    private List<GameObject> _spawnedEnemies = new List<GameObject>();

    public void OnPatrolChange(string patrolType)
    {
        _patrolSelected = patrolType;
    }

    public void OnAttackTypeChange(bool isArrow)
    {
        _isArrowAttack = isArrow;
    }

    public void Spawn()
    {
        GameObject instance = Instantiate(_isArrowAttack ? archerPrefab : knightPrefab, transform);
        instance.transform.position = initPosition.position;

        BehaviorGraphAgent behaviorGraphAgent = instance.GetComponent<BehaviorGraphAgent>();
        behaviorGraphAgent.BlackboardReference.SetVariableValue(EnemyIdleTypeParameterName, GetIdleType());
        behaviorGraphAgent.BlackboardReference.SetVariableValue(PatrolPointsParameterName, patrolPoints);

        _spawnedEnemies.Add(instance);
    }

    private IdleType GetIdleType()
    {
        switch (_patrolSelected)
        {
            case PatrolString:
                return IdleType.Patrol;
            case OnGuardString:
                return IdleType.OnGuard;
            case ScanningString:
                return IdleType.Scanning;
            default:
                return IdleType.Patrol;
        }
    }
}
using System;
using Enemy.Attack;
using Enemy.Properties;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Behaviours
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Set Enemy General Blackboard Variables", story: "Set Enemy Blackboard Variables", category: "Action", id: "5c76a840a604c3fcd214922d8a1d0231")]
    public partial class SetEnemyGeneralBlackboardVariablesAction : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Self;
        [SerializeReference] public BlackboardVariable<EnemyGeneralProperties> EnemyProperties;
        [SerializeReference] public BlackboardVariable<ArrowAttackProperties> ArrowProperties;
        
        private BehaviorGraphAgent _agent;
        
        private readonly String _attackDistanceParameterName = "AttackDistance";
        private readonly String _minRotationTimeParameterName = "MinRotationTime";
        private readonly String _maxRotationTimeParameterName = "MaxRotationTime";
        private readonly String _investigationWaitTimeSecondsParameterName = "InvestigationWaitTimeSeconds";
        private readonly String _patrolPointWaitTimeParameterName = "PatrolPointWaitTime";
        private readonly String _attackNoiseLevelParameterName = "AttackNoiseLevel";
        private readonly String _attackDurationParameterName = "AttackCooldown";
        private readonly String _attackStartTimeParameterName = "AttackStartTime";
        private readonly String _attackIframesDurationParameterName = "AttackIframesDuration";
        private readonly String _arrowVelocityParameterName = "ArrowVelocity";
        
        protected override Status OnStart()
        {
            _agent ??= Self.Value.GetComponent<BehaviorGraphAgent>();
            EnemyGeneralProperties properties = EnemyProperties.Value;
            
            if (
                !(
                    _agent.GetVariable(_attackDistanceParameterName, out BlackboardVariable attackDistance) &&
                    
                    _agent.GetVariable(_minRotationTimeParameterName, out BlackboardVariable minRotationTime) &&
                    _agent.GetVariable(_maxRotationTimeParameterName, out BlackboardVariable maxRotationTime) &&
                    _agent.GetVariable(_investigationWaitTimeSecondsParameterName,
                        out BlackboardVariable investigationSeconds) &&
                    _agent.GetVariable(_patrolPointWaitTimeParameterName, out BlackboardVariable patrolPointWaitTime) &&
                    _agent.GetVariable(_attackNoiseLevelParameterName, out BlackboardVariable attackNoiseLevel) &&
                    _agent.GetVariable(_attackDurationParameterName, out BlackboardVariable attackDuration) &&
                    _agent.GetVariable(_attackStartTimeParameterName, out BlackboardVariable attackStartTime) &&
                    _agent.GetVariable(_attackIframesDurationParameterName, out BlackboardVariable attackIframes) &&
                    _agent.GetVariable(_arrowVelocityParameterName, out BlackboardVariable arrowVelocity)
                )
            )
            {
                Debug.LogError("Not all variables were obtained correctly.");

                return Status.Failure;
            }

            attackDistance.ObjectValue = properties.attackRange;
            minRotationTime.ObjectValue = properties.minRotationTime;
            maxRotationTime.ObjectValue = properties.maxRotationTime;
            investigationSeconds.ObjectValue = properties.investigationTime;
            patrolPointWaitTime.ObjectValue = properties.patrolPointWaitTimeSeconds;
            attackNoiseLevel.ObjectValue = properties.attackNoiseLevel;
            attackDuration.ObjectValue = properties.attackCooldown;
            attackStartTime.ObjectValue = properties.attackStartTime;
            attackIframes.ObjectValue = properties.attackIframesDuration;
            arrowVelocity.ObjectValue = ArrowProperties.Value.velocity;
            
            return Status.Success;
        }

        protected override Status OnUpdate()
        {
            return Status.Success;
        }

        protected override void OnEnd()
        {
        }
    }
}


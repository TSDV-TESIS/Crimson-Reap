using System;
using Enemy.Properties;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using Action = Unity.Behavior.Action;
using NavMeshData = Enemy.Properties.NavMeshData;

namespace Behaviours
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Modify navmesh", story: "Set navmesh agent values", category: "Action", id: "6dbd05c96b5921fa3ffc40b9a4c2706d")]
    public partial class ModifyNavmeshAction : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Self;
        [SerializeReference] public BlackboardVariable<VelocityType> Type; 
        [SerializeReference] public BlackboardVariable<EnemyGeneralProperties> EnemyGeneralProperties;
        [SerializeReference] public BlackboardVariable<float> SpeedToUse;
        [SerializeReference] public BlackboardVariable<float> SlowdownDistance;
        private NavMeshAgent _agent;
        
        protected override Status OnStart()
        {
            _agent ??= Self.Value.GetComponent<NavMeshAgent>();
            EnemyGeneralProperties properties = EnemyGeneralProperties.Value;
            
            switch (Type.Value)
            {
                case VelocityType.Patrol:
                    SetData(properties.patrolNavmeshData);
                    break;
                case VelocityType.Chase:
                    SetData(properties.chaseNavmeshData);
                    break;
                case VelocityType.Suspicious:
                    SetData(properties.suspiciousNavmeshData);
                    break;
            }
            
            return Status.Success;
        }
    
            protected override Status OnUpdate()
            {
                return Status.Success;
            }
    
            protected override void OnEnd()
            {
            }

            private void SetData(NavMeshData data)
            {
                _agent.speed = data.MaxSpeed;
                SpeedToUse.Value = data.MaxSpeed;
                _agent.acceleration = data.Acceleration;
                _agent.angularSpeed = data.AngularVelocity;
                SlowdownDistance.Value = data.SlowdownDistance;
            }
        }
}


using System;
using Enemy;
using Enemy.Properties;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Check if Self can hear the scream", story: "Check if [self] can hear the [scream]", category: "Action", id: "8619b77e9c2847ab2f1eb7461fb90061")]
public partial class CheckIfSelfCanHearTheScreamAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Scream;
    [SerializeReference] public BlackboardVariable<EnemyGeneralProperties> enemyProperties;
    
    protected override Status OnStart()
    {
        if (!Self.Value || !Scream.Value || !enemyProperties.Value) return Status.Failure;
        if (Physics.Linecast(Self.Value.transform.position, Scream.Value.transform.position, enemyProperties.Value.hearingOcclussionMask))
            return Status.Failure;
        
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


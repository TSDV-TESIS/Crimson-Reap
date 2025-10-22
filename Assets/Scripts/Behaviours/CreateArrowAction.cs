using System;
using Enemy.Attack;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Object = UnityEngine.Object;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Create Arrow", story: "Create new Arrow and set variables", category: "Action", id: "01194426f65f927733f15e2b599cd429")]
public partial class CreateArrowAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<GameObject> Arrow;
    [SerializeReference] public BlackboardVariable<float> Velocity;
    
    protected override Status OnStart()
    {
        Arrow.Value.GetComponent<ArrowAttack>().SetVelocityDirectionAndAttack(Velocity, Player.Value.transform.position);
        
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


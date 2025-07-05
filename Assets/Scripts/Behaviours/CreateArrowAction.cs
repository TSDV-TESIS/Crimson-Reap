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
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<GameObject> ArrowPrefab;
    [SerializeReference] public BlackboardVariable<float> Velocity;
    [SerializeReference] public BlackboardVariable<Vector3> offset;
    
    protected override Status OnStart()
    {
        Debug.Log($"TEST {ArrowPrefab.Value} {Self.Value}");
        GameObject arrowObject = Object.Instantiate(ArrowPrefab.Value, Self.Value.transform);
        arrowObject.transform.position = Self.Value.transform.position + offset;
        arrowObject.GetComponent<ArrowAttack>().SetVelocityAndDirection(Velocity, Player.Value.transform.position);
        
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


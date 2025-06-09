using System;
using Enemy;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Attack Player with Sword", story: "[Self] attacks with sword", category: "Action", id: "4bbdb2eec813e968badac45cb804ad9f")]
public partial class AttackPlayerWithSwordAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    private EnemyAttackController _attackController;
    
    protected override Status OnStart()
    {
        _attackController ??= Self.Value.GetComponent<EnemyAttackController>();
        _attackController.StartAttack();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return _attackController.Attack();
    }

    protected override void OnEnd()
    {
    }
}


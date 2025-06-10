using System;
using Enemy;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Self can hear the Scream", story: "[Self] can hear the [scream]", category: "Conditions",
    id: "7ff4965c6448deb6237f65a488a7f606")]
public partial class SelfCanHearTheScreamCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Scream;
    private BehaviorGraphAgent _agent;

    private LayerMask? _screamingOcclussionMask;
    public override bool IsTrue()
    {
        return Self.Value && Scream.Value && _screamingOcclussionMask != null && !Physics.Linecast(
            Self.Value.transform.position, Scream.Value.transform.position,
            _screamingOcclussionMask.Value);
    }

    public override void OnStart()
    {
        _agent ??= Self.Value.GetComponent<BehaviorGraphAgent>();
        _agent.GetVariable("EnemyDeathProperties", out BlackboardVariable enemyDeathProperties);
        if (enemyDeathProperties?.ObjectValue == null) return;

        _screamingOcclussionMask = ((EnemyDeathProperties)enemyDeathProperties.ObjectValue).screamingOcclussionMask;
    }

    public override void OnEnd()
    {
    }
}
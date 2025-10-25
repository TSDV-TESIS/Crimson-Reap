using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set animation rotation To Player", story: "Point [SelfAnimator] rotation To [Player]", category: "Action", id: "a455eaa22a3da3ce1574d7e1e4506c3b")]
public partial class PointSelfAnimationToPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<Animator> SelfAnimator;
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<Vector3> PivotOffset;
    private static readonly int AttackAnglePercentage = Animator.StringToHash("AttackAnglePercentage");

    private float _maxAngle;
    
    protected override Status OnStart()
    {
        _maxAngle = 30f;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        float angle = Vector3.SignedAngle(Self.Value.transform.forward + PivotOffset.Value,
            Player.Value.transform.position - Self.Value.transform.position, -Self.Value.transform.right);
        float clampedAngle = Mathf.Clamp(angle + _maxAngle, 0, _maxAngle * 2f);
        float percentage = clampedAngle / (_maxAngle * 2f);
        SelfAnimator.Value.SetFloat(AttackAnglePercentage, percentage);
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}


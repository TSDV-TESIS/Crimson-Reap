using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Check not null", story: "[Variable] is not null", category: "Conditions", id: "621572fc518a3272b6fffeda95d0ba1a")]
public partial class CheckNotNullCondition : Condition
{
    [SerializeReference] public BlackboardVariable Variable;

    public override bool IsTrue()
    {
        return Variable.ObjectValue != null;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}

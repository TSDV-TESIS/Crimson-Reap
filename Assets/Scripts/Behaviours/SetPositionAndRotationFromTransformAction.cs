using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Behaviours
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Set Position and Rotation From Transform",
        story: "Set [PositionValue] and [RotationValue] to [Position] position", category: "Action",
        id: "858fc4f806e74998c389b2ecc73d4e00")]
    public partial class SetPositionFromTransformAction : Action
    {
        [SerializeReference] public BlackboardVariable<Vector3> PositionValue;
        [SerializeReference] public BlackboardVariable<Vector3> RotationValue;
        [SerializeReference] public BlackboardVariable<Transform> Position;

        protected override Status OnStart()
        {
            if (PositionValue == null || Position == null || RotationValue == null)

            {
                return Status.Failure;
            }

            RotationValue.Value = Position.Value.rotation.eulerAngles;
            PositionValue.Value = Position.Value.position;
            return Status.Success;
        }
    }
}
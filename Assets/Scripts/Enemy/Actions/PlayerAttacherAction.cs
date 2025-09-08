using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Enemy.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "player attacher", story: "Check if [VisionHandler] sees [player] object and [restart] if changed", category: "Action", id: "be2be2c535978739243872f69c7377f3")]
    public partial class PlayerAttacherAction : Action
    {
    [SerializeReference] public BlackboardVariable<VisionHandler> VisionHandler;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<bool> Restart;

    private bool _wasNull = true;
        protected override Status OnStart()
        {
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            VisionHandler.Value.CanSeeObjective(out GameObject player);
            Player.Value = player;
            if (_wasNull ? !Player.Value : Player.Value) return Status.Success;
            
            _wasNull = !Player.Value;
            Restart.Value = true;
            return Status.Success;
        }

        protected override void OnEnd()
        {
        }
    }
}


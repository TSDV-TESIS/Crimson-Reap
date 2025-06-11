using System;
using Sounds;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.UIElements;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set Sound focal point", story: "Set [Sound] focal point", category: "Action", id: "718dc0045fd48ecd6a8a93e788171c2f")]
public partial class SetSoundFocalPointAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Sound;

    protected override Status OnStart()
    {
        SoundCollisionHandler handler = Sound.Value?.GetComponent<SoundCollisionHandler>();
        if (!handler) return Status.Failure;
        
        Sound.Value = handler.GetScreamingPoint();
        
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


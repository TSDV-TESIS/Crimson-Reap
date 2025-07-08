using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Point Line To Target", story: "Point Line to [Target]", category: "Action", id: "98fe661cb966175f34fe220c9ea56e01")]
public partial class PointLineToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Source; 
    [SerializeReference] public BlackboardVariable<LineRenderer> LinePredictionRenderer;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> MaxLength;
    
    protected override Status OnStart()
    {
        LinePredictionRenderer.Value.gameObject.SetActive(true);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        LineRenderer renderer = LinePredictionRenderer.Value;
        Vector3 sourcePosition = Source.Value.transform.position;
        Vector3 targetPosition = Target.Value.transform.position;
        renderer.SetPosition(0, sourcePosition);

        Vector3 direction = targetPosition - sourcePosition;
        
        renderer.SetPosition(1, sourcePosition + direction.normalized * MaxLength);
        return Status.Running;
    }

    protected override void OnEnd()
    {
        LinePredictionRenderer.Value.gameObject.SetActive(false);
    }
}


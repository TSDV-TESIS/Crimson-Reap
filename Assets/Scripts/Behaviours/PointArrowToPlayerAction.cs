using System;
using Enemy.Attack;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Object = UnityEngine.Object;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Point Arrow to Target", story: "Point [Arrow] to [Target]", category: "Action", id: "98fe661cb966175f34fe220c9ea56e01")]
public partial class PointArrowToPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Source; 
    [SerializeReference] public BlackboardVariable<GameObject> ArrowPrefab;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> MaxSeconds;
    [SerializeReference] public BlackboardVariable<GameObject> Arrow;

    private float _secondsPointing;
    private ArrowAttack _arrowAttack;
    
    protected override Status OnStart()
    {
        Arrow.Value = Object.Instantiate(ArrowPrefab.Value, Self.Value.transform);
        Arrow.Value.transform.position = Source.Value.transform.position;
        _arrowAttack = Arrow.Value.GetComponent<ArrowAttack>();
        _secondsPointing = 0f;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (_secondsPointing == 0f)
        {
            _arrowAttack.StartCharge();    
        }
        
        if(_secondsPointing < MaxSeconds.Value)
        {
            Arrow.Value.transform.LookAt(Target.Value.transform.position);
            _arrowAttack.SetChargeValue(_secondsPointing / MaxSeconds.Value);
            _secondsPointing += Time.deltaTime;
            return Status.Running;
        }
        
        _arrowAttack.SetLoop();

        return Status.Success;
    }
}


using System;
using Enemy.Attack;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Object = UnityEngine.Object;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Create Arrow and Manage Vfx", story: "Create [Arrow] and Manage Vfx", category: "Action", id: "98fe661cb966175f34fe220c9ea56e01")]
public partial class CreateArrowAndManageVfxAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Arrow;
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> ArrowPrefab;
    [SerializeReference] public BlackboardVariable<float> MaxSeconds;
    [SerializeReference] public BlackboardVariable<GameObject> BowBase;
    [SerializeReference] public BlackboardVariable<SkinnedMeshRenderer> BowSkinnedMesh;
    [SerializeReference] public BlackboardVariable<Vector3> ArrowDirection;
    
    private float _secondsPointing;
    private ArrowAttack _arrowAttack;
    private ArrowInArchMovement _arrowInArchMovement;

    protected override Status OnStart()
    {
        Arrow.Value = Object.Instantiate(ArrowPrefab.Value, BowBase.Value.transform);
        _arrowAttack = Arrow.Value.GetComponent<ArrowAttack>();
        _arrowInArchMovement = Arrow.Value.GetComponent<ArrowInArchMovement>();
        _arrowInArchMovement.enabled = true;
        _arrowInArchMovement.BowMesh = BowSkinnedMesh.Value;
        _secondsPointing = 0f;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (_secondsPointing == 0f)
        {
            _arrowAttack.StartCharge();
        }

        if (_secondsPointing < MaxSeconds.Value)
        {
            _arrowAttack.SetChargeValue(_secondsPointing / MaxSeconds.Value);
            _secondsPointing += Time.deltaTime;
            return Status.Running;
        }

        _arrowAttack.SetLoop();
        _arrowInArchMovement.enabled = false;
        Debug.Log($"FORWARD: {Arrow.Value.transform.forward}");
        ArrowDirection.Value = new Vector3(Arrow.Value.transform.forward.x, Arrow.Value.transform.forward.y, 0f).normalized;
        Arrow.Value.transform.parent = Self.Value.transform;

        return Status.Success;
    }
}

using System;
using Unity.Behavior;
using UnityEngine;

namespace Enemy.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "Does enemy see player", story: "Does [VisionHandler] see The player?", category: "Conditions", id: "2fb93108f154d4168fbc891dd870e19c")]
    public partial class DoesEnemySeePlayerCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<VisionHandler> VisionHandler;

        public override bool IsTrue()
        {
            return VisionHandler.Value.CanSeeObjective();
        }

        public override void OnStart()
        {
        }

        public override void OnEnd()
        {
        }
    }
}

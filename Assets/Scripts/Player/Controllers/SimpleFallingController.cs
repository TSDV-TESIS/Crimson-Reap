using System;
using UnityEngine;

namespace Player.Controllers
{
    [RequireComponent(typeof(PlayerMovement))]
    public class SimpleFallingController : FallingController
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            PlayerMovementActions.ShouldAddFasterFallingValues = false;
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            IsFromGroundedTransition = false;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (agent.MovementChecks.IsDoingDropdown())
            {
                agent.ChangeStateToFasterFalling();
            }
        }
        
        public void SetIsFromGroundedTransition()
        {
            IsFromGroundedTransition = true;
        }
    }
}

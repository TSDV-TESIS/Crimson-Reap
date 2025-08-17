using System;
using UnityEngine;

namespace Player.Controllers
{
    [RequireComponent(typeof(PlayerMovement))]
    public class SimpleFallingController : FallingController
    {
        protected override void OnDisable()
        {
            base.OnDisable();
            IsFromGroundedTransition = false;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (PlayerMovementActions.IsGoingDownFaster())
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

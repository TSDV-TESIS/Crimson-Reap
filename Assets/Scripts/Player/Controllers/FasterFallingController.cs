namespace Player.Controllers
{
    public class FasterFallingController : FallingController
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            IsFromGroundedTransition = false;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (!PlayerMovementActions.IsGoingDownFaster())
            {
                agent.ChangeStateToFalling();
            }
        }
    }
}

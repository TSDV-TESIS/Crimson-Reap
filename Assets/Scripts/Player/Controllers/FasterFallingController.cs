namespace Player.Controllers
{
    public class FasterFallingController : FallingController
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            PlayerMovementActions.ShouldAddFasterFallingValues = true;
            IsFromGroundedTransition = false;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (!agent.MovementChecks.IsDoingDropdown())
            {
                agent.ChangeStateToFalling();
            }
        }
    }
}

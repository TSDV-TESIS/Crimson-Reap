using Events;
using UnityEngine;

namespace Player
{
    public class RotationAnimationHandler : StateMachineBehaviour
    {
        [SerializeField] private VoidEventChannelSO onRotationFinish;

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            onRotationFinish?.RaiseEvent(); 
        }
    }
}

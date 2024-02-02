using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetInteracting : StateMachineBehaviour
{

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsInteracting", false);
        animator.applyRootMotion = false;
    }

}

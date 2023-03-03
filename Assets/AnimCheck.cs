using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCheck : StateMachineBehaviour
{
    public string paramToCheck;

    public bool requrieUpdate;
    public bool requireExit = true;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(paramToCheck, true);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!requireExit) return;
        animator.SetBool(paramToCheck, false);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!requrieUpdate) return;
        if(stateInfo.normalizedTime > 0.95f && !animator.IsInTransition(0))
        {
            animator.SetBool(paramToCheck, false);
        }
        
    }
}

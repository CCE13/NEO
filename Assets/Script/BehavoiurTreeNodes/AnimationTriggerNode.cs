using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggerNode : ActionNode
{
    public string triggerName;
    public bool playAnimation;
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if (playAnimation)
        {
            enemyInfo.animator.StopPlayback();
            enemyInfo.animator.Play(triggerName);
        }
        else
        {
            enemyInfo.animator.SetTrigger(triggerName);
        }
        
        return State.Success;
    }
}

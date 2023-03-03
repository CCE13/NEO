using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsBossHitNode : CompositeNode
{
    public bool setToFailue;

    protected override void OnStart()
    {
        blackboard.bossStages = enemyInfo.bossDeathManager.enemyHits;
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {

        if(setToFailue && enemyInfo.bossDeathManager.enemyHits == 1)
        {
            return State.Failure;
        }

        if (enemyInfo.bossDeathManager.enemyHits == 1)
        {
            setToFailue = true;
            blackboard.bossStages = enemyInfo.bossDeathManager.enemyHits;
            children[0].Update();
            return State.Success;
        }
        else if(enemyInfo.bossDeathManager.enemyHits >= 2)
        {
            blackboard.bossStages = enemyInfo.bossDeathManager.enemyHits;
            children[1].Update();
            return State.Success;
        }

        return State.Failure;
    }
}

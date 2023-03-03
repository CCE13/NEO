using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerDashingNode : CompositeNode
{
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (enemyInfo.player.GetComponent<Dashing>().playerState == Dashing.PlayerState.Dashing)
        {
            Debug.Log(enemyInfo.player.GetComponent<Dashing>().playerState);
            return children[0].Update();
        }
        else
        {
            return children[1].Update();
        }

    }
}

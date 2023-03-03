using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashToPlayerNode : ActionNode
{
    public Vector3 targetPos;
    public float speed;
    protected override void OnStart()
    {
        enemyInfo.rigidbody2D.velocity = Vector3.zero;
        targetPos = enemyInfo.player.transform.position;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {


        if (Vector3.Distance(enemyInfo.transform.position, targetPos) <= 0.1f)
        {

            enemyInfo.transform.position = targetPos;
            return State.Success;
        }
        else
        {
            enemyInfo.transform.position = Vector2.MoveTowards(enemyInfo.transform.position, targetPos, speed * Time.deltaTime);
            blackboard.SpottedPlayer = true;
            
            return State.Running;
        }


    }
}

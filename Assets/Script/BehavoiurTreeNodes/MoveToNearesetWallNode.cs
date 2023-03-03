using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToNearesetWallNode : ActionNode
{
    public float radius = 3;
    public Vector2 target;
    protected override void OnStart()
    {
        RaycastHit2D hit = Physics2D.CircleCast(enemyInfo.transform.position, radius, Vector3.one);
        target = hit.collider.ClosestPoint(enemyInfo.transform.position);
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {

        Debug.Log(Vector2.Distance(enemyInfo.transform.position, target));

        if (Vector2.Distance(enemyInfo.transform.position, target) > 1) 
        {
            enemyInfo.rigidbody2D.AddForce(target.normalized, ForceMode2D.Impulse);
            return State.Running;
        }
        else
        {
            enemyInfo.rigidbody2D.velocity = Vector3.zero;
            return State.Success;
        }

    }
}

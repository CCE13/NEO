    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToWaypointNode : ActionNode
{

    public Transform target;
    public float speed = 1;
    public int wayPointIndex;
    public bool useWayPointIndex;
    protected override void OnStart()
    {
        if (useWayPointIndex)
        {
            target = enemyInfo.parentObject.transform.GetChild(wayPointIndex);
        }
        else
        {
            target = enemyInfo.enemyWaypoints.waypoints[Random.Range(0, enemyInfo.enemyWaypoints.waypoints.Length)];
        }
        
        enemyInfo.rigidbody2D.velocity = Vector3.zero;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if(Vector2.Distance(enemyInfo.transform.position, target.position) >= 0.1f)
        {
            enemyInfo.transform.position = Vector2.MoveTowards(enemyInfo.transform.position, target.position, speed * Time.deltaTime);
            return State.Running;
        }
        else
        {
            enemyInfo.transform.position = target.position;
            return State.Success;
        }
        
    }
}

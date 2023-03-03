using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayerNode : ActionNode
{
    /// <summary>
    /// Sets the target position to the players position
    /// </summary>
    public string TagToChase;
    public int waypointIndex;
    Transform _waypoint;
    private Vector2 _waypoinOrigin;
    protected override void OnStart()
    {
        _waypoint = enemyInfo.parentObject.transform.GetChild(waypointIndex);
        _waypoinOrigin = _waypoint.transform.position;

    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if (blackboard.enemyState == Blackboard.EnemyStates.SpottedPlayer)
        {
            blackboard.enemyState = Blackboard.EnemyStates.Chasing;
            _waypoint.transform.position = enemyInfo.player.transform.position;
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}

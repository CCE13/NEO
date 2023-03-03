using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Blackboard
{
    /// <summary>
    /// This blackboard can be accessed by all nodes therefore it is like a means of communication between all of the nodes
    /// </summary>

    public enum EnemyStates { Idle, Patroling, Attacking, Chasing, Missed, Agressive, SpottedPlayer, Dashing, OnWall }
    public EnemyStates enemyState;
    public bool SpottedPlayer;

    public int bossStages;
    public bool changingValue;

    public Vector3 playerPostion;

}

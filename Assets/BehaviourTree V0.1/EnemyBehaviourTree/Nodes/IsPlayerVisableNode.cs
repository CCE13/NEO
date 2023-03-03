using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPlayerVisableNode : CompositeNode
{
    /// <summary>
    /// Checks if the enemy can see the player and if the player is not blocked by a wall
    /// 
    /// Max of two nodes allowed with this node, 
    /// 
    /// it will update node 0 if player is visable and node 1 if player is not visable (the node 0 and node 1 refernce to the corresponding child index in this node)
    /// </summary>
    [SerializeField] private LayerMask enemyLayer;
    public float rayCastLength = 1000;
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {

        //Checks if the there are more then two nodes connected to it
        if(children.Count > 2)
        {
            return State.Failure;
        }

        if (children.Count == 1)
        {
          
            //Runs the children if there is only one child connect
            return children[0].Update();
        }
        if(enemyInfo.player.GetComponent<Dashing>()?.playerState == Dashing.PlayerState.Death)
        {
            blackboard.changingValue = false;
            return children[1].Update();
        }



        //If enemy is on agressive mode it will always return the player is visable
        //if (blackboard.enemyState == Blackboard.EnemyStates.Agressive /* || blackboard.enemyState == Blackboard.EnemyStates.Chasing*/)
        //{
        //    return children[0].Update();
        //}

        //Gets the player postion and sets it to the blackboard so that all nodes can view 
        Vector3 dir = (enemyInfo.player.transform.position - enemyInfo.transform.position).normalized;
        var hit = Physics2D.Raycast(enemyInfo.transform.position, new Vector2(dir.x, dir.y), rayCastLength, ~LayerMask.GetMask("Enemy","Camera","EnemyDeath") );
        //Debug.DrawRay(enemyInfo.transform.position, new Vector2(dir.x, dir.y) * rayCastLength, Color.red, 3);
        if (hit)
        {
            blackboard.playerPostion = hit.transform.position;
        }


        if (hit == false)
        {
            //Debug.Log("No hit");

            enemyInfo.tracing.player = null;
            blackboard.enemyState = Blackboard.EnemyStates.Idle;
            blackboard.changingValue = false;
            return children[1].Update();
            
        }

        if (hit.collider.tag == ("Player"))
        {

            if (enemyInfo.tracing != null)
            {
                enemyInfo.tracing.player = enemyInfo.player;
            }
                
            blackboard.enemyState = Blackboard.EnemyStates.SpottedPlayer;
            
            return children[0].Update();
        }

        
        
        if(hit)
        {

            if (enemyInfo.tracing != null)
            {
                enemyInfo.tracing.player = null;
            }
            
            blackboard.enemyState = Blackboard.EnemyStates.Idle;
            blackboard.changingValue = false;
            return children[1].Update();
        }



        return State.Failure;






    }

}

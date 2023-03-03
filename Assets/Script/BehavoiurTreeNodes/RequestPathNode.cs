using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class RequestPathNode : ActionNode
{
    public Vector3Int[] path;
    public int index;
    public bool startMovingEnemy;
    protected override void OnStart()
    {
        enemyInfo.rigidbody2D.velocity = Vector3.zero;
        if(blackboard.SpottedPlayer == true)
        {
            path = new Vector3Int[0];
            index = 0;
            startMovingEnemy = false;
            enemyInfo.enemyMovement.ResetPathfinding();
            blackboard.SpottedPlayer = false;
        }
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {

        if(path.Length == 0 && Vector3.Distance(enemyInfo.transform.position, enemyInfo.player.transform.position) > 10f)
        {

            enemyInfo.enemyMovement.RunPathfinding(enemyInfo.player.transform);
            if (enemyInfo.enemyMovement.pathGenerationSucess)
            {
                path = enemyInfo.enemyMovement.path;
                startMovingEnemy = enemyInfo.enemyMovement.pathGenerationSucess;
            }
            
            return State.Success;
        }

        if (startMovingEnemy)
        {
            if(index != path.Length)
            {
                
                if(Vector3.Distance(enemyInfo.transform.position, (Vector3)path[index]) > 1f)
                {
                    Vector3 target =  (Vector3)path[index] - enemyInfo.transform.position;
                    enemyInfo.rigidbody2D.AddForce(target.normalized, ForceMode2D.Impulse);
                    //Debug.Log(Vector3.Distance(enemyInfo.transform.position, (Vector3)path[index]));
                    return State.Running;
                }
                else
                {
                    index += 1;
                    enemyInfo.rigidbody2D.velocity = Vector3.zero;
                    return State.Success;
                }
            }
            else
            {
                path = new Vector3Int[0];
                index = 0;
                startMovingEnemy = false;
                enemyInfo.enemyMovement.ResetPathfinding();

                return State.Success;
            }
            
            
        }

        return State.Success;

        
        
    }


    
}

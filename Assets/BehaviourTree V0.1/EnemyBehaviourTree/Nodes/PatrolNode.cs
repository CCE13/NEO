using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolNode : ActionNode
{
    public int waypointIndex;
    private Transform waypoint;
    public float speed;
    public float detectionLeeway = 0.3f;
    public float chasePlayer;
    public LayerMask groundLayer;

    [Space(10)]

    public string WalkAnimationName;

    protected override void OnStart()
    {
        //waypoint = enemyInfo.parentObject.transform.GetChild(waypointIndex);    
    }

    protected override void OnStop()
    {  
    }
    
    protected override State OnUpdate()
    {
        waypoint = enemyInfo.parentObject.transform.GetChild(waypointIndex);
        Debug.DrawRay(enemyInfo.capsuleCollider2D.bounds.center, Vector2.down * (enemyInfo.capsuleCollider2D.bounds.extents.y + 0.5f));
        RaycastHit2D onFloorRay = Physics2D.Raycast(enemyInfo.capsuleCollider2D.bounds.center, Vector2.down, enemyInfo.capsuleCollider2D.bounds.extents.y +  0.5f, groundLayer);

     
        if (chasePlayer == 1)
        {
            blackboard.enemyState = Blackboard.EnemyStates.Chasing;
            waypoint.transform.position = blackboard.playerPostion;
            
        }

        if (onFloorRay.collider == null)
        {
            
            enemyInfo.rigidbody2D.velocity = Vector2.zero;
            return State.Running;
        }

        
        
        if (enemyInfo.transform.position.x < waypoint.position.x && !enemyInfo.player.GetComponent<GroundCheck>().isTouchingCeiling )
        {
            
            moveEnemy(1);
             
        }

        if (enemyInfo.transform.position.x > waypoint.position.x && !enemyInfo.player.GetComponent<GroundCheck>().isTouchingCeiling )
        {          
            moveEnemy(-1);
        }
        //if (enemyInfo.transform.position.x - blackboard.playerPostion.x < detectionLeeway + 0.3f && !enemyInfo.player.GetComponent<GroundCheck>().isTouchingCeiling)
        //{
        //    enemyInfo.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        //    blackboard.MissingPlayerHit = true;
        //    //Debug.Log("Direction" + (enemyInfo.transform.position.x - blackboard.playerPostion.x));
        //    return State.Success;

        //}      


        if (Vector2.Distance(enemyInfo.transform.position, waypoint.transform.position) < detectionLeeway && onFloorRay.collider != null)
        {
            enemyInfo.rigidbody2D.velocity = Vector2.zero; /*new Vector2(0, enemyInfo.rigidbody2D.velocity.y);*/         
            return State.Success;
        }

       
        return State.Running;
    }


    private void moveEnemy(int moveDiretion)
    {
        
        if (enemyInfo.animator.GetBool("isBlocking")) return;
        if (enemyInfo.animator.GetBool("isAttacking")) return;
        enemyInfo.animator.Play(WalkAnimationName);
        
        enemyInfo.rigidbody2D.velocity = new Vector2(speed * moveDiretion, enemyInfo.rigidbody2D.velocity.y);
        enemyInfo.transform.localScale = new Vector3(1 * moveDiretion, 1, 1);


    }
   
}

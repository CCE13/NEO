using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCheckNode : CompositeNode
{
    public enum ColliderToCheck { triggerCollider,contactCollider,boxCast,circleCast, Raycast};
    public enum Check { tag, layer};

    public ColliderToCheck colliderToUse;
    public Check whatToCheckWith;

    public string tagToCheckFor;
    public LayerMask layerToCheckFor;


    public LayerMask playerLayer;
    public Vector3 boxSize;
    public float raycastLength = 4;

    public float circleRadius;
    protected override void OnStart()  
    {

    }

    protected override void OnStop()
    {

    }
    private void OnValidate()
    {
        
        switch (whatToCheckWith)
        {
            case Check.layer:
                tagToCheckFor = string.Empty;
                break;
            case Check.tag:
                layerToCheckFor.value = 0;
                break;
        }
    }

    protected override State OnUpdate()
    {

        switch (colliderToUse)
        {
            case ColliderToCheck.boxCast:
                enemyInfo.gizmos.SetGizomos(boxSize, enemyInfo.boxLocation.position);
                break;
            case ColliderToCheck.circleCast:
                enemyInfo.gizmos.SetGizmos(circleRadius, enemyInfo.transform.position);
                break;
        }
       
       
        //Checks if the there are more then two nodes connected to it
        if (children.Count > 2)
        {
            return State.Failure;
        }

        if (children.Count == 1)
        {
            //Runs the children if there is only one child connect
            return children[0].Update();
        }
        ////If enemy is on agressive mode it will always return the player is visable
        //if (blackboard.enemyState == Blackboard.EnemyStates.Agressive)
        //{
        //    return children[0].Update();
        //}


        switch (colliderToUse)
        {
            case ColliderToCheck.contactCollider:
                switch (whatToCheckWith)
                {
                    case Check.layer:
                        if (enemyInfo.capsuleCollider2D.IsTouchingLayers(layerToCheckFor))
                        {
                            blackboard.enemyState = Blackboard.EnemyStates.Idle;
                            return children[0].Update();
                        }
                        else
                        {
                            blackboard.enemyState = Blackboard.EnemyStates.Idle;
                            return children[1].Update();
                        }
                    case Check.tag:
                        if (enemyInfo.capsuleCollider2D.IsTouching(GameObject.FindGameObjectWithTag(tagToCheckFor).GetComponent<Collider2D>()))
                        {
                            blackboard.enemyState = Blackboard.EnemyStates.Idle;
                            return children[0].Update();
                        }
                        else
                        {
                            blackboard.enemyState = Blackboard.EnemyStates.Idle;
                            return children[1].Update();
                        }
                }
                break;
            case ColliderToCheck.triggerCollider:
                switch (whatToCheckWith)
                {
                    case Check.layer:
                        if (enemyInfo.boxCollider2D.IsTouchingLayers(layerToCheckFor))
                        {
                            blackboard.enemyState = Blackboard.EnemyStates.Idle;
                            return children[0].Update();
                        }
                        else
                        {
                            blackboard.enemyState = Blackboard.EnemyStates.Idle;
                            return children[1].Update();
                        }
                    case Check.tag:
                        if (enemyInfo.boxCollider2D.IsTouching(GameObject.FindGameObjectWithTag(tagToCheckFor).GetComponent<Collider2D>()))
                        {
                            blackboard.enemyState = Blackboard.EnemyStates.Idle;
                            return children[0].Update();
                        }
                        else
                        {
                            blackboard.enemyState = Blackboard.EnemyStates.Idle;
                            return children[1].Update();
                        }
                }
                break;
            case ColliderToCheck.boxCast:
                switch (whatToCheckWith)
                {
                    case Check.layer:
                        Collider2D hit = Physics2D.OverlapBox(enemyInfo.boxLocation.position, boxSize,0,playerLayer);
                        
                        if (hit == null) { return children[1].Update(); }
                        if(hit.IsTouchingLayers(layerToCheckFor) )
                        {
                            return children[0].Update();
                        }
                        else
                        {
                            
                            return children[1].Update();
                        }
                    case Check.tag:
                        Collider2D hit2 = Physics2D.OverlapBox(enemyInfo.boxLocation.position, boxSize, 0,playerLayer);
                        if (hit2 == null) {return children[1].Update(); }
                        if (hit2.CompareTag(tagToCheckFor))
                        {
                            
                            return children[0].Update();
                        }
                        else
                        {
                            
                            return children[1].Update();
                        }
                }
                break;
            case ColliderToCheck.circleCast:
                switch (whatToCheckWith)
                {
                    case Check.layer:
                        Collider2D hit = Physics2D.OverlapCircle(enemyInfo.transform.position, circleRadius);
                        if (hit == null) { return children[1].Update(); }
                        if (hit.IsTouchingLayers(layerToCheckFor))
                        {
                            return children[0].Update();
                        }
                        else
                        {
                            return children[1].Update();
                        }
                    case Check.tag:
                        Collider2D hit2 = Physics2D.OverlapCircle(enemyInfo.transform.position, circleRadius);
                        if (hit2 == null) { return children[1].Update(); }
                        if (hit2.CompareTag(tagToCheckFor))
                        {
                            return children[0].Update();
                        }
                        else
                        {
                            return children[1].Update();
                        }
                }
                break;
            case ColliderToCheck.Raycast:
                switch (whatToCheckWith)
                {
                    case Check.tag:
                        var target = enemyInfo.player.transform.position - enemyInfo.transform.position;
                        RaycastHit2D hit2 = Physics2D.Raycast(enemyInfo.transform.position, target.normalized, raycastLength, ~LayerMask.GetMask("Enemy", "Camera", "EnemyDeath"));
                        Debug.DrawRay(enemyInfo.transform.position, target.normalized * raycastLength, Color.red, 3);
                        if (!hit2) { return children[1].Update(); } else { Debug.Log(hit2.collider); }
                        if(hit2.collider.tag == tagToCheckFor)
                        {
                            return children[0].Update();
                        }
                        else
                        {
                            return children[1].Update();
                        }
                }
                break;


        }

        return State.Failure;
    }
}

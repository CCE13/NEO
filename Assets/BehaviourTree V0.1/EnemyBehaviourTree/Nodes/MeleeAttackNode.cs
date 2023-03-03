using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackNode : ActionNode
{
    public LayerMask AbleToKillLayers;
    public float punchTime;
    public float raycastLength = 1.3f;

    protected override void OnStart()
    {
        //enemyInfo.animator.Play("MeleeEnemyHitting");
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        Debug.Log("Hellohit?");
        //How long the punch will take
        enemyInfo.rigidbody2D.velocity = Vector2.zero;
        if (punchTime > 0)
        {
            enemyInfo.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
            punchTime -= Time.deltaTime;
            return State.Running;
        }

        
        enemyInfo.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;

        //Checks where is going to do its melee attack and what it is hitting
        Vector3 dir = (enemyInfo.player.transform.position - enemyInfo.transform.position).normalized;
        RaycastHit2D HitEnemy = Physics2D.Raycast(enemyInfo.meleeHitPoint.transform.position, dir , raycastLength, AbleToKillLayers);


        //If hit player kill the player
        if (enemyInfo.player.GetComponent<Dashing>().playerState == Dashing.PlayerState.Holding && HitEnemy.collider != null && HitEnemy.collider.tag == "Player")
        {
            Debug.Log("hello");
            enemyInfo.player.GetComponent<Dashing>().playerState = Dashing.PlayerState.Death;
            enemyInfo.player.GetComponent<Dashing>().InvokeEvent();
            return State.Success;
        }

        

        //if hit enemy kill the enemy
        if (HitEnemy.collider != null && HitEnemy.collider != enemyInfo.boxCollider2D && HitEnemy.collider.tag == "Enemy")
        {
            Debug.Log("It Hit");
            
            enemyInfo.player.GetComponent<PlayerManager>().CollectableCollecter.AddToList(enemyInfo.gameObject.GetComponent<Collectable>());
            HitEnemy.collider.GetComponent<EnemyDeathManger>().KillEnemy();
            return State.Success;
        }



        return State.Failure;

    }

    
}

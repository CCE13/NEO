using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlockingNode : ActionNode
{
    public float knockBackStrength;
    public string blocking;

    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {

    }   

    protected override State OnUpdate()
    {
        //play blocking animation
        enemyInfo.animator.Play(blocking);
        enemyInfo.player.GetComponent<Dashing>().beingBlocked = true;
        enemyInfo.rigidbody2D.velocity = Vector2.zero;
        Debug.Log("FUCK YOU IM TRYING TO BNLOCK BUT TIS NOT FUCKING BLOKCING CAN U JUST FUKCVING BLOCK PLS ");
        
        var player = enemyInfo.player;
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        var directionToKnockBack = (player.transform.position - enemyInfo.transform.position).normalized;
        player.transform.position = new Vector3(player.transform.position.x + 0.2f, player.transform.position.y + 0.5f, player.transform.position.z);

        rb.velocity = new Vector2(directionToKnockBack.x * knockBackStrength, 10);
        TimeController.sharedInstance.TransitionToRealTime();
        player.GetComponent<Dashing>().playerState = Dashing.PlayerState.Falling;

        return State.Success;

    }
}



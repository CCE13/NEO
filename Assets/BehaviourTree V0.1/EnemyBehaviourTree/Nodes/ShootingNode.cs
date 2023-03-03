using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingNode : ActionNode
{
    [SerializeField] private GameObject bulletTrail;
    [SerializeField] private float delay = 0.3f;
    [SerializeField] private int missedShots = 1;
    [SerializeField] private bool limitedBullets;
    [SerializeField] private int shootFor;

    [SerializeField] private int shootForTemp;
    [SerializeField] private float gunDelayWait;
    [SerializeField] private int missedShotsTemp;
    // Start is called before the first frame update

    protected override void OnStart()
    {
        
        shootForTemp = shootFor;
        missedShotsTemp = missedShots;
        gunDelayWait = 0f;
    }

    protected override void OnStop()
    {
        shootForTemp = shootFor;
        missedShotsTemp = missedShots;
        gunDelayWait = 0f;

    }



    protected override State OnUpdate()
    {
        if (gunDelayWait != 0 || gunDelayWait != delay)
        {

            if (blackboard.changingValue == false)
            {
                Debug.Log("DelayGone");
                gunDelayWait = 0;
            }
           

        }

        //enemyInfo.animator.Play("Shooting Enemy Idle");

        //if (PauseMenuController.enemyRestart)
        //{
            
        //    missedShotsTemp = missedShots;
        //    gunDelayWait = 0;
        //    PauseMenuController.enemyRestart = false;
        //}

        enemyInfo.rigidbody2D.velocity = Vector2.zero;

        //Delay after each shot
        if (gunDelayWait < delay)
        {
            blackboard.changingValue = true;
            gunDelayWait = gunDelayWait + Time.deltaTime;
            return State.Running;

        }


        blackboard.changingValue = false;

        
        //to locate where the enemy is
        Vector3 direction = (enemyInfo.player.transform.position - enemyInfo.transform.position).normalized;
        var hit = Physics2D.Raycast(enemyInfo.shootingGunPoint.transform.position, direction, 50,  ~LayerMask.GetMask("Camera", "EnemyDeath"));



        if(hit.collider.tag != "Player" || !hit)
        {
           
            return State.Failure;
        }


        
        //If it hits the player shoot at the player else shoot infront of the enemy
        if (hit.collider != null )
        {
            
            enemyInfo.rigidbody2D.velocity = Vector2.zero;
            var trail = Instantiate(bulletTrail, enemyInfo.shootingGunPoint.transform.position, enemyInfo.gameObject.transform.rotation);
            AudioManager.Instance.Play("EnemyShooting", enemyInfo.shootingGunPoint.transform, true);
            CameraShake.Instance.CameraShakeControl(1f, 0.1f);
            var trailScript = trail.GetComponent<BulletTrail>();
            Debug.Log("da fak u not shooting?");
            

            blackboard.enemyState = Blackboard.EnemyStates.Agressive;

            if (missedShotsTemp > 0)
            {
                Debug.Log("missed");
                missedShotsTemp -= 1;
                //Checks the direction of where the enemy is firing the missed shot and sets the bullet postion
                if (enemyInfo.transform.localScale.x == 1)
                {
                    var endPos = enemyInfo.player.transform.right + enemyInfo.player.transform.position;
                    trailScript.SetTargetPostion(endPos, direction, enemyInfo.gameObject.GetInstanceID());
                    //enemyInfo.shootingPOint.rotation = Quaternion.LookRotation(direction, Vector3.up);
                    gunDelayWait = 0f;
                    return State.Running;
                }
                else
                {
                    var endPos = -enemyInfo.player.transform.right + enemyInfo.player.transform.position;
                    trailScript.SetTargetPostion(endPos, direction, enemyInfo.gameObject.GetInstanceID());
                    //enemyInfo.shootingPOint.rotation = Quaternion.LookRotation(direction, Vector3.up);
                    gunDelayWait = 0f;
                    return State.Running;
                }

                
            }

            trailScript.SetTargetPostion(hit.point, direction, enemyInfo.gameObject.GetInstanceID());
            //enemyInfo.shootingPOint.rotation = Quaternion.LookRotation(direction, Vector3.up);

            if (!limitedBullets)
            {
                gunDelayWait = 0f;
                return State.Running;
            }

            if(shootForTemp > 0)
            {
                gunDelayWait = 0f;
                shootForTemp -= 1;
                return State.Running;
            }
            else
            {
                gunDelayWait = 0f;
                return State.Success;
            }



           
        }


        return State.Failure;



    }


}

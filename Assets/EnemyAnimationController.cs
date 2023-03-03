using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    EnemyMovement enemyMovement;
    EnemyDash enemyDash;
    GroundCheck groundCheck;
    [HideInInspector] public Animator playerAnimation;
    Transform spriteTranform;

    //Transform particleTransform;
    // Start is called before the first frame update
    void Start()
    {
        enemyDash = GetComponentInParent<EnemyDash>();
        enemyMovement = GetComponentInParent<EnemyMovement>();
        playerAnimation = GetComponent<Animator>();
        groundCheck = GetComponentInParent<GroundCheck>();
        spriteTranform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyMovement.targetDirection.x > 0 && enemyDash.playerState == EnemyDash.PlayerState.Holding)
        {
            if (groundCheck.isTouchingFloor)
            {
                spriteTranform.localScale = new Vector3(Mathf.Abs(spriteTranform.localScale.x), spriteTranform.localScale.y, spriteTranform.localScale.z);
                //particleTransform.localScale = new Vector3(Mathf.Abs(particleTransform.localScale.x), particleTransform.localScale.y, particleTransform.localScale.z);
            }
            else
            {
                spriteTranform.localScale = new Vector3(-Mathf.Abs(spriteTranform.localScale.x), spriteTranform.localScale.y, spriteTranform.localScale.z);
                //particleTransform.localScale = new Vector3(-Mathf.Abs(particleTransform.localScale.x), particleTransform.localScale.y, particleTransform.localScale.z);
            }
        }

        else
        {
            if (groundCheck.isTouchingFloor)
            {
                spriteTranform.localScale = new Vector3(-Mathf.Abs(spriteTranform.localScale.x), spriteTranform.localScale.y, spriteTranform.localScale.z);
                //particleTransform.localScale = new Vector3(-Mathf.Abs(particleTransform.localScale.x), particleTransform.localScale.y, particleTransform.localScale.z);
            }
            else
            {
                spriteTranform.localScale = new Vector3(Mathf.Abs(spriteTranform.localScale.x), spriteTranform.localScale.y, spriteTranform.localScale.z);
                //particleTransform.localScale = new Vector3(Mathf.Abs(particleTransform.localScale.x), particleTransform.localScale.y, particleTransform.localScale.z);
            }
        }
    }
}

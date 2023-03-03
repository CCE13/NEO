using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public SwipeController swipeDirection;
    Dashing dashing;
    GroundCheck groundCheck;
    [HideInInspector] public Animator playerAnimation;
    Transform spriteTranform;

    Transform particleTransform;

    private void Awake()
    {
        dashing = GetComponentInParent<Dashing>();
        playerAnimation = GetComponent<Animator>();
        groundCheck = GetComponentInParent<GroundCheck>();
        spriteTranform = GetComponent<Transform>();
        particleTransform = GetComponentInChildren<Ghost>().transform;
    }
    void Start()
    {
        dashing.OnPlayerStateChanged += PlayerDashing;
    }
    private void OnDestroy()
    {
        dashing.OnPlayerStateChanged -= PlayerDashing;
    }
    private void PlayerDashing(Dashing.PlayerState playerState)
    {
        playerAnimation.SetBool("isFalling", playerState == Dashing.PlayerState.Falling);
        playerAnimation.SetBool("isDashing", playerState == Dashing.PlayerState.Dashing);
        playerAnimation.SetBool("isOnCeiling", groundCheck.isTouchingCeiling);
        playerAnimation.SetBool("isOnFloor", groundCheck.isTouchingFloor);
        playerAnimation.SetBool("isOnWall", groundCheck.isTouchingWall);

        if (swipeDirection.direction2D.x > 0 && playerState == Dashing.PlayerState.Holding)
        {
            if (groundCheck.isTouchingFloor)
            {
                spriteTranform.localScale = new Vector3(Mathf.Abs(spriteTranform.localScale.x), spriteTranform.localScale.y, spriteTranform.localScale.z);
                particleTransform.localScale = new Vector3(Mathf.Abs(particleTransform.localScale.x), particleTransform.localScale.y, particleTransform.localScale.z);
            }
            else
            {
                spriteTranform.localScale = new Vector3(-Mathf.Abs(spriteTranform.localScale.x), spriteTranform.localScale.y, spriteTranform.localScale.z);
                particleTransform.localScale = new Vector3(-Mathf.Abs(particleTransform.localScale.x), particleTransform.localScale.y, particleTransform.localScale.z);
            }
        }
           
        else
        {
            if (groundCheck.isTouchingFloor)
            {
                spriteTranform.localScale = new Vector3(-Mathf.Abs(spriteTranform.localScale.x), spriteTranform.localScale.y, spriteTranform.localScale.z);
                particleTransform.localScale = new Vector3(-Mathf.Abs(particleTransform.localScale.x), particleTransform.localScale.y, particleTransform.localScale.z);
            }
            else
            {
                spriteTranform.localScale = new Vector3(Mathf.Abs(spriteTranform.localScale.x), spriteTranform.localScale.y, spriteTranform.localScale.z);
                particleTransform.localScale = new Vector3(Mathf.Abs(particleTransform.localScale.x), particleTransform.localScale.y, particleTransform.localScale.z);
            }
        }
            
    }
    
    public void DashSound()
    {
        AudioManager.Instance.Play("Dashing", transform, true);
    }

}
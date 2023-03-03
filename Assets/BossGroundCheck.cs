using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGroundCheck : MonoBehaviour
{
    public bool onGround;
    public bool onWall;
    public bool onCeiling;

    public float ceilingRayLength;
    public float floorRayLength;
    public float wallRayLength;
    public LayerMask groundLayer;
    private Animator anim;
    // Start is called before the first frame update

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        var ceilingRay = Physics2D.Raycast(transform.position, Vector2.up, ceilingRayLength, groundLayer);

        var floorRay = Physics2D.Raycast(transform.position, Vector2.down, floorRayLength, groundLayer);
        var rightWallCheck = Physics2D.Raycast(transform.position, Vector2.right, wallRayLength, groundLayer);
        var leftWallCheck = Physics2D.Raycast(transform.position, Vector2.left, wallRayLength, groundLayer);

        if (ceilingRay.collider != null)
        {
            if (anim.GetBool("isBlocking")) return;
            anim.Play("Boss HoldingOnCeiling");
            if (onWall) return;
            onCeiling = true;

        }
        else
        {
            onCeiling = false;
        }

        if (floorRay.collider != null)
        {
            if (anim.GetBool("isBlocking")) return;
            if (anim.GetBool("isDashing")) return;
            if (anim.GetBool("isOneShot")) return;
            if (onWall) return;
            anim.Play("Boss Idle");
            onGround = true;
        }
        else
        {
            onGround = false;
        }

        if (leftWallCheck.collider != null)
        {
            if (anim.GetBool("isBlocking")) return;
            if (anim.GetBool("isDashing")) return;
            if (anim.GetBool("isOneShot")) return;
            anim.Play("Boss HoldingOnWall");
            transform.localScale = new Vector3(1.43f, 1.43f, 1.43f);
            onWall = true;
        }
        else
        {
            onWall = false;
        }

        if (rightWallCheck.collider != null)
        {
            if (anim.GetBool("isBlocking")) return;
            if (anim.GetBool("isDashing")) return;
            if (anim.GetBool("isOneShot")) return;
            anim.Play("Boss HoldingOnWall");
            transform.localScale = new Vector3(-1.43f, 1.43f, 1.43f);
            onWall = true;
        }
        else
        {
            onWall = false;
        }
    }


}

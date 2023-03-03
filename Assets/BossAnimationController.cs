using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    private GroundCheck groundCheck;
    private Animator anim;
    void Start()
    {
        groundCheck = GetComponentInParent<GroundCheck>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (groundCheck.isTouchingWall)
        {
            anim.Play("Boss HoldingOnWall");
        }

        if (groundCheck.isTouchingCeiling)
        {
            anim.Play("Boss HoldingOnCeiling");
        }
    }
}

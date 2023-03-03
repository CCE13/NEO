using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationHandler : MonoBehaviour
{
    private BossGroundCheck ground;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        ground = GetComponent<BossGroundCheck>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var floor = ground.onGround;
        var ceiling = ground.onCeiling;
        var wall = ground.onWall;

        if (floor)
        {
            anim.Play("Boss Idle");
        }
        if (ceiling)
        {
            anim.Play("Boss HoldingOnCeiling");
        }
        if (wall)
        {
            anim.Play("Boss HoldingOnWall");
        }
    }
}

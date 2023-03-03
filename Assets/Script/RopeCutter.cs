using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RopeCutter : MonoBehaviour
{
    private HingeJoint2D _HingeJointHit;
    // Update is called once per frame


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Link")
        {
            _HingeJointHit = collision.gameObject.GetComponent<HingeJoint2D>();
            collision.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            Destroy(_HingeJointHit);
            collision.gameObject.transform.parent.GetComponent<RopeGenerator>().isCut = true;
            CameraShake.Instance.CameraShakeControl(1.5f, 0.05f);

        }
    }
}

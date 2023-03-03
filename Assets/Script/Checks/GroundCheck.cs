 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Collider Sizes")]
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private Vector3 boxLocation;
    [Space(10)]
    public LayerMask wallLayer;
    public bool isTouchingWall;
    public bool isTouchingFloor;
    public bool isTouchingCeiling;  
    public float touchingFloorDistance = 3;
    public float ceilingRayLength = 0.1f;
    [HideInInspector] public Collider2D wallray;
    [HideInInspector] public RaycastHit2D onFloorRay;
    [HideInInspector] public RaycastHit2D isTouchingCeilingRay;
    private CapsuleCollider2D capsuleCollider;

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();

    }



    // Update is called once per frame
    void LateUpdate()
    {
        wallray = Physics2D.OverlapBox(transform.position + boxLocation, boxSize, 0, wallLayer);
        onFloorRay = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.down, touchingFloorDistance, wallLayer);
        isTouchingCeilingRay = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.up, capsuleCollider.bounds.extents.y + ceilingRayLength, wallLayer);
        Ray2D rey = new Ray2D(capsuleCollider.bounds.center, Vector2.down);

        


        Debug.DrawRay(rey.origin, rey.direction.normalized * touchingFloorDistance, Color.blue);

        isTouchingCeiling = isTouchingCeilingRay;

        if (capsuleCollider.bounds.center.y - capsuleCollider.bounds.extents.y - onFloorRay.point.y <= 0.2 && onFloorRay)
        {
            isTouchingFloor = true;

        }
        else
        {
            isTouchingFloor = false;
        }

        if (wallray)
        {
            isTouchingWall = true;
        }
        else
        {
            isTouchingWall = false;

        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + boxLocation, boxSize);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }



}
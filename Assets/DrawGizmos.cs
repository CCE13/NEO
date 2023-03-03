using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmos : MonoBehaviour
{
    private Vector3 boxSize;
    private Vector3 boxLocation;
    private float circleRadius;
    private Vector3 circleLocation;

    public void SetGizomos(Vector3 size,Vector3 location)
    {
        boxSize = size;
        boxLocation = location;
    }
    public void SetGizmos(float radius,Vector3 location)
    {
        circleRadius = radius;
        circleLocation = location;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxLocation,boxSize);
        Gizmos.DrawSphere(circleLocation, circleRadius);
    }
}

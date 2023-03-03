using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Vector3 ScreenToWorld(Camera camera, Vector3 position)
    {
        //Has to be the distance of the object from the camera
        position.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(position);
    }
}

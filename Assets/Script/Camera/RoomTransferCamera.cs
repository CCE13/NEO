using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cameras
{
    public class RoomTransferCamera : MonoBehaviour
    {
        public string playerTag;
        public GameObject thisRoomVirtualCam;


        public static GameObject cameraPlayerIsInside;
        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(playerTag) && !collision.isTrigger)
            {
                thisRoomVirtualCam.SetActive(true);
                cameraPlayerIsInside = thisRoomVirtualCam;
            }
        }


        public void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == playerTag && !collision.isTrigger)
            {
                thisRoomVirtualCam.SetActive(false);
            }
        }
    }
}


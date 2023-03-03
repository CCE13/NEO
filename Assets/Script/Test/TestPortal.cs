using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPortal : MonoBehaviour
{
    public GameObject player;
    public Transform correspondingPortal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            player.transform.position = correspondingPortal.position;
            //playerRb.velocity = new Vector2(0, 0);
        }
    }
}

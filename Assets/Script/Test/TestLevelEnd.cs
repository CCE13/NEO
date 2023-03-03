using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLevelEnd : MonoBehaviour
{

    public GameObject endScreen;
    // Start is called before the first frame update
    void Start()
    {
        endScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            endScreen.SetActive(true);
        }
    }

}

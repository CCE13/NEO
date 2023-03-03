using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : MonoBehaviour
{
    private Vector2 originalTransform;
    // Start is called before the first frame update
    void Start()
    {
        originalTransform = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - 20f);
        }
        else
        {
            transform.position = originalTransform;
        }
    }
}

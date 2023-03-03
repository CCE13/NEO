using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOutliner : MonoBehaviour
{

    public GameObject mainSprite;

    private GameObject leftOutliner;
    public Material outliner;


    private void Start()
    {
        leftOutliner = Instantiate(mainSprite, transform);
        SpriteRenderer spriteRenderer = leftOutliner.GetComponent<SpriteRenderer>();
        spriteRenderer.material = outliner;
        spriteRenderer.sortingOrder = 2;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

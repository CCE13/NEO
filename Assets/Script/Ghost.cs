using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    PlayerManager playerManager;
    ParticleSystem particle;
    GameObject playerOutline;

    public bool onBoss;
    // Start is called before the first frame update

    private void Start()
    {
        if (onBoss)
        {
            playerOutline = transform.parent.gameObject;
            particle = GetComponent<ParticleSystem>();
        }
        else
        {
            playerManager = FindObjectOfType<PlayerManager>();
            playerOutline = playerManager.transform.GetChild(0).GetChild(0).gameObject;
            particle = GetComponent<ParticleSystem>();
        }





    }

    private void Update()   
    {
        particle.textureSheetAnimation.SetSprite(0, playerOutline.GetComponent<SpriteRenderer>().sprite);
    }
}

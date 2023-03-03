using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyCollectable : MonoBehaviour
{
    [SerializeField] CollectableType collectableType;
    public CollectableType CollectableType => collectableType;
    private void Awake()
    {
    }

    private void OnValidate()
    {
        //if(gameObject.name != null)
        //{
        //    gameObject.name = collectableType.name;
        //    if(collectableType.name == "Melee Enemy")
        //    {
        //        transform.GetChild(1).Find("Gun").gameObject.SetActive(false);
        //        transform.GetChild(1).Find("IK").gameObject.SetActive(false);
        //    }
        //    else
        //    {
        //        transform.GetChild(1).Find("Gun").gameObject.SetActive(true);
        //        transform.GetChild(1).Find("IK").gameObject.SetActive(true);
        //    }
        //}
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.TryGetComponent<PlayerManager>(out var player))
        {
            Debug.Log("yes" + player.CollectableCollecter);
            player.CollectableCollecter.AddToList(this);
            player.enemiesKilledSinceCheckPoint.Add(transform.parent.gameObject);
            CameraShake.Instance.CameraShakeControl(3f, 0.2f);
        }
    }
}

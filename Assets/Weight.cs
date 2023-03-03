using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weight : MonoBehaviour
{
    [SerializeField] private GameObject collectableToReveal;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool HitWall = collision.gameObject.CompareTag("Wall");
        bool HitEnemy = collision.gameObject.CompareTag("Enemy");

        if (HitWall || HitEnemy)
        {
            CameraShake.Instance.CameraShakeControl(4f, 0.06f);
        }

        if (HitEnemy)
        {
            collision.gameObject.GetComponent<EnemyDeathManger>().KillEnemy();
        }
    }
}
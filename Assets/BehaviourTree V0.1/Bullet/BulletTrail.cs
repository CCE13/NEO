using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    private Vector3 startPos;
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private Vector2 boxSize;
    private float destinationReachPerecent;
    private TrailRenderer trailRenderer;
    private float enemyColliderNotToHit;



    Vector3 directionPlayer;

    [SerializeField] private float speed = 40f;
    // Start is called before the first frame update
    void Start()
    {
        startPos = new Vector3(transform.position.x, transform.position.y, -1);
        trailRenderer = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        destinationReachPerecent += Time.deltaTime * speed;
        transform.position = transform.position + (directionPlayer * speed * Time.deltaTime);
        Collider2D[] collider = Physics2D.OverlapBoxAll(transform.position, boxSize, 0);
        //RaycastHit2D[] hit = Physics2D.BoxCastAll(transform.position, boxSize, 0, new Vector2(1,1));
        if (collider.Length > 0 )
        {
            foreach (var Collider in collider)
            {
                
                if (Collider.CompareTag("Player"))
                {
                    Collider.GetComponent<Dashing>().playerState = Dashing.PlayerState.Death;
                    Collider.GetComponent<Dashing>().InvokeEvent();
                    Debug.Log("Kill Player");
                    Destroy(this.gameObject);

                }
                else if (Collider.CompareTag("Wall"))
                {

                    Destroy(this.gameObject);
                }
                else if (Collider.CompareTag("Enemy") && Collider.gameObject.GetInstanceID() != enemyColliderNotToHit )
                {
                    Debug.Log(Collider);
                    Collider.GetComponent<EnemyDeathManger>().KillEnemy();
                    Destroy(this.gameObject);
                }
            }
            
            
        }
        else
        {
            Destroy(this.gameObject, 3f);
            
        }
        
        


    }


    public void SetTargetPostion(Vector3 _targetPos, Vector3 direction, float gameobjectId)
    {
        
        targetPos = new Vector3(_targetPos.x, _targetPos.y, -1);
        directionPlayer = direction;
        enemyColliderNotToHit = gameobjectId;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow; 
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}

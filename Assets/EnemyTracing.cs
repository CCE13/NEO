using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracing : MonoBehaviour
{
    public GameObject gunPivot;
    public GameObject player;


    public Quaternion startRotation;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void TracePlayer(GameObject enemy, GameObject player)
    {
        if (enemy != this) return;
        this.player = player;
    }

    private void Update()
    {
       
        if (player != null)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            float directionToTurn = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if(transform.localScale.x > 0)
            {
                gunPivot.transform.rotation = Quaternion.Lerp(gunPivot.transform.rotation, Quaternion.Euler(0, 0, directionToTurn), 5f * Time.unscaledDeltaTime);
            }
            else
            {
                gunPivot.transform.rotation = Quaternion.Lerp(gunPivot.transform.rotation, Quaternion.Euler(0, 0, directionToTurn- 180), 5f * Time.unscaledDeltaTime);
            }
            
        }
        else
        {
            gunPivot.transform.rotation = Quaternion.Lerp(gunPivot.transform.rotation, Quaternion.Euler(0, 0, 0), 5f * Time.unscaledDeltaTime);
        }
    }
}


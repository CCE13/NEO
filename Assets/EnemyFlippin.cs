using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlippin : MonoBehaviour
{
    private GameObject player;


    public bool isBoss = false;
    // Start is called before the first frame update
    private void Start()
    {
        player = FindObjectOfType<PlayerManager>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<BehaviourTreeRunner>().tree.blackboard.enemyState == Blackboard.EnemyStates.Agressive 
            || GetComponent<BehaviourTreeRunner>().tree.blackboard.enemyState == Blackboard.EnemyStates.SpottedPlayer)
        {
            
            if (player.transform.position.x < transform.position.x)
            {
                if (isBoss)
                {
                    transform.localScale = new Vector3(-1.43f, 1.43f, 1.43f);
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                
                
            }
            else
            {
                if (isBoss)
                {
                    transform.localScale = new Vector3(1.43f, 1.43f, 1.43f);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                
                
            }
        }
    }

    public void SetScale()
    {
        transform.localScale = new Vector3(1.43f, 1.43f, 1.43f);
    }

    public IEnumerator Wait(Vector3 pos)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        //transform.localScale = Vector3.MoveTowards(transform.localScale, pos, 1f);
    }
}

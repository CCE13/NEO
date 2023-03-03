using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{
    public BehaviourTree tree;
    EnemyInfo enemyInfo;
    private void Start()
    {
        enemyInfo = GetEnemyInfo();
        tree = tree.Clone();
        tree.Bind(enemyInfo);
    }

    private void Update()
    {
        tree.Update();
    }

    EnemyInfo GetEnemyInfo()
    {
        return EnemyInfo.getComponenetsOfThis(gameObject);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.white;
    //    Gizmos.DrawLine(enemyInfo.transform.position, enemyInfo.player.transform.position);
    //}
}

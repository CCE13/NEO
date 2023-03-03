using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfo
{
    //This class is to get all the necessary enemy items
    public GameObject player;
    public GameObject gameObject;
    public GameObject parentObject;
    public Rigidbody2D rigidbody2D;
    public Transform transform;
    public Animator animator;
    public EnemyTracing tracing;
    public DrawGizmos gizmos;
    //public GameObject gunGroup;
    public GameObject shootingGunPoint;
    public GameObject meleeHitPoint;
    public GameObject ragdool;
    public BoxCollider2D boxCollider2D;
    public CapsuleCollider2D capsuleCollider2D;
    public EnemyMovement enemyMovement;
    public Image staminaImage;
    public Transform boxLocation;
    public EnemyWaypoints enemyWaypoints;
    public EnemyFlippin flipping;
    public BossDeathManager bossDeathManager;
    //public Transform shootingPOint;

    public static EnemyInfo getComponenetsOfThis(GameObject gameObject)
    {
        EnemyInfo enemyInfo = new EnemyInfo();
        enemyInfo.player = GameObject.FindGameObjectWithTag("Player");
        enemyInfo.gameObject = gameObject;
        enemyInfo.rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        enemyInfo.transform = gameObject.GetComponent<Transform>();
        enemyInfo.animator = gameObject.transform?.GetChild(0).GetComponentInChildren<Animator>();

        enemyInfo.boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        enemyInfo.capsuleCollider2D = gameObject.GetComponent<CapsuleCollider2D>();
        enemyInfo.parentObject = gameObject.transform.parent.gameObject;
        enemyInfo.shootingGunPoint = enemyInfo.transform?.GetChild(0)?.Find("Body")?.Find("Gun Pivot")?.Find("ShootingPoint").gameObject;
        enemyInfo.meleeHitPoint = enemyInfo.transform.Find("MeleeHitPoint")?.gameObject;
        enemyInfo.tracing = gameObject?.GetComponent<EnemyTracing>();
        enemyInfo.staminaImage = gameObject.transform.Find("Stamina Holder")?.GetComponentInChildren<Image>();
        enemyInfo.enemyMovement = gameObject?.GetComponent<EnemyMovement>();
        enemyInfo.gizmos = gameObject?.GetComponent<DrawGizmos>();
        enemyInfo.boxLocation = gameObject.transform?.Find("BlockingPos");
        enemyInfo.enemyWaypoints = gameObject?.GetComponent<EnemyWaypoints>(); 
        enemyInfo.flipping = gameObject.GetComponent<EnemyFlippin>();
        enemyInfo.bossDeathManager = gameObject?.GetComponent<BossDeathManager>();
        //enemyInfo.shootingPOint = gameObject.transform.GetChild(0).Find("Gun Pivot");



        return enemyInfo;
    }
}

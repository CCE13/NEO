using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingEnemyNew : MonoBehaviour
{

    public List<GameObject> placesToDash;
    public float dashTime;
    public bool onGround;
    public bool onWall;
    public bool onCeiling;

    public float ceilingRayLength;
    public float floorRayLength;
    public float wallRayLength;
    public LayerMask groundLayer;
    public float knockBackStrength;

    public LayerMask player;
    [Range(0,1f)]public float speed;

    public Vector3 size;
    public GameObject location;



    private Rigidbody2D rb;
    private Animator anim;

    private float timer;
    [SerializeField] private float forceMultiplierPL = 0.8f;
    [SerializeField] private float forceMultiplierPLMax = 1f;
    [SerializeField] CollectableCollecter collecter;
    public ParticleSystem _deathParticle;
    public GameObject enemyRagdollObject;
    public GameObject newRagdoll;
    public GameObject swordToDrop;
    //GameObject _enemy;
    private Rigidbody2D _rb;
    private Rigidbody2D _pRb;
    private CapsuleCollider2D _capsuleCollider;
    private CollectableType _enemyType;
    private Vector3 _startPos;
    private SpriteRenderer[] _sprite;
    private Rigidbody2D[] enemyRigidbody;
    private Vector2 objVel;
    private Dashing _dashing;
    private bool _dead;
    private bool canDash;

    // Start is called before the first frame update


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _pRb = _deathParticle.GetComponent<Rigidbody2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _enemyType = GetComponent<EnemyCollectable>().CollectableType;
        _sprite = new SpriteRenderer[GetComponentsInChildren<SpriteRenderer>().Length];
        _sprite = GetComponentsInChildren<SpriteRenderer>();
        _dashing = FindObjectOfType<Dashing>();

    }
    // Start is called before the first frame update
    private void Start()
    {
        PauseMenuController.Restarting += Restarted;
        PlayerManager.PlayerDied += Restarted;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        timer = dashTime;
        canDash = false;
        _startPos = transform.position;
    }
    private void OnDestroy()
    {
        PauseMenuController.Restarting -= Restarted;
        PlayerManager.PlayerDied -= Restarted;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(location.transform.position, size);
    }

    // Update is called once per frame
    void Update()
    {
        dashTime -= Time.deltaTime;
        if (_dead) return;
        GroundCheck();
        Blocking();
        StartCoroutine(StandStill());
        if (!canDash) { return; }
        if (dashTime <= 0 && placesToDash.Count > 0)
        {
            StopAllCoroutines();
            int wayPointToGo = Random.Range(0, placesToDash.Count);
            var dir = placesToDash[wayPointToGo].transform.position;
            StartCoroutine(Dash(dir));
            anim.Play("Boss Dashing");
            dashTime = timer;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AudioManager.Instance.Play("EnemyKilling", transform, true);
            objVel = collision.GetComponent<Rigidbody2D>().velocity;
            KillEnemy();
        }
    }

    private IEnumerator StandStill()
    {
        yield return new WaitForSecondsRealtime(4f);
        canDash = true;

    }

    public void KillEnemy()
    {
        canDash = false;
        _dead = true;
        _pRb.AddForce(objVel * Random.Range(forceMultiplierPL, forceMultiplierPLMax), ForceMode2D.Impulse);
        _deathParticle.Play();
        ragdollModeOn();
        GameObject oldRagdoll = enemyRagdollObject;
        swordToDrop.SetActive(true);
        Dashing dashing = FindObjectOfType<Dashing>();
        dashing._dashCount = 2;
        Destroy(oldRagdoll, 1f);

    }
    void Restarted()
    {
        if(_dead) return;
        StopAllCoroutines();
        GameObject oldRagdoll = enemyRagdollObject;
        Destroy(oldRagdoll);
        transform.position = _startPos;
        foreach (SpriteRenderer sprite in _sprite)
        {
            sprite.enabled = true;
        }
        GetComponent<BoxCollider2D>().enabled = true;

        _rb.bodyType = RigidbodyType2D.Dynamic;
        _capsuleCollider.enabled = true;
        newRagdoll = Instantiate(enemyRagdollObject, transform);
        enemyRagdollObject = newRagdoll;      
        enemyRagdollObject.SetActive(false);
        _dead = false;
    }



    private void ragdollModeOn()
    {
        foreach (SpriteRenderer sprite in _sprite)
        {
            sprite.enabled = false;
        }
        _rb.bodyType = RigidbodyType2D.Static;
        _capsuleCollider.enabled = false;

        enemyRagdollObject.SetActive(true);
        GetComponent<BoxCollider2D>().enabled = false;

        enemyRigidbody = GetComponentsInChildren<Rigidbody2D>();

        foreach (Rigidbody2D rig in enemyRigidbody)
        {

            //changes the velocity of the rigidbodies in children when hit
            rig.velocity = objVel * Random.Range(forceMultiplierPL, forceMultiplierPLMax);
        }
    }

    private void Blocking()
    {
        Collider2D hit = Physics2D.OverlapBox(location.transform.position, size, 0, player);

        if (hit != null && hit.CompareTag("Player"))
        {
            anim.Play("Boss Blocking");
            Debug.Log("FUCK YOU IM TRYING TO BNLOCK BUT TIS NOT FUCKING BLOKCING CAN U JUST FUKCVING BLOCK PLS ");

            var player = hit.gameObject;
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            var directionToKnockBack = (player.transform.position - transform.position).normalized;
            player.transform.position = new Vector3(player.transform.position.x + 0.2f, player.transform.position.y + 0.5f, player.transform.position.z);

            rb.velocity = new Vector2(directionToKnockBack.x * knockBackStrength, 10);
            TimeController.sharedInstance.TransitionToRealTime();
            player.GetComponent<Dashing>().playerState = Dashing.PlayerState.Falling;

        }
    }

    private void GroundCheck()
    {
        var ceilingRay = Physics2D.Raycast(transform.position, Vector2.up, ceilingRayLength, groundLayer);

        var floorRay = Physics2D.Raycast(transform.position, Vector2.down, floorRayLength, groundLayer);
        var rightWallCheck = Physics2D.Raycast(transform.position, Vector2.right, wallRayLength, groundLayer);
        var leftWallCheck = Physics2D.Raycast(transform.position, Vector2.left, wallRayLength, groundLayer);

        if (ceilingRay.collider != null)
        {
            if (anim.GetBool("isBlocking")) return;
            anim.Play("Boss HoldingOnCeiling");
            onCeiling = true;

            
        }
        else
        {
            onCeiling = false;
        }

        if (floorRay.collider != null)
        {
            if (anim.GetBool("isBlocking")) return;
            anim.Play("Boss Idle");
            onGround = true;
        }
        else
        {
            onGround = false;
        }

        if (leftWallCheck.collider != null)
        {
            if (anim.GetBool("isBlocking")) return;
            anim.Play("Boss HoldingOnWall");
            transform.localScale = new Vector3(1, 1, 1);
            onWall = true;
        }
        else
        {
            onWall = false;
        }

        if (rightWallCheck.collider != null)
        {
            if (anim.GetBool("isBlocking")) return;
            anim.Play("Boss HoldingOnWall");
            transform.localScale = new Vector3(-1, 1, 1);
            onWall = true;
        }
        else
        {
            onWall = false;
        }
    }

    private IEnumerator Dash(Vector3 dir)
    {
        for (float i = 0; i < 1f; i+=Time.deltaTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, dir, speed);
            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeathManager : MonoBehaviour
{
    [SerializeField] private float forceMultiplierPL = 0.8f;
    [SerializeField] private float forceMultiplierPLMax = 1f;
    [SerializeField] CollectableCollecter collecter;
    public ParticleSystem _deathParticle;
    public GameObject enemyRagdollObject;
    public GameObject newRagdoll;
    public int enemyHits;
    public GameObject Sword;
    //GameObject _enemy;

    private BehaviourTreeRunner _BTRunner;
    private Rigidbody2D _rb;
    private Rigidbody2D _pRb;
    private CapsuleCollider2D _capsuleCollider;
    private CollectableType _enemyType;
    private Vector3 _startPos;
    private SpriteRenderer[] _sprite;
    private Rigidbody2D[] enemyRigidbody;
    private Vector2 objVel;
    private Dashing _dashing;

    // Start is called before the first frame update


    private void Awake()
    {
        _BTRunner = GetComponent<BehaviourTreeRunner>();
        _rb = GetComponent<Rigidbody2D>();
        _pRb = _deathParticle.GetComponent<Rigidbody2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        //_enemyType = GetComponent<EnemyCollectable>()?.CollectableType;
        _sprite = new SpriteRenderer[GetComponentsInChildren<SpriteRenderer>().Length];
        _sprite = GetComponentsInChildren<SpriteRenderer>();
        _dashing = FindObjectOfType<Dashing>();

    }
    private void Start()
    {
        PauseMenuController.Restarting += Restarted;
        PlayerManager.PlayerDied += Restarted;
        _startPos = transform.position;
        newRagdoll = Instantiate(enemyRagdollObject, transform);
    }

    private void OnDestroy()
    {
        PauseMenuController.Restarting -= Restarted;
        PlayerManager.PlayerDied -= Restarted;
    }
    public void StopAudio()
    {
        AudioManager.Instance.Stop("BossMusic");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if(collision.GetComponent<Dashing>().playerState == Dashing.PlayerState.Dashing)
            {
                enemyHits += 1;
                AudioManager.Instance.Play("EnemyKilling", transform, true);
                CameraShake.Instance.CameraShakeControl(2f, 0.2f);
                objVel = collision.GetComponent<Rigidbody2D>().velocity;
                if (enemyHits == 3)
                {
                    KillEnemy();
                }
            }

           
        }
    }



    public void KillEnemy()
    {
        //_pRb.AddForce(objVel * Random.Range(forceMultiplierPL, forceMultiplierPLMax), ForceMode2D.Impulse);
        _deathParticle = transform.GetChild(4).GetComponentInChildren<ParticleSystem>();
        _deathParticle.Play();
        ragdollModeOn();
        _dashing.setDashCount(2);
        GameObject oldRagdoll = enemyRagdollObject;
        Destroy(oldRagdoll, 1f);
        Sword.SetActive(true);

    }
    void Restarted()
    {
        enemyHits = 0;
        GameObject oldRagdoll = enemyRagdollObject;
        Destroy(oldRagdoll);
        transform.position = _startPos;
        foreach (SpriteRenderer sprite in _sprite)
        {
            sprite.enabled = true;
        }
        GetComponent<BoxCollider2D>().enabled = true;
        _BTRunner.enabled = true;
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _capsuleCollider.enabled = true;
        enemyRagdollObject = newRagdoll;
        newRagdoll = Instantiate(enemyRagdollObject, transform);
        enemyRagdollObject.SetActive(false);
    }



    private void ragdollModeOn()
    {
        foreach (SpriteRenderer sprite in _sprite)
        {
            sprite.enabled = false;
        }
        _BTRunner.enabled = false;

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
}

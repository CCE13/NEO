using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyDash : MonoBehaviour
{
    public enum PlayerState
    {
        Dashing,
        Holding,
        Falling,
        Death
    }

    public PlayerState playerState;
    [SerializeField] private InputController controller;
    [SerializeField, Range(0, 10)] private int maxDashAmt;
    public float speed;
    public bool moveDone;
    
    [SerializeField, Range(0, 10)] private float TimeItTakesToDash = 3f;
    [Space(10)]
    public float playerDetectionRadius;
    [SerializeField] private bool sendEvent;

    public event Action<PlayerState> OnPlayerStateChanged;

    private float _speed;
    private int _dashCount;
    private Vector3 _playerDirection;

    private float _timeItTakesToDash;

    public Vector2 _cachedDireciton;
    private Rigidbody2D _rb;

    private bool _onlyOnce;
    private bool _dontDie;
    private bool _mustDie;
    private bool _audioPlayed;

    private GroundCheck _wallGroundCheck;
    [SerializeField] private AnimationCurve _speedAnimCurve;

    public Rigidbody2D RigidBody2D => _rb;

    private void Awake()
    {
        //_waveMovement = GetComponent<SinWaveMovement>();
        _rb = GetComponent<Rigidbody2D>();
        _wallGroundCheck = GetComponent<GroundCheck>();
    }
    private void Start()
    {
        _dontDie = true;
        _timeItTakesToDash = TimeItTakesToDash;
        playerState = PlayerState.Holding;
        _dashCount = maxDashAmt;
        moveDone = true;

    }


    private void FixedUpdate()
    {
        if (PauseMenuController.S_isPaused) { return; }
        //if(_speed == 0)
        //{
        //    moveDone = true;
        //}

        if (playerState == PlayerState.Holding)
        {
            //ConstrainsCheck();
            moveDone = true;
            
            _audioPlayed = false;

        }

        if (_onlyOnce)
        {
          
            RaycastHit2D isThereWall = Physics2D.Raycast(transform.position, -_playerDirection, 2f, ~LayerMask.GetMask("Default", "Camera", "Ignore Raycast"));
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            //Debug.DrawRay(transform.position, -_playerDirection * 2f, Color.cyan, 5);

            //if hit enemy kill the enemy
            if (isThereWall && isThereWall.collider != this.GetComponent<Collider2D>() && isThereWall.collider.tag == "Enemy")
            {
                Debug.Log("It Hit");
                player.GetComponent<PlayerManager>().CollectableCollecter.AddToList(this.gameObject.GetComponent<Collectable>());

                isThereWall.collider.GetComponent<EnemyDeathManger>().KillEnemy();
                ResetDash();

            }

            //If hit player kill the player
            if (player.GetComponent<Dashing>().playerState == Dashing.PlayerState.Holding && isThereWall && isThereWall.collider.tag == "Player")
            {
                Debug.Log("hello33333333333333");
                player.GetComponent<Dashing>().playerState = Dashing.PlayerState.Death;
                player.GetComponent<Dashing>().InvokeEvent();
                ResetDash();

            }
        }

        if (!moveDone)
        {
            if (_timeItTakesToDash > 0)
            {
                _timeItTakesToDash -= (Time.deltaTime % 60);

                _speed = _speedAnimCurve.Evaluate(_timeItTakesToDash);
                Move(_cachedDireciton, _speed);

            }
            
            if (TimeItTakesToDash - _timeItTakesToDash >= 0.1)
            {
               
                CheckIfThereIsWall();
            }

            if (_timeItTakesToDash <= 0)
            {
                playerState = PlayerState.Falling;
                InvokeEvent();
            }

            //CheckIfPlayerIsThere();
        }

    
        
    }

    private void CheckIfPlayerIsThere()
    {
        RaycastHit2D CircleHit = Physics2D.CircleCast(transform.position, playerDetectionRadius, Vector2.zero, 0, LayerMask.GetMask("Player"));
        


        if (CircleHit && !_onlyOnce)
        {
            var target = transform.position - CircleHit.collider.transform.position;
            _playerDirection = target;
            //target = target.normalized;
            RaycastHit2D isThereWall = Physics2D.Raycast(transform.position, -target, 10, LayerMask.GetMask("Player", "Wall"));
            //Debug.DrawRay(transform.position, -target, Color.cyan, 5);

            if (isThereWall.collider.tag == "Player")
            {
                
                _cachedDireciton = -target.normalized;


                _onlyOnce = true;
                //ResetDash();
                //DashMovementTimer(_cachedDireciton);
            }
           
        }
    }

    public void Move(Vector2 direction, float speed)
    {
        

        _rb.velocity = new Vector2(_cachedDireciton.x * speed, _cachedDireciton.y * speed);
    }



    private void ConstrainsCheck()
    {
        if (_wallGroundCheck.isTouchingWall || _wallGroundCheck.isTouchingFloor)
        {
            if (TimeItTakesToDash - _timeItTakesToDash >= 0.1f && playerState == PlayerState.Dashing)
            {
                ResetDash();
            }

            if (playerState != PlayerState.Dashing && playerState != PlayerState.Death && !_wallGroundCheck.isTouchingFloor)
            {
                //Use the same veloctiy as the objects that he hits so that it will stick to the object no matter if its moving or stationary;
                _rb.velocity = _wallGroundCheck.wallray.GetComponent<Rigidbody2D>().velocity;
            }
        }
    }

    private void CheckIfThereIsWall()
    {
        if (_wallGroundCheck.isTouchingWall || _wallGroundCheck.isTouchingFloor)
        {
            
            moveDone = true;
            _onlyOnce = false;
            _speed = 0;
            ResetDash();
        }
    }

    public void DashMovementTimer(Vector2 direction)
    {
        playerState = PlayerState.Dashing;
        moveDone = false;
        _cachedDireciton = direction;

    }

    public void ResetDash()
    {
        //_speed = 0;
        _dashCount = maxDashAmt;
        _rb.velocity = Vector2.zero;
        _cachedDireciton = Vector2.zero;
        _timeItTakesToDash = TimeItTakesToDash;
        playerState = PlayerState.Holding;
        //InputActionManager.S_Touched = false;
        _mustDie = false;
        moveDone = true;
        _onlyOnce = false;
        InvokeEvent();
    }

    //For the slow mo orb
    public void ResetDashSlowMo()
    {
        //Move(15f);

        _dashCount = maxDashAmt;
        _timeItTakesToDash = TimeItTakesToDash;
        //InputActionManager.S_Touched = false;
    }

    public void setDashCount(int dashAmt)
    {
        _dashCount = dashAmt;
    }
    public void PlayerChoose()
    {
        _timeItTakesToDash = 1000f;
        TimeController.sharedInstance.BackToSlowMotion();

    }

    public void InvokeEvent()
    {
        if (sendEvent) { OnPlayerStateChanged?.Invoke(playerState); }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}

using System;
using UnityEngine;

public class Dashing : MonoBehaviour
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

    [SerializeField, Range(0, 10)] private float TimeItTakesToDash = 3f;
    [SerializeField] private bool sendEvent;
    public float fallingSpeed;

    public event Action<PlayerState> OnPlayerStateChanged;

    public event Action MustDie;

    public ParticleSystem puff;

    private float _speed;
    public int _dashCount;

    private float _timeItTakesToDash;

    private Vector2 _cachedDireciton;
    private Rigidbody2D _rb;

    private bool _dontDie;
    public bool _mustDie { get; private set; }
    private bool _audioPlayed;
    private bool _die;
    private bool orbCollected;

    private GroundCheck _wallGroundCheck;
    [SerializeField] private AnimationCurve _speedAnimCurve;

    public Rigidbody2D RigidBody2D => _rb;
    [HideInInspector]public bool beingBlocked;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _wallGroundCheck = GetComponent<GroundCheck>();
    }
    private void Start()
    {
        _dontDie = true;
        _timeItTakesToDash = TimeItTakesToDash;
        playerState = PlayerState.Holding;
        _dashCount = maxDashAmt;

    }

    private void FixedUpdate()
    {
        if (PauseMenuController.S_isPaused) { return; }

        if (playerState == PlayerState.Holding)
        {
            ConstrainsCheck();
            AbleToDashCheck(controller.MoveDirection());
            _audioPlayed = false;
            if (_wallGroundCheck.isTouchingWall && !_wallGroundCheck.isTouchingCeiling)
            {
                _rb.velocity = Vector2.zero;
                _rb.AddForce(Vector2.down * fallingSpeed, ForceMode2D.Force);
            }
            if (!_wallGroundCheck.isTouchingWall && !_wallGroundCheck.isTouchingFloor && !_wallGroundCheck.isTouchingCeiling)
            {
                playerState = PlayerState.Falling;
                InvokeEvent();
            }

        }

        if (playerState == PlayerState.Dashing)
        {
            orbCollected = false;
            beingBlocked = false;
            _dontDie = false;
            _mustDie = false;
            AbleToDashCheck(controller.MoveDirection());
            DashMovementTimer();
            if (!_audioPlayed)
            {
                AudioManager.Instance.Play("Dashing", transform, true);
                _audioPlayed = true;
            }
        }

        if (playerState == PlayerState.Falling)
        {
            AbleToDashCheck(controller.MoveDirection());
            //Need to put this as the player will never die if the raycast calls on update
            
            if (orbCollected)
            {
                Move(5f);
            }
            else
            {
                if (beingBlocked)
                {
                    _rb.AddForce(Vector2.down * (fallingSpeed/10f), ForceMode2D.Force);
                }
                else
                {
                    _rb.velocity = Vector2.zero;
                    _rb.AddForce(Vector2.down * (fallingSpeed * 2f), ForceMode2D.Force);
                }

            }
            
            
            if (_mustDie)
            {
                MustDie?.Invoke();
                if (_wallGroundCheck.isTouchingFloor)
                {
                    playerState = PlayerState.Death;
                    InvokeEvent();
                    
                }
            }
            //If there is no ground the moment the player is falling
            if (!_wallGroundCheck.onFloorRay.collider && playerState != PlayerState.Death && !_dontDie)
            {
                _mustDie = true;
            }
            //if there is reset player dash player dont die
            else if (playerState != PlayerState.Death && !_mustDie)
            {
                if (_wallGroundCheck.isTouchingFloor)
                {
                    ResetDash();

                }
            }
            _audioPlayed = false;
        }

        if (playerState == PlayerState.Death)
        {
            StopAllCoroutines();
            transform.tag = "Untagged";
            if (_wallGroundCheck.isTouchingFloor || _die)
            {
                _die = false;
                foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
                {
                    sprite.enabled = false;
                }
            }
            _audioPlayed = false;
            orbCollected = false;
            beingBlocked = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != null)
        {
            _die = true;
        }
    }
    private void AbleToDashCheck(Vector2 direction)
    {
        if (sendEvent) { if (!InputActionManager.S_Touched) { return; } }

        if (_dashCount <= 0) { return; }

        if (playerState == PlayerState.Dashing || playerState == PlayerState.Falling)
        {
            ResetDashTime();
        }

        playerState = PlayerState.Dashing;
        InvokeEvent();

        //Caches the direction and sets the respective booleans to show that it is dashing
        StopAllCoroutines();
        _cachedDireciton = direction;
        InputActionManager.S_Touched = false;
        _dashCount -= 1;
        if(_dashCount == 0)
        {
            puff.Play();
        }
    }

    private void Move(float speed)
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
            var HIT = Physics2D.OverlapBox(transform.position + new Vector3(0.1f, -0.44f, 0), new Vector2(1.56f, 1.66f), 0, LayerMask.GetMask("Wall"));
            if (HIT != null && HIT.CompareTag("Weight"))
            {

                //Use the same veloctiy as the objects that he hits so that it will stick to the object no matter if its moving or stationary; 
                _rb.velocity = HIT.GetComponent<Rigidbody2D>().velocity;
            }

        }
    }

    private void CheckIfThereIsWall()
    {
        if (_wallGroundCheck.isTouchingWall || _wallGroundCheck.isTouchingFloor || _wallGroundCheck.isTouchingCeiling)
        {
            _speed = 0f;
            ResetDash();
        }
    }

    public void DashMovementTimer()
    {
        if (_timeItTakesToDash > 0)
        {
            _timeItTakesToDash -= (Time.deltaTime % 60);

            _speed = _speedAnimCurve.Evaluate(_timeItTakesToDash);
            Move(_speed);

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
    }

    public void ResetDash()
    {
        _dashCount = maxDashAmt;
        _rb.velocity = Vector2.zero;
        _cachedDireciton = Vector2.zero;
        _timeItTakesToDash = TimeItTakesToDash;
        playerState = PlayerState.Holding;
        InputActionManager.S_Touched = false;
        _mustDie = false;

        InvokeEvent();
    }

    public void ResetDashTime()
    {
        _cachedDireciton = Vector2.zero;
        _timeItTakesToDash = TimeItTakesToDash;
        InputActionManager.S_Touched = false;
        _mustDie = false;

    }

    //For the slow mo orb
    public void ResetDashSlowMo(bool orbCollect)
    {
        Move(15f);
        orbCollected = orbCollect;
        _dashCount = maxDashAmt;
        _timeItTakesToDash = TimeItTakesToDash;
        playerState = PlayerState.Holding;
        InputActionManager.S_Touched = false;
    }

    public void setDashCount(int dashAmt)
    {
        _dashCount = dashAmt;
    }

    public void InvokeEvent()
    {
        if (sendEvent) { OnPlayerStateChanged?.Invoke(playerState); }
    }

    public void PlayerDeath()
    {
        playerState = PlayerState.Death;
        InvokeEvent();
    }
}
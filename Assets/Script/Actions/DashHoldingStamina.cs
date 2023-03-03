using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DashHoldingStamina : MonoBehaviour
{
    //[SerializeField] float holdingOnWallMaxTime = 3f;
    [SerializeField] private float HoldingDegenerationSpeed = 0.5f;
    [SerializeField] private float DashingRegenrationTime = 0.2f;
    [SerializeField] private float StandingRegenrationTime = 0.3f;
    private float _holdingTime = 1;



    Dashing dashing;
    GroundCheck groundCheck;
    public float _currentHoldingTime { get; private set; }

    public static event Action<float> setHoldingTime;

    private void Awake()
    {

        dashing = GetComponent<Dashing>();
        groundCheck = GetComponent<GroundCheck>();
    }
    private void Start()
    {
        ResetTime();
    }
    public void ResetTime()
    {
        _currentHoldingTime = _holdingTime;
    }
    private void Update()
    {


        if (PauseMenuController.S_isPaused) { return; }
        setHoldingTime?.Invoke(_currentHoldingTime);

        bool isDashing = dashing.playerState == Dashing.PlayerState.Dashing;
        bool holdingOnSomething = dashing.playerState == Dashing.PlayerState.Holding && (groundCheck.isTouchingWall|| groundCheck.isTouchingCeiling) && !groundCheck.isTouchingFloor;
        bool onFloor = dashing.playerState == Dashing.PlayerState.Holding && groundCheck.isTouchingFloor;

        if (holdingOnSomething)
        {
            _currentHoldingTime -= HoldingDegenerationSpeed * Time.unscaledDeltaTime;
            _currentHoldingTime = Mathf.Clamp(_currentHoldingTime, 0, _holdingTime);
        }

        if (isDashing|| onFloor)
        {
            _currentHoldingTime = _holdingTime;
        }
        if (_currentHoldingTime <= 0)
        {
            dashing.playerState = Dashing.PlayerState.Falling;
            dashing.InvokeEvent();
        }
    }


}

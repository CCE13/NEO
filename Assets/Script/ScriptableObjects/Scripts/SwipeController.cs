using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SwipeInput", menuName = "InputController/SwipeDetector")]
public class SwipeController : InputController
{
    [SerializeField] private float _minimumDistance;
    [SerializeField] private float _maximumTime;


    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private float _startTime;
    private float _endTime;

    public Vector2 direction2D;

    private void Awake()
    {
        PlayerManager.PlayerDied += resetPos;
    }

    private void OnDestroy()
    {
        PlayerManager.PlayerDied -= resetPos;
    }

    public void resetPos()
    {
        Debug.Log("Hello");
        _startPosition = Vector2.zero;
        _endPosition = Vector2.zero;
    }
    public void StartTouch(Vector2 position, float time)
    {
        _startPosition = position;
        _startTime = time;
       //    Debug.Log("start pos" + position);
    }

    public void EndTouch(Vector2 position, float time)
    {
        _endPosition = position;
        _endTime = time;
        DetectSwipe();
        //Debug.Log("end pos" + position);
    }

    private void DetectSwipe()
    {

        //Checks if the distance is long enough and if the player did not swipe too long for the swipe to register
        bool distanceSwiped = Vector3.Distance(_startPosition, _endPosition) >= _minimumDistance;
        bool swipeOnTime = (_endTime - _startTime) <= _maximumTime;
        if (Vector3.Distance(_startPosition, _endPosition) >= _minimumDistance && (_endTime - _startTime) <= _maximumTime)
        {
            //Debug.Log("Yoooooooooo" + Vector3.Distance(_startPosition, _endPosition));
            InputActionManager.S_Touched = true;
            Debug.DrawLine(_startPosition, _endPosition, Color.red, 5);
            Vector3 direction = _endPosition - _startPosition;
            direction2D = new Vector2(direction.x, direction.y).normalized; //normalise the value , 0 - 1 , as we want the direction and not the length
     
        }
        else
        {
            direction2D = Vector3.zero; 
        }
    }


    public override Vector2 MoveDirection()
    {
        return direction2D;
        
    }


}

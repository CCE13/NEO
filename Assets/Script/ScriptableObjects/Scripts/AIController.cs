using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIController", menuName = "InputController/AIController")]
public class AIController : InputController
{
    public Vector2 direction;
    public Vector2 crossDirection;
    public float amplitude;
    public float frequency;
    public float time;
    public float speed;
    public Vector2 directionCalculation;
    public Vector2 _startPosition;


    public Vector2 ChangeDirection(float time)
    {

        var sinCalculation = (crossDirection * MathF.Sin(time * frequency) * amplitude);

        direction = directionCalculation + sinCalculation;
        direction = direction.normalized;
        Debug.Log(direction);
        return this.direction;
        //this.direction = new Vector2(NormalizedDir.x, direction.y /10);
    }


    public void CalculateDirection(Vector2 direction, float time)
    {
        var normDirection = direction.normalized;
        crossDirection = new Vector2(normDirection.y, -normDirection.x);
        directionCalculation = _startPosition + (direction * time * speed);
        
    }

    public void SetStartPosition(Transform objectTransform)
    {
        _startPosition = objectTransform.position;
    }

    public void SetTime(float time)
    {
        this.time = time;
    }
    public override Vector2 MoveDirection()
    {
        
        
        return ChangeDirection(time);
    }

    

   
}

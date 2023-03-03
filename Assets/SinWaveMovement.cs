//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SinWaveMovement : MonoBehaviour
//{

//    public Transform target;
//    public float speed;

//    public EnemyDash enemyDash;
//    public Vector2 direction;

//    [Space(10)]
//    private Vector2 linePos;
//    private Vector2 lineVelocity;
//    public float angle = 0f;
//    public float maxOffset;
//    public float value;
//    // Start is called before the first frame update
//    void Start()
//    {

//        linePos = transform.position;

//    }


//    public static float pointdir(Vector2 position, Vector2 target)
//    {
//        return Mathf.Atan2(target.y - position.y, target.x - position.x);
//    }
//    public static float lengthdir_x(float length, float dir)
//    {
//        return (Mathf.Cos(dir) * length);
//    }
//    public static float lengthdir_y(float length, float dir)
//    {
      
//        return (Mathf.Sin(dir) * length);
//    }
//    public static Vector2 lengthdir(float length, float dir)
//    {
//        return new Vector2(lengthdir_x(length, dir), lengthdir_y(length, dir));
//    }

//    private void ShootTowards(Vector2 targetPosition)
//    {

//        lineVelocity = lengthdir(speed, pointdir(transform.position, targetPosition));
//    }

//    // Update is called once per frame
//    void Update()
//    {

//        if(enemyDash.moveDone == false) 
//        {
//            return;
//        }

//        ShootTowards(target.position);

//        angle += value * Time.deltaTime;
//        if (angle >= 360f * Mathf.Deg2Rad)
//        {
//            angle -= 360f * Mathf.Deg2Rad;
//        }
//        linePos += lineVelocity * Time.deltaTime;
      

//        float lineVelocityAngle = Mathf.Atan2(lineVelocity.y, lineVelocity.x);
        
//        transform.position = linePos + lengthdir(maxOffset * Mathf.Sin(angle), lineVelocityAngle + 90f * Mathf.Deg2Rad);

        
//        if (Mathf.Round( (angle * Mathf.Rad2Deg) / 10) == 8)
//        {
//            MoveEnemy();
//        }

//        if(Mathf.Round((angle * Mathf.Rad2Deg) / 10) == 28)
//        {

//            MoveEnemy();
            
//        }

//        void MoveEnemy()
//        {
//            direction = linePos + lengthdir(maxOffset * Mathf.Sin(angle), lineVelocityAngle + 90f * Mathf.Deg2Rad);
//            enemyDash.Move(direction);

//            //enemyDash.DashMovementTimer();
//        }

//        Debug.Log(direction);



//        Debug.DrawRay(transform.position, lengthdir(maxOffset * Mathf.Sin(angle), lineVelocityAngle + 90f * Mathf.Deg2Rad), Color.red, 3);

//    }






//}
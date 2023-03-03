using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    
    public Transform player;
    public Vector3Int[] path;
    public int _targetIndex;
    public Vector3 targetDirection;
    Vector3 _targetNodePosition;
    Vector3 _targetPosition;

    public bool dashing;
    public bool goToWall;
    public bool pathGenerationSucess;
    private Vector3 wallPos;


    //[Header("CircleCollider")]

    public float radius;
    private Animator anim;


    public EnemyDash enemyDash;
    // Start is called before the first frame update

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyDash = GetComponent<EnemyDash>();
        anim = GetComponentInChildren<Animator>();



    }

    //private void Update()
    //{
    //    RaycastHit2D CircleHit = Physics2D.CircleCast(transform.position, radius, Vector2.zero, 0, LayerMask.GetMask("Player"));

    //    if (CircleHit)
    //    {
    //        AStarPathRequestManager.RequestPath(transform.position, player.position, OnPathCreation);
    //    }

    //}

    //for on hit colliders

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RunPathfinding(player);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            ResetPathfinding();
        }
    }
    public void RunPathfinding(Transform target)
    {
        Debug.Log(path);
        if (path.Length <= 0)
        {
            _targetPosition = target.position;
            AStarPathRequestManager.RequestPath(transform.position, target.position, OnPathCreation);
        }
        

    }

    public void ResetPathfinding()
    {
        _targetIndex = 0;
        path = new Vector3Int[0];
        AStarPathRequestManager.Reset();
    }

    private void OnPathCreation(Vector3Int[] path, bool sucess)
    {
        Debug.Log(sucess);
        pathGenerationSucess = sucess;
        if (sucess)
        {
            this.path = path;
            _targetIndex = 0;
            //StopCoroutine(SendDirection());
            //StartCoroutine(SendDirection());
        }
    }

    public void GoToWall(Vector3 wallPos)
    {
        this.wallPos = wallPos;
        goToWall = true;
    }

    public IEnumerator SendDirection()
    {
        Vector3 currentWaypoint = path[0];
        int index = path.Length;
        while (true)
        {
            Debug.Log(currentWaypoint);
            if (transform.position == currentWaypoint)
            {
                dashing = false;
                if (Vector3.Distance(currentWaypoint, path[_targetIndex]) > 2)
                {
                    anim.Play("Boss HoldingOnWall");
                    yield return new WaitForSeconds(0.03f);

                    if (goToWall)
                    {
                        Debug.Log("yo");
                        currentWaypoint = wallPos;
                        index += 1;


                    }
                    else
                    {
                        _targetIndex += 1;
                        if (_targetIndex >= index)
                        {
                            _targetIndex = 0;
                            path = new Vector3Int[0];
                            AStarPathRequestManager.Reset();
                            yield break;
                        }
                        currentWaypoint = path[_targetIndex];
                    }

                    goToWall = false;
                }
                else
                {
                    _targetIndex += 1;
                    if (_targetIndex >= index)
                    {
                        _targetIndex = 0;
                        path = new Vector3Int[0];
                        AStarPathRequestManager.Reset();
                        yield break;
                    }
                    currentWaypoint = path[_targetIndex];
                }
                


               



            }

            //Debug.Log($"{path.Length} {index} {_targetIndex} {transform.position - currentWaypoint} ");
            //

            RaycastHit2D hit = Physics2D.Raycast(transform.position, currentWaypoint - transform.position, 1);
            if(hit.collider.tag == "Wall")
            {
                transform.position = Vector3.MoveTowards(transform.position, hit.point, 40 * Time.deltaTime);
            }
            
            anim.Play("Boss Dashing");
            dashing = true;

            //if(enemyDash.playerState != EnemyDash.PlayerState.Dashing)
            //{
            //    GetDirectionToDash(currentWaypoint);
            //}


            yield return null;
        }
    }

    public void MoveTo(Vector3 targetPos)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, 40 * Time.deltaTime);
    }
    private void GetDirectionToDash(Vector3 targetPosition)
    {   
        _targetNodePosition = targetPosition;
        Vector3 direction = targetPosition - transform.position;
        targetDirection = direction.normalized;
        enemyDash.DashMovementTimer(targetDirection);
        //if (!CheckIfDirectionBlocekd())
        //{
        //    Debug.DrawRay(transform.position, targetDirection, Color.red, 3);
        //    enemyDash.DashMovementTimer(targetDirection);
        //}
        
        
        
    }

    private bool CheckIfDirectionBlocekd()
    {
        Vector3 fixedPosition = new Vector3(_targetNodePosition.x, -_targetNodePosition.y, _targetNodePosition.z);       
        RaycastHit2D hit = Physics2D.Raycast(transform.position, fixedPosition, 1, LayerMask.GetMask("Wall"));
        Debug.DrawRay(transform.position, fixedPosition * 1, Color.yellow, 30);

        if (hit)
        {
            Debug.Log("wall blocking");
            
            return HitNextBestWall();
        }
        else
        {
            //Debug.Log("wall not blocking");
            return false;
        }
    }

    private bool HitNextBestWall()
    {
        var realTargetPos = _targetPosition - transform.position;
        realTargetPos = realTargetPos.normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, realTargetPos, 8, LayerMask.GetMask("Wall"));
        Debug.DrawRay(transform.position, realTargetPos * 8, Color.green, 3);

        if(hit /*&& Vector2.Distance(hit.point, transform.position) >= 2*/)
        {
            
            RaycastHit2D hitback = Physics2D.Raycast(transform.position, realTargetPos, 1, LayerMask.GetMask("Wall"));

            if (hitback )
            {
                targetDirection = -realTargetPos;
                return false;
            }
            else
            {
                //check if the back got range
                return CheckVerticle();
            }
        }
        else
        {
            targetDirection = realTargetPos.normalized;
            return false;
            
        }
        

        
    }

    private bool CheckVerticle()
    {
        Vector3 UpDirection = targetDirection * Vector2.up;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, UpDirection , 1, LayerMask.GetMask("Wall"));
        
        //if the enemy is too close to the wall go down else go up

        if (!hit)
        {
            targetDirection = UpDirection;
            return false;
        }
        else
        {
            targetDirection = -UpDirection;
            return false;
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }






}

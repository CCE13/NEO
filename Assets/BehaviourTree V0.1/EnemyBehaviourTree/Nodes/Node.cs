using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : ScriptableObject
{
    public enum State
    {
        Running,
        Failure,
        Success
    }

    [HideInInspector]public State state = State.Running;
    [HideInInspector] public bool started = false;
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 position;
    [HideInInspector] public EnemyInfo enemyInfo;
    [HideInInspector] public Blackboard blackboard;
    //add the enemy class later\
    [TextArea] public string description;

    //Runs like an update functions and runs the other 3 methods 
    public State Update()
    {
        if (!started)
        {
            OnStart();
            started = true;
        }
        state = OnUpdate();

        if(state == State.Failure || state == State.Success)
        {
            OnStop();
            started = false;
        }

        return state;
    }

    public virtual Node Clone()
    {
        return Instantiate(this);
    }

    public void Abort()
    {
        BehaviourTree.Traverse(this, (node) => {
            node.started = false;
            node.state = State.Running;
            node.OnStop();
        });
    }
    //The three functions that will be on the node
    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract State OnUpdate();
}

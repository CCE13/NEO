using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencerNode : CompositeNode
{
    //This is the squence in which the nodes will be running 
    int current;
    protected override void OnStart()
    {
        current = 0;
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        var child = children[current];
        switch (child.Update())
        {
            case State.Running:
                return State.Running;
            case State.Failure:
                return State.Failure;
            case State.Success:
                current++;
                break;
        }

        //if it finish running through all the child it returns State.success if it does not retrun State.running
        return current == children.Count ? State.Success : State.Running;
    }
}

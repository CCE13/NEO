using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogNode : ActionNode
{
    public string message;
    protected override void OnStart()
    {
        Debug.Log(message);
    }

    protected override void OnStop()
    {
        Debug.Log(message);
    }

    protected override State OnUpdate()
    {
        Debug.Log(message);
        return State.Success;
        
    }
}

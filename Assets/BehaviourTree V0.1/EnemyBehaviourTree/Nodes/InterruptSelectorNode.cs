using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterruptSelectorNode : SelectorNode
{
    protected override State OnUpdate()
    {
        int previous = current;
        base.OnStart();
        var status = base.OnUpdate();
        //Debug.Log($"{children[previous]} {previous} {current}");
        if (previous != current)
        {
            if (children[previous].state == State.Running)
            {
                children[previous].Abort();
                
            }
        }

        return status;
    }
}

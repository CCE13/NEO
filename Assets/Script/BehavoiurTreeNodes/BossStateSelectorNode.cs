using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateSelectorNode : CompositeNode
{
    public int amountOfStates;
    protected override void OnStart()
    {
        amountOfStates = children.Count;
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {


        int RandomChild = Random.Range(0, amountOfStates);
        Debug.Log(RandomChild);
        children[RandomChild].Update();
        return State.Success;



    }
}

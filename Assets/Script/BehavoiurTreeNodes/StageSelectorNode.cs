using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StageSelectorNode : CompositeNode
{
    public List<string> IncludedChildPerStage;
    protected int current;
   

    protected override void OnStart()
    {
        List<int> childrenToSelect = IncludedChildPerStage[blackboard.bossStages].Split(',').Select(int.Parse).ToList();
        current = Random.Range(0, childrenToSelect.Count);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        var child = children[current];
        return child.Update();
    }
}

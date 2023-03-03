using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Node: IHeapItem<A_Node>
{
    public int G { get; set; }
    public int H { get; set; }
    public int F { get; set; }
    public A_Node StartingNode { get; set; }
    public Vector3Int Position { get; set; }
    public A_Node Parent { get; set; }

    public int movementPenalty { get; set; }

    public int goThrouhWallPenalty { get; set; }

    int heapindex;

    public A_Node(Vector3Int position)
    {
        Position = position;
    }

    public int HeapIndex { get { return heapindex; } set { heapindex = value; } }

    public int CompareTo(A_Node compareThisNode)
    {
        int compare = F.CompareTo(compareThisNode.F);

        if (compare == 0)
        {
            compare = H.CompareTo(compareThisNode.H);
        }

        return -compare;
    }
}

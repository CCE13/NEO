using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarf : MonoBehaviour
{
    public Transform scarfLocation;
    public int scarfVertexCount;
    public float scarfVertexDisplacement;
    public LineRenderer lr;
    private LineRenderer _lr;

    public List<Vector2> _scarfPosList = new List<Vector2>();

    private void Awake()
    {
        _lr = Instantiate(lr, Vector3.zero, Quaternion.identity);
        _lr.positionCount = scarfVertexCount;

        for (int i = 0; i < _lr.positionCount; i++)
        {
            _lr.SetPosition(i, scarfLocation.position);
            _scarfPosList.Add(scarfLocation.position);
        }
    }

    private void Update()
    {
        _lr.SetPosition(0, scarfLocation.position);
        _scarfPosList[0] = scarfLocation.position;
        
        if(Vector2.Distance(_scarfPosList[1], _scarfPosList[0]) > scarfVertexDisplacement)
        {
            _scarfPosList[1] = _scarfPosList[0];
            for (int i = 1; i < _scarfPosList.Capacity; i++)
            {
                if( i + 1 < _scarfPosList.Capacity)
                {
                    _scarfPosList[i + 1] = _scarfPosList[i];
                }
            }
        }
    }
}

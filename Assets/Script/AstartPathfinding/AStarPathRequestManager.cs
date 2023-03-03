using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AStarPathRequestManager : MonoBehaviour
{

    Queue<PathRequest> _pathRequestQueue = new Queue<PathRequest>();
    PathRequest _currentPathRequest;

    static AStarPathRequestManager instance;
    Astar _astar;
    bool _isProcessingPath;

    private void Awake()
    {
        instance = this;
        _astar = GetComponent<Astar>();
    }
    public static void RequestPath(Vector3 startPath, Vector3 endPath, Action<Vector3Int[], bool> callback)
    {
        
        PathRequest pathRequest = new PathRequest(startPath, endPath, callback);
        instance._pathRequestQueue.Enqueue(pathRequest);
        
        instance.TryProcessNext();
    }

    public static void Reset()
    {
        instance._astar.Reset();
    }

    private void TryProcessNext()
    {
        if(!_isProcessingPath && _pathRequestQueue.Count > 0)
        {
            //Gets the first path from the queue
            _currentPathRequest = _pathRequestQueue.Dequeue();
            _isProcessingPath = true;

            //Generate the path
            _astar.StartFindingPath(_currentPathRequest.startPath, _currentPathRequest.endPath);
            


        }
    }

    public void FinishProcessingPath(Vector3Int[] path, bool success)
    {
        
        _currentPathRequest.callback(path, success);

        _isProcessingPath = false;

        TryProcessNext();
    }

    struct PathRequest
    {
        public Vector3 startPath;
        public Vector3 endPath;
        public Action<Vector3Int[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3Int[], bool> _callback)
        {
            startPath = _start;
            endPath = _end;
            callback = _callback;
        }
    }
}

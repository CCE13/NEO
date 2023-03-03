using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Diagnostics;
using System;


public enum TileType { Ground, Wall }
public class Astar : MonoBehaviour
{
    private TileType tileType;
    public GameObject enemy;
    public Tilemap foregroundTilemap;
    public Tilemap midgroundTilemap;
    public Tilemap AstartTilemap;
    public GroundType[] groundValues;
    LayerMask _walkableMask;
    Dictionary<int, int> _walkableRegions = new Dictionary<int, int>();

    //Hashset is used as it does not allow any duplicate object
    private Heap<A_Node> _openNodeList;
    private HashSet<A_Node> _closedNodeList;
    private Dictionary<Vector3Int, A_Node> _allNodes =  new Dictionary<Vector3Int, A_Node>();
    private Vector3Int[] _path;


    private A_Node _currentInspectedNode;
    private AStarPathRequestManager _pathRequestManager;
    private Vector3Int _startPos, _goalPos;
    

    private bool _isonwall;
    private Vector3Int[] waypoints = new Vector3Int [0];
    private bool pathSuccess = false;

    private Vector3 _gridSize;

    //private int gridX;
    //private int gridY;

    int penaltyMin = int.MaxValue;
    int penaltyMax = int.MinValue;  

    private void Awake()
    {
        _pathRequestManager = GetComponent<AStarPathRequestManager>();
        
        AstartTilemap = GameObject.FindGameObjectWithTag("AstarTile").GetComponent<Tilemap>();
        _gridSize = AstartTilemap.GetComponent<BoxCollider2D>().size/2;
       

        foreach (GroundType type in groundValues)
        {
            
            _walkableMask.value |= type.groundLayer.value;
            //Gets the binary format of the layer as a key and the layer to store it in a dicnorary
            _walkableRegions.Add( (int)Mathf.Log(type.groundLayer.value, 2), type.terrainPenalty);
        }
    }



    private IEnumerator Algorithm(Vector3 startPos, Vector3 goalPos)
    {
        _startPos = Vector3Int.FloorToInt( startPos);
        _goalPos = Vector3Int.FloorToInt(goalPos);
        if(_currentInspectedNode == null)
        {
            Initialize();
        }
        Stopwatch sw = new Stopwatch();
        

        while(_openNodeList.Count > 0 && _path == null)
        {
            sw.Start();
            List<A_Node> neighbours = FindNeighbours(_currentInspectedNode.Position);
            CheckNeightbours(neighbours, _currentInspectedNode);

            UpdateInspectedTile(ref _currentInspectedNode);

            
            _path = GeneratePath(_currentInspectedNode);
            

        }
        yield return null;

        UnityEngine.Debug.Log("DonePath");
        
        if (pathSuccess)
        {
            //BlurPenaltyMap(10);
            _openNodeList.ResetNodes();
            waypoints = _path;
        }

        _pathRequestManager.FinishProcessingPath(waypoints, pathSuccess);



        sw.Stop();
        print("Time it took " + sw.ElapsedMilliseconds);

        
        AstarDebugger.Instance?.CreateTiles(_openNodeList, _closedNodeList, _allNodes, _startPos, _goalPos, _path);
    }

    private void BlurPenaltyMap(int blurSize)
    {
        int kernalSize = blurSize * 2 + 1;
        int gridX = 50;
        int gridY = 50;
        int[,] pentaltiesHorizontalPath = new int[gridX, gridY];
        int[,] pentaltiesVerticlePath = new int[gridX, gridY];

        for(int y = 0; y < gridY; y++)
        {
            for(int x = -blurSize; x <= blurSize; x++)
            {
                //Clamp x to be zero so that it will not be out of bounds
                int sampleX = Mathf.Clamp(x, 0, blurSize);
                UnityEngine.Debug.Log(new Vector3Int(sampleX, y));
                UnityEngine.Debug.Log(GetNodeWorldSpace(new Vector3Int(sampleX, y)).movementPenalty);
                UnityEngine.Debug.Log(pentaltiesHorizontalPath[0, y]);
                pentaltiesHorizontalPath[0, y] += GetNodeWorldSpace(new Vector3Int(sampleX, y)).movementPenalty;

            }

            for (int x = 1; x < gridX; x++)
            {
                int removeIndex = Mathf.Clamp(x - blurSize - 1, 0, gridX);
                int addIndex = Mathf.Clamp( x + blurSize, 0, gridX -1);

                pentaltiesHorizontalPath[x, y] = pentaltiesHorizontalPath[x - 1, y] - GetNodeWorldSpace(new Vector3Int(removeIndex, y)).movementPenalty + GetNodeWorldSpace(new Vector3Int(addIndex, y)).movementPenalty;
            }
        }

        for (int x = 0; x < gridX; x++)
        {
            for (int y = -blurSize; y <= blurSize; y++)
            {
                //Clamp x to be zero so that it will not be out of bounds
                int sampleY = Mathf.Clamp(y, 0, blurSize);
                pentaltiesVerticlePath[x, 0] += pentaltiesHorizontalPath[x, sampleY];

            }

            for (int y = 1; y < gridY; y++)
            {
                int removeIndex = Mathf.Clamp(y - blurSize - 1, 0, gridY);
                int addIndex = Mathf.Clamp(y + blurSize, 0, gridY - 1);

                pentaltiesVerticlePath[x, y] = pentaltiesVerticlePath[x, y - 1] - pentaltiesHorizontalPath[x, removeIndex] + pentaltiesHorizontalPath[x, addIndex];

                int blurredPenalty = Mathf.RoundToInt( (float) pentaltiesVerticlePath[x,y]/(kernalSize * kernalSize));

                GetNodeWorldSpace(new Vector3Int(x, y)).movementPenalty = blurredPenalty;

                if(blurredPenalty > penaltyMax)
                {
                    penaltyMax = blurredPenalty; 
                }
                if(blurredPenalty < penaltyMin)
                {
                    penaltyMin = blurredPenalty;
                }
            }
        }


    }


    private List<A_Node> FindNeighbours(Vector3Int parentPosition)
    {
        List<A_Node> neighbors = new List<A_Node>();

        //Check a 3x3 square near the parent node
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
               // UnityEngine.Debug.Log(parentPosition);
                Vector3Int neighbourPosition = new Vector3Int(parentPosition.x - x, parentPosition.y - y, parentPosition.z);
                //if y is not a zero and x is not a zero its not 0,0 (which is the parent node location) //
                if(y != 0 || x != 0)
                {
                    //
                    //neighbors.Add(GetNode(neighbourPosition));
                    if (neighbourPosition != _startPos)
                    {

                        //var foregroundTileCheck = foregroundTilemap.GetTile(foregroundTilemap.WorldToCell(neighbourPosition));
                        //var midgroundTileCheck = midgroundTilemap.GetTile(midgroundTilemap.WorldToCell(neighbourPosition)); 

                        //if its on the astar map
                        if (AstartTilemap.GetTile(AstartTilemap.WorldToCell(neighbourPosition)))
                        {

                            
                            neighbors.Add(GetNode(neighbourPosition));

                            



                        }
                        
                        
                    }
                    
                }
            }
        }

        

        return neighbors;
    }

    public A_Node GetNodeWorldSpace(Vector3Int pos)
    {
        var NodePosGrid = AstartTilemap.WorldToCell(pos);
        if (_allNodes.ContainsKey(NodePosGrid))
        {
            //If node exist
            return _allNodes[NodePosGrid];
        }
        else
        {
            return null;
        }
    }

    private void CheckNeightbours(List<A_Node> neighbours, A_Node inspectedNode)
    {
        for (int i = 0; i < neighbours.Count; i++)
        {
            A_Node currentNeighbour = neighbours[i];
            int gScore = DetermineGScore(currentNeighbour.Position, inspectedNode.Position);

            int movementPenalty = 0;
            // raycast here


            Vector2 pos = new Vector2(currentNeighbour.Position.x, currentNeighbour.Position.y);
            //RaycastHit2D hit = Physics2D.Raycast(pos, Vector3.up, 1, _walkableMask);
            //RaycastHit2D hit = Physics2D.BoxCast(pos, new Vector2(2f, 2f), 0, Vector2.zero, 3, _walkableMask);
            //if (hit)
            //{
            //    UnityEngine.Debug.Log(hit.collider.gameObject.layer);
                
            //    int movePen;
            //    _walkableRegions.TryGetValue(hit.collider.gameObject.layer, out movePen);
            //    movementPenalty = movePen;



            //}
            //else
            //{
            //    //Debug.Log("no hit");
            //}
           

            //if the node is in the open list
            if (_openNodeList.Contains(currentNeighbour))
            {
                //Check if the new g score is smaller then the neighbour old g score re calculate values
                if(inspectedNode.G + gScore < currentNeighbour.G)
                {
                    
                   CalculateValues(inspectedNode, currentNeighbour, gScore, movementPenalty);
                }
            }else if (!_closedNodeList.Contains(currentNeighbour))
            {
                CalculateValues(inspectedNode, currentNeighbour, gScore, movementPenalty);
                _openNodeList.Add(currentNeighbour);
            }
         

        }
    }

    private void Initialize()
    {
        _currentInspectedNode = GetNode(_startPos);


        _openNodeList = new Heap<A_Node>(100);
        //int yo = GameObject.FindGameObjectWithTag("AstarTile").GetComponent<Tilemap>().size.x + GameObject.FindGameObjectWithTag("AstarTile").GetComponent<Tilemap>().size.y;

        
        _closedNodeList = new HashSet<A_Node>();    

        _openNodeList.Add(_currentInspectedNode);
    }

    //Get node from position
    private A_Node GetNode(Vector3Int position)
    {
        if (_allNodes.ContainsKey(position))
        {
            //If node exist
            return _allNodes[position];
        }
        else
        {
            //if node doesnt exist create new node

            A_Node node = new A_Node(position);
            _allNodes.Add(position, node);  
            return node;        
        }
    }

    private void CalculateValues(A_Node parent, A_Node neighbour, int cost, int movePen)
    {
        neighbour.Parent = parent;
        neighbour.movementPenalty = movePen;
        neighbour.G = parent.G + cost;
        //the distance form the node to the target node
        neighbour.H = Mathf.Abs((neighbour.Position.x - _goalPos.x) + Mathf.Abs(neighbour.Position.y - _goalPos.y) * 10 );

        neighbour.F = neighbour.G + neighbour.H + neighbour.movementPenalty;

    }

    private int DetermineGScore(Vector3Int neighbour, Vector3Int inspectedNode)
    {
        int gScore = 0;

        int x = inspectedNode.x - neighbour.x;
        int y = inspectedNode.y - neighbour.y;

        if(Mathf.Abs(x-y) % 2 == 1)
        {
            gScore = 10;
        }
        else
        {
            gScore = 14;
        }

        return gScore;
    }

    //sometimes have no reference when you take out of the list thats why its using ref
    private void UpdateInspectedTile(ref A_Node inspecedNode)
    {


        inspecedNode = _openNodeList.RemoveFirstItem();
        _closedNodeList.Add(inspecedNode);


    }



    private Vector3Int[] GeneratePath(A_Node targetNode)
    {
        UnityEngine.Debug.Log(_goalPos);

        if (Vector3.Distance( targetNode.Position, AstartTilemap.CellToWorld(_goalPos)) < 1)
        {
            List<A_Node> targetPath = new List<A_Node>();
            

            while (targetNode.Position != _startPos)
            {
                //adds the position to the stack and push it to the end
                targetPath.Add(targetNode);

                targetNode = targetNode.Parent;
            }

            Vector3Int[] waypoints = SimplifyPath(targetPath);

            //waypoints.Reverse();
            
            pathSuccess = true;

            return waypoints;
        }

        return null;
    }

    private Vector3Int[] SimplifyPath(List<A_Node> targetPath)
    {
        List<Vector3Int> waypoints = new List<Vector3Int>();
        Vector2 directionOld = Vector2.zero;
        int index = 0;

        for (int i = 1; i < targetPath.Count; i++)
        {
            Vector2 directionNew = new Vector2(targetPath[i - 1].Position.x - targetPath[i].Position.x, targetPath[i - 1].Position.y - targetPath[i].Position.y);
            index += 1;

            if (directionNew != directionOld /*|| index % 4 == 0*/)
            {
                waypoints.Add(targetPath[i].Position);
            }

            directionOld = directionNew;
           
        }

        waypoints.Reverse();

        return waypoints.ToArray();
    }

    public void StartFindingPath(Vector3 startPos, Vector3 goalPos)
    {
        StartCoroutine( Algorithm(startPos, goalPos));
    }

    public void Reset()
    {
        //foreach (Vector3Int pos in _allNodes.Keys)
        //{
        //    AstartTilemap.SetTile(pos, null);
        //}

        _allNodes.Clear();
        _path = null;
        _currentInspectedNode = null;
    }

    [System.Serializable]
    public class GroundType
    {
        public LayerMask groundLayer;
        public int terrainPenalty;
    }
} 

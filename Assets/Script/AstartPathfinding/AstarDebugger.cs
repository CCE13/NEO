using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AstarDebugger : MonoBehaviour
{
    private static AstarDebugger _instance; 

    public static AstarDebugger Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<AstarDebugger>();  
            }

            return _instance;
        }
    }

    [SerializeField] private Grid grid;
    public Tilemap tilemap;
    [SerializeField] private Tile tile;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Color openColour, closeColour, pathColour, currentColour, startColour, goalColour;
    [SerializeField] private GameObject debugText;

    private List<GameObject> _debugObjects = new List<GameObject>();


    public void CreateTiles(Heap<A_Node> openNodeList, HashSet<A_Node> closedNodeList, Dictionary<Vector3Int, A_Node> allNodes, Vector3Int start, Vector3Int goal,  Vector3Int[] path = null)
    {
        for (int i = 0; i < openNodeList.Count; i++)
        {
            ColourTile(openNodeList.items[i].Position, openColour);
        }


        foreach (A_Node node in closedNodeList)
        {
            ColourTile(node.Position, closeColour);
        }

        if(path != null)
        {
            foreach(Vector3Int pos in path)
            {
                if(pos != start && pos != goal)
                {
                    ColourTile(pos, pathColour);
                }
            }
        }

        ColourTile(start, startColour);
        ColourTile(goal, goalColour);

        //Sets the debug arrows position
        foreach (KeyValuePair<Vector3Int, A_Node> node in allNodes)
        {
            if(node.Value.Parent != null)
            {
                GameObject go = Instantiate(debugText, canvas.transform);
                go.transform.position = node.Key;
                _debugObjects.Add(go);
                GenerateDebugText(node.Value, go.GetComponent<AstarDebugText>());

            }
        }
    }

    private void GenerateDebugText(A_Node node, AstarDebugText debugText)
    {

        debugText.G.text = $"G:{node.G}";
        debugText.H.text = $"H:{node.H}";
        debugText.F.text = $"F:{node.movementPenalty}";
        debugText.P.text = $"P:{node.Position.x}, {node.Position.y}";

        Vector3Int diretion = node.Parent.Position - node.Position;
        debugText.arrow.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(diretion.y, diretion.x) * Mathf.Rad2Deg);
    }

    public void ColourTile(Vector3Int position, Color colour)
    {
        tilemap.SetTile(position, tile);
        //Makes sure that there are no flages controlling the tile so that it can colour the tile
        tilemap.SetTileFlags(position, TileFlags.None);
        tilemap.SetColor(position, colour);
    }

    public void Reset()
    {
        foreach (GameObject GB in _debugObjects)
        {
            Destroy(GB);
        }
        
        _debugObjects.Clear();

    }
}

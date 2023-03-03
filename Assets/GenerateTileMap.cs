#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor.SceneManagement;

[ExecuteInEditMode]
public class GenerateTileMap : MonoBehaviour
{
    public bool generateTileMap;
    public bool resetTileMap;
    public GridLayout gridLayout;
    public Tile tile;

    public BoxCollider2D boxCol;

    private Tilemap _AStarTile;
    public List<GameObject> tilemapObjs;
    public Tilemap[] TileMaps;
    public Tilemap[] midgroundTilemap;
    public List<Vector3Int> tilesPos = new List<Vector3Int>();
    public GroundType[] groundValues;
    Dictionary<int, int> _walkableRegions = new Dictionary<int, int>();
    LayerMask _walkableMask;



    // Start is called before the first frame update
    void Start()
    {
        _AStarTile = GetComponent<Tilemap>();

        foreach (GroundType type in groundValues)
        {

            _walkableMask.value |= type.groundLayer.value;
            //Gets the binary format of the layer as a key and the layer to store it in a dicnorary
            _walkableRegions.Add((int)Mathf.Log(type.groundLayer.value, 2), type.terrainPenalty);
        }
        // boxCol = GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        // on press button
        // generate tilemap that does not incluide wall and midground and its in a collider

        if (generateTileMap)
        {
            StartCoroutine(GenerateTileLocation());
        }

        if (resetTileMap)
        {
            _AStarTile.ClearAllTiles(); 
            resetTileMap = false;

            tilemapObjs.Clear();
           
            TileMaps = new Tilemap[0];
            midgroundTilemap = new Tilemap[0];
            tilesPos.Clear();
        }
    }

    public IEnumerator GenerateTileLocation()
    {
        //var tileMapChecker = Physics2D.OverlapBox(transform.position, boxColliderSize, 0);
        //Allows cross scene referencing
        EditorSceneManager.preventCrossSceneReferences = false;
        tilemapObjs.AddRange(GameObject.FindGameObjectsWithTag("Wall"));
        tilemapObjs.AddRange(GameObject.FindGameObjectsWithTag("Midground"));

        
        Vector3 gridSize = boxCol.size / 2;
        TileMaps = new Tilemap[tilemapObjs.Count];

        //Gets all the tilemaps
        for (int i = 0; i < tilemapObjs.Count; i++)
        {

            TileMaps[i] = tilemapObjs[i].GetComponent<Tilemap>();
        }



        //Stores all the tiles position

        foreach (Tilemap _tilemap in TileMaps)
        {
            
            for (int x = (int)-gridSize.x; x <= gridSize.x; x++)
            {
                for (int y = (int)-gridSize.y; y <= gridSize.y; y++)
                {

                    Vector3 targetPos = new Vector3Int(x + (int)boxCol.offset.x, y + (int)boxCol.offset.y, 0);
                    

                    if (_tilemap.GetTile(_tilemap.WorldToCell(targetPos)))
                    {

                        

                        tilesPos.Add(Vector3Int.CeilToInt(targetPos));
                    }



                    

                    //

                }
            }
          
        }


        //Have to put seperate if not it would not apply to all tilemaps

        ////Finds the empty areas
        for (int x = (int)-gridSize.x; x <= gridSize.x; x++)
        {
            for (int y = (int)-gridSize.y; y <= gridSize.y; y++)
            {

                Vector3Int targetPos = new Vector3Int(x + (int)boxCol.offset.x, y + (int)boxCol.offset.y, 0);
                Vector3 vector3Pos = new Vector3(targetPos.x, targetPos.y, targetPos.z);
                //RaycastHit2D hit = Physics2D.BoxCast(new Vector3( targetPos.x, targetPos.y), new Vector2(1.8f, 1.8f), 0, Vector2.zero, 3, LayerMask.GetMask("Wall"));
                RaycastHit2D hit = Physics2D.Raycast(vector3Pos, Vector2.down, 1f, LayerMask.GetMask("Wall"));
                RaycastHit2D hit2 = Physics2D.Raycast(vector3Pos, Vector2.up, 1f, LayerMask.GetMask("Wall"));
                //RaycastHit2D hit3 = Physics2D.Raycast(vector3Pos, Vector2.left, 1f, LayerMask.GetMask("Wall"));
                //RaycastHit2D hit4 = Physics2D.Raycast(vector3Pos, Vector2.right, 1f, LayerMask.GetMask("Wall"));

                
                ////UnityEngine.Debug.DrawLine((targetPos), vector3Pos + Vector3.down * 1, Color.red, 5);

                //if (!hit || !hit2  )
                //{
                //    //This is to not get the celing and ground of an area
                //    if(vector3Pos.y > boxCol.offset.y && vector3Pos.y - boxCol.offset.y > 1)
                //    {
                //        if (!tilesPos.Contains(targetPos))
                //        {
                //            _AStarTile.SetTile(targetPos, tile);
                //        }
                //    }
                //    else if(vector3Pos.y < boxCol.offset.y && boxCol.offset.y - vector3Pos.y > 1)
                //    {
                //        if (!tilesPos.Contains(targetPos))
                //        {
                //            _AStarTile.SetTile(targetPos, tile);
                //        }
                //    }
                    



                //}
                
                

                if (!tilesPos.Contains(targetPos))
                {
                    _AStarTile.SetTile(targetPos, tile);
                }









                //check if the target position is hit in the overlapbox collider



            }
        }

        yield return null;
        //EditorSceneManager.preventCrossSceneReferences = true;

        generateTileMap = false;
    }




    [System.Serializable]
    public class GroundType
    {
        public LayerMask groundLayer;
        public int terrainPenalty;
    }

}

#endif

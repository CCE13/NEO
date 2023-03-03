using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class MeshMaker : MonoBehaviour
{
    Sprite _sprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _sprite = GetComponent<SpriteRenderer>().sprite;
        SpriteToMesh();
    }


    Mesh SpriteToMesh()
    {
        Mesh mesh = new Mesh();
        mesh.SetVertices(Array.ConvertAll(_sprite.vertices, i => (Vector3)i).ToList());
        Debug.Log(_sprite.vertices);
        mesh.SetUVs(0, _sprite.uv.ToList());
        mesh.SetTriangles(Array.ConvertAll(_sprite.triangles, i => (int)i), 0);
        Debug.Log(mesh);
        return mesh;
    }
}

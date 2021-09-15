using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    ChunkFabric i = new ChunkFabric ();
    Chunk[] j;

    void Start()
    {
        MathU.SeedGenerator (42);
        MathU.NoiseGenerator (256);
        j = new Chunk[8];
        for (int k = 0; k < 8; k++)
        {
            j[k] = i.Create (new Vector3Int (k,0,0));
            i.GeneratePoints (ref j[k], .1f);
        }
        var l = new GameObject ("Test", typeof (MeshFilter), typeof (MeshRenderer));
        i.GenerateMesh (ref j[0]);
        l.GetComponent <MeshFilter>().mesh = j[0].TerrainMesh;
    }

    void Update()
    {
        
    }
}

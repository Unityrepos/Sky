using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    ChunkFabric i = new ChunkFabric ();
    Chunk[] j;
    public float timing;

    void Start()
    {
        MathU.SeedGenerator (42);
        MathU.NoiseGenerator (256);
        var e = 8;
        j = new Chunk[e*e*e];
        for (int k = 0; k < e; k++)
        {
            for (int n = 0; n < e; n++)
            {
                for (int x = 0; x < e; x++)
                {
                    j[k*e*e + n*e + x] = i.Create (new Vector3Int (k,n,x));
                    i.GeneratePoints (ref j[k*e*e + n*e + x], .1f);
                }
            }
        }
        
        MarchingCubesSmoothTriangles.Limit = .6f;
        var l = new GameObject [e*e*e];
        for (int c = 0; c < e*e*e; c++)
        {
            i.GenerateMesh (ref j[c]);
            l[c] = new GameObject ("Test"+c.ToString(), typeof (MeshFilter), typeof (MeshRenderer), typeof (MeshCollider));
            l[c].GetComponent <MeshFilter>().mesh = j[c].TerrainMesh;
            l[c].GetComponent <MeshCollider>().sharedMesh = j[c].TerrainMesh;
            l[c].GetComponent <MeshRenderer>().material = this.GetComponent <MeshRenderer>().material;
            
        }
    }
}

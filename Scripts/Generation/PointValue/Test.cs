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
        var e = 16;
        j = new Chunk[e*e*2];
        for (int k = 0; k < e; k++)
        {
            for (int n = 0; n < e; n++)
            {
                    j[k*e+ n] = i.Create (new Vector3Int (k,0,n));
                    i.GeneratePoints (ref j[k*e+ n], .03f);
            }
        }
        for (int k = 0; k < e; k++)
        {
            for (int n = 0; n < e; n++)
            {
                    j[k*e+ n + e*e] = i.Create (new Vector3Int (k,1,n));
                    i.GeneratePoints (ref j[k*e+ n + e*e], .03f);
            }
        }
        
        MarchingCubesSmoothTriangles.Limit = .6f;
        var l = new GameObject [e*e*2];
        for (int c = 0; c < e*e*2; c++)
        {
            i.GenerateMesh (ref j[c]);
            l[c] = new GameObject ("Test"+c.ToString(), typeof (MeshFilter), typeof (MeshRenderer), typeof (MeshCollider));
            l[c].GetComponent <MeshFilter>().mesh = j[c].TerrainMesh;
            l[c].GetComponent <MeshFilter>().mesh.RecalculateBounds ();
            l[c].GetComponent <MeshFilter>().mesh.RecalculateNormals ();
            l[c].GetComponent <MeshCollider>().sharedMesh = j[c].TerrainMesh;
            l[c].GetComponent <MeshRenderer>().material = this.GetComponent <MeshRenderer>().material;
            
        }
    }
}

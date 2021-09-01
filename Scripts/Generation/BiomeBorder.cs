using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

public class BiomeBorder : MonoBehaviour
{
    public float scale;
    private Transform tr;
    private Mesh mesh;
    private MeshCollider mc;
    private float [] heights;
    private Vector3[] vertices;
    [SerializeField]
    private Vector3 startCoord;
    private int chance = 0;
    private int allCha;

    void Start () 
    {
        mc = this.GetComponent <MeshCollider> ();
        tr = this.GetComponent <Transform> ();
        mesh = this.GetComponent <MeshFilter> ().mesh;
        heights = new float [mesh.vertices.Length];
        vertices = mesh.vertices;
        var vert = new NativeArray<Vector3> (vertices.Length, Allocator.Persistent);
        var hei = new NativeArray<float> (heights.Length, Allocator.Persistent);
        for (int i = 0; i < vertices.Length; i++)
        {
            vert [i] = vertices [i];
            hei [i] = heights [i];
        }
        var perlinJob = new PerlinJob ()
        {
            Vertices = vert,
            Heights = hei,
            StartCoord = startCoord,
            Scale = scale,
        };
        var pj = perlinJob.Schedule (vertices.Length, 0);
        pj.Complete ();
        chance = PerlinJob.Chance;
        allCha = PerlinJob.AllCha;
        for (int i = 0; i < heights.Length; i++)
        {
            vertices [i] = perlinJob.Vertices [i];
            heights [i] = perlinJob.Heights [i];
        }
        vert.Dispose ();
        hei.Dispose ();
        mesh.vertices = vertices;
        mesh.RecalculateBounds (); 
        mesh.RecalculateNormals ();
        mc.sharedMesh = mesh;
        Debug.Log((float)PerlinJob.Chance/(float)PerlinJob.AllCha);
    }
}

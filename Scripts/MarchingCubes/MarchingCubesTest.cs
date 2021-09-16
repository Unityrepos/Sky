using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingCubesTest : MonoBehaviour
{
    [Range(0,1)]
    public float[] i;
    Vector3[] k = new Vector3 [0];
    int[] l = new int [0];
    Vector2[] uio = new Vector2 [0];
    MarchingCubesSmoothTriangles j = new MarchingCubesSmoothTriangles ();
    GameObject ju;
    Mesh op;

    private void Start() 
    {
        ju = new GameObject ("Test", typeof (MeshFilter), typeof (MeshRenderer));
        op = new Mesh ();
        MarchingCubesSmoothTriangles.Limit = .5f;

        ju.GetComponent<MeshFilter> ().mesh = op;
        ju.GetComponent<MeshRenderer> ().material = this.GetComponent<MeshRenderer> ().material;
    }
    void Update()
    {
        j.Triangulate (i, out k, out l);
        uio = new Vector2 [k.Length];
        for (int i = 0; i < k.Length; i++)
        {
            uio[i] = new Vector2 (k[i].x, k[i].z);
        }
        op.vertices = k;
        op.uv = uio;
        op.triangles = l;
    }
}

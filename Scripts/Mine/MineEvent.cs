using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

public class MineEvent : MonoBehaviour
{
    Vector3[] vertices;
    float force;
    float radius;
    Vector3 point;
    NativeArray <Vector3> nVert;

    private void Start() 
    {
        vertices = this.GetComponent <MeshFilter> ().mesh.vertices;
    }

    public void Mine (float force, Vector3 point, float radius)
    {
        this.force = force;
        this.radius = radius;
        this.point = point;
        nVert = new NativeArray<Vector3> (vertices.Length, Allocator.Persistent);
        for (int i = 0; i < vertices.Length; i++)
        {
            nVert [i] = vertices [i];
        }
        StartCoroutine (JobC ());
    }
    IEnumerator JobC ()
    {
        yield return null;
        var mineJob = new MineJob ()
        {
            Vertices = nVert,
            Force = force,
            Point = point,
            Radius = radius
        };
        var j = mineJob.Schedule (vertices.Length, 0);
        j.Complete ();
        nVert.Dispose ();
    }
}

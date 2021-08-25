using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

public struct MineJob : IJobParallelFor
{
    public NativeArray<Vector3> Vertices;
    public float Force;
    public Vector3 Point;
    public float Radius;

    public void Execute (int index)
    {
        if ((Vertices[index] - Point).magnitude <= Radius)
        {
            Vertices[index] -= Vertices[index].normalized * Force;
        }
    }
}
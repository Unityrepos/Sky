using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

public class SpawnJob : IJobParallelFor
{
    public NativeArray<Vector3> Vertices;
    public NativeArray<float>   Heights;

    public void Execute (int index)
    {
        if (Heights[index] < 20)
        {

        }
        else
        {

        }
    }
}

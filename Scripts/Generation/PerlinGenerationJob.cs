using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

public struct PerlinGenerationJob : IJobParallelFor
{
    public NativeArray <Vector3> Vertices;
    public NativeArray <float> Heights;
    public int Octave;
    public Vector3 StartCoord;
    public float Scale;
    public float Power;
    public float Force;
    public float Multiplier;

    public void Execute (int index)
    {
        float max = 0;
        float height = 0;
        for (int j = 0; j < Octave; j++)
        {
            //height += PerlinNoise.Perlin3D (Vertices [index] * Scale * Mathf.Pow (2,  j) + StartCoord)  / (j + 1);
            height += (Vertices [index] * Scale * Mathf.Pow (2,  j) + StartCoord).Perlin () / Mathf.Pow (2,  j);
            max += 1/Mathf.Pow (2,  j);
        }
        height /= max;
        height = Mathf.Pow (height * Force, Power);
        Heights[index] = height;
        var p = Vertices [index].normalized * height * Multiplier;
        Vertices [index] += p;
    }
}

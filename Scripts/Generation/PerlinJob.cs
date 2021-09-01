using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

public struct PerlinJob : IJobParallelFor
{
    public NativeArray <Vector3> Vertices;
    public NativeArray <float> Heights;
    public Vector3 StartCoord;
    public float Scale;
    public static int Chance = 0;
    public static int AllCha = 0;

    public void Execute (int index)
    {
        float height = 0;
        height += PerlinNoise.Perlin3D (Vertices [index] * Scale + StartCoord);
        Heights[index] = height;
        var p = Vertices [index].normalized;
        if (height > .45f)
        {
            Vertices [index] += p * .2f;
            //Chance[9]++;
            AllCha++;
            Chance++;
        }
        else if (height > .35f)
        {
            Vertices [index] += p * .18f;
            //Chance[8]++;
            AllCha++;
        }
        else if (height > .3f)
        {
            Vertices [index] += p * .16f;
            //Chance[7]++;
            AllCha++;
        }
        else if (height > .26f)
        {
            Vertices [index] += p * .14f;
            //Chance[6]++;
            AllCha++;
        }
        else if (height > .225f)
        {
            Vertices [index] += p * .12f;
            //Chance[5]++;
            AllCha++;
        }
        else if (height > .19f)
        {
            Vertices [index] += p * .1f;
            //Chance[4]++;
            AllCha++;
        }
        else if (height > .155f)
        {
            Vertices [index] += p * .06f;
            //Chance[3]++;
            AllCha++;
        }
        else if (height > .012f)
        {
            Vertices [index] += p * .04f;
            //Chance[2]++;
            AllCha++;
        }
        else if (height > .001f)
        {
            Vertices [index] += p * .02f;
            //Chance[1]++;
            AllCha++;
        }
        else
        {
            AllCha++;
        }
    }
}

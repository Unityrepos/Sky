using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

public struct ChunkGenerationJob : IJobParallelFor
{
    public static int ChunkSize;
    public Vector3 ChunkLocation;
    public static float Scale;
    public NativeArray<float> CombinedNum;

    public void Execute (int i)
    {
        BlockGeneration (i);
    }
    private void BlockGeneration (int i)
    {
        if ((new Vector3 (ChunkLocation.x * ChunkSize + ((i - (i % ChunkSize) - (((i - (i % ChunkSize)) / ChunkSize) % ChunkSize)) / ChunkSize / ChunkSize) + .01f, ChunkLocation.y * ChunkSize + (((i - (i % ChunkSize)) / ChunkSize) % ChunkSize) + .01f, ChunkLocation.z * ChunkSize + (i % ChunkSize) + .01f) * Scale).Perlin() > .5f)
        {
            CombinedNum [i] = 1;
        }
        else 
        {
            CombinedNum [i] = 0;
        }
    }
}

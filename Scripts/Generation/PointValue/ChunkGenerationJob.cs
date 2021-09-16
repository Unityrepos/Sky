using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

public struct ChunkPointsGenerationJob : IJobParallelFor
{
    public Vector3 Position;
    public float Scale;
    public static PointFabric pointFabric;
    public NativeArray<Point> Points;
    
    public void Execute (int i)
    {
        Points[i] = pointFabric.Create(new Vector3 ((i-((((i)-i % (Chunk.Size + 1))/(Chunk.Size + 1))%(Chunk.Size + 1)))/(Chunk.Size + 1)/(Chunk.Size + 1), 
                                                    (((i)-i % (Chunk.Size + 1))/(Chunk.Size + 1))%(Chunk.Size + 1), 
                                                    (i % (Chunk.Size + 1))) * Chunk.PointSize + Position * (Chunk.Size) * Chunk.PointSize, Scale);
    }
}

public struct ChunkMeshGenerationJob : IJobParallelFor
{
    public NativeArray<Chunk> Chunks;
    public static BlockFabric BlockFabric;
    
    public void Execute (int i)
    {
        var l = new Vector3 ((i-((((i)-i % (Chunk.Size))/(Chunk.Size))%(Chunk.Size)))/(Chunk.Size)/(Chunk.Size), 
                            (((i)-i % (Chunk.Size))/(Chunk.Size))%(Chunk.Size), 
                            (i % (Chunk.Size)));
        Chunks[0] = BlockFabric.GenerateMeshValue (Chunks[0], (int)l.x, (int)l.y, (int)l.z);
    }
}

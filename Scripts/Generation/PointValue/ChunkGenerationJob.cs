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
                                                    (i % (Chunk.Size + 1))) * Chunk.PointSize + Position * (Chunk.Size + 1) * Chunk.PointSize, Scale);
    }
}

public struct ChunkMeshGenerationJob : IJobParallelFor
{
    public NativeArray<float> Points;
    public NativeArray<Vector3> Vertices;
    public NativeArray<int> Triangles;
    public static MarchingCubesSmoothVertices MarchingCubesSmoothVertices;
    public int Length;
    
    public void Execute (int i)
    {
        var l = new Vector3 ((i-((((i)-i % (Chunk.Size))/(Chunk.Size))%(Chunk.Size)))/(Chunk.Size)/(Chunk.Size), 
                            (((i)-i % (Chunk.Size))/(Chunk.Size))%(Chunk.Size), 
                            (i % (Chunk.Size)));
        var j = new float [8];
        j[0] = Points[(int)((l.x)   *(Chunk.Size + 1)*(Chunk.Size + 1) + (l.y)      *(Chunk.Size + 1) + (l.z))];
        j[1] = Points[(int)((l.x+1) *(Chunk.Size + 1)*(Chunk.Size + 1) + (l.y)      *(Chunk.Size + 1) + (l.z))];
        j[2] = Points[(int)((l.x+1) *(Chunk.Size + 1)*(Chunk.Size + 1) + (l.y+1)    *(Chunk.Size + 1) + (l.z))];
        j[3] = Points[(int)((l.x)   *(Chunk.Size + 1)*(Chunk.Size + 1) + (l.y+1)    *(Chunk.Size + 1) + (l.z))];
        j[4] = Points[(int)((l.x)   *(Chunk.Size + 1)*(Chunk.Size + 1) + (l.y)      *(Chunk.Size + 1) + (l.z+1))];
        j[5] = Points[(int)((l.x+1) *(Chunk.Size + 1)*(Chunk.Size + 1) + (l.y)      *(Chunk.Size + 1) + (l.z+1))];
        j[6] = Points[(int)((l.x+1) *(Chunk.Size + 1)*(Chunk.Size + 1) + (l.y+1)    *(Chunk.Size + 1) + (l.z+1))];
        j[7] = Points[(int)((l.x)   *(Chunk.Size + 1)*(Chunk.Size + 1) + (l.y+1)    *(Chunk.Size + 1) + (l.z+1))];
        var k = new Vector3 [0];
        MarchingCubesSmoothVertices.Triangulate (j, out k);
        for (int t = 0; t < k.Length; t++)
        {
            Vertices[Length] = k[t];
            Triangles[Length] = Length;
        }
    }
}

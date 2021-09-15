using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

public class ChunkFabric : MonoBehaviour
{
    private static float Scale;
    public Chunk Create (Vector3Int position, int size = 16, float pointSize = 1)
    {
        var i = new Chunk ();
        Chunk.Size = size;
        Chunk.PointSize = pointSize;
        i.Points = new Point[Chunk.Size+1,Chunk.Size+1,Chunk.Size+1];
        i.Position = position;
        i.TerrainMesh = new Mesh ();
        return i;
    }
    public void GeneratePoints (ref Chunk chunk, float scale = 1)
    {
        if (ChunkPointsGenerationJob.pointFabric == null)
        {
            ChunkPointsGenerationJob.pointFabric = new PointFabric ();
        }
        if (Scale == 0)
        {
            Scale = scale;
        }
        var i = new NativeArray <Point> (Chunk.Size*Chunk.Size*Chunk.Size, Allocator.Persistent);
        var j = new ChunkPointsGenerationJob ()
        {
            Position = chunk.Position,
            Scale = ChunkFabric.Scale,
            Points = i
        };
        var k = j.Schedule (Chunk.Size*Chunk.Size*Chunk.Size, 0);
        k.Complete ();
        for (int t = 0; t < Chunk.Size; t++)
        {
            for (int y = 0; y < Chunk.Size; y++)
            {
                for (int u = 0; u < Chunk.Size; u++)
                {
                    chunk.Points[t,y,u] = i[t*Chunk.Size*Chunk.Size+y*Chunk.Size+u];
                }
            }
        }
        i.Dispose();
    }
    public void GenerateMesh (ref Chunk chunk)
    {
        var i = new NativeArray<float>((Chunk.Size+1)*(Chunk.Size+1)*(Chunk.Size+1), Allocator.Persistent);
        var j = new NativeArray<Vector3>(Chunk.Size*Chunk.Size*Chunk.Size*12, Allocator.Persistent);
        var k = new NativeArray<int>(Chunk.Size*Chunk.Size*Chunk.Size*12, Allocator.Persistent);

        for (int g = 0; g < (Chunk.Size+1); g++)
        {
            for (int h = 0; h < (Chunk.Size+1); h++)
            {
                for (int b = 0; b < (Chunk.Size+1); b++)
                {
                    i[g * (Chunk.Size+1) * (Chunk.Size+1) + h * (Chunk.Size+1) + b] = chunk.Points[g,h,b].Value;
                }
            }
        }
        ChunkMeshGenerationJob.MarchingCubesSmoothVertices = new MarchingCubesSmoothVertices ();
        var y = new ChunkMeshGenerationJob ()
        {
            Points = i,
            Vertices = j,
            Triangles = k,
            Length = 0
        };
        var v = y.Schedule((Chunk.Size)*(Chunk.Size)*(Chunk.Size), 0);
        v.Complete ();
        var z = new Vector3[y.Length];
        var x = new int[y.Length];
        for (int m = 0; m < y.Length; m++)
        {
            z[m] = y.Vertices[m];
            x[m] = y.Triangles[m];
        }
        i.Dispose ();
        j.Dispose ();
        k.Dispose ();
        chunk.TerrainMesh.vertices = z;
        chunk.TerrainMesh.triangles = x;
    }
}

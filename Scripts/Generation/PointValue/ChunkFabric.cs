using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

public class ChunkFabric
{
    private static float Scale;
    private static BlockFabric blockFabric = new BlockFabric ();
    public Chunk Create (Vector3Int position, int size = 16, float pointSize = 1)
    {
        var i = new Chunk ();
        Chunk.Size = size;
        Chunk.PointSize = pointSize;
        i.Points = new Point[size+1,size+1,size+1];
        i.Blocks = new Block[size,size,size];
        for (int r = 0; r < size; r++)
        {
            for (int t = 0; t < size; t++)
            {
                for (int y = 0; y < size; y++)
                {
                    i.Blocks[r,t,y] = blockFabric.Create ((Vector3)(position)*size*pointSize + new Vector3 (r + .5f, t + .5f, y + .5f));
                }
            }
        }
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
        var i = new NativeArray <Point> ((Chunk.Size+1)*(Chunk.Size+1)*(Chunk.Size+1), Allocator.Persistent);
        var j = new ChunkPointsGenerationJob ()
        {
            Position = chunk.Position,
            Scale = ChunkFabric.Scale,
            Points = i
        };
        var k = j.Schedule ((Chunk.Size+1)*(Chunk.Size+1)*(Chunk.Size+1), 0);
        k.Complete ();
        for (int t = 0; t < (Chunk.Size+1); t++)
        {
            for (int y = 0; y < (Chunk.Size+1); y++)
            {
                for (int u = 0; u < (Chunk.Size+1); u++)
                {
                    chunk.Points[t,y,u] = i[t*(Chunk.Size+1)*(Chunk.Size+1)+y*(Chunk.Size+1)+u];
                }
            }
        }
        i.Dispose();
    }
    // public JobHandle GenerateMeshInBlock (ref Chunk chunk)
    // {
    //     ChunkMeshGenerationJob.BlockFabric = new BlockFabric ();
    //     var n = new NativeArray<Chunk>(1, Allocator.Persistent);
    //     var y = new ChunkMeshGenerationJob () {Chunks = n};
    //     return y.Schedule (Chunk.Size * Chunk.Size * Chunk.Size, 0);
    // }
    // public void GenerateMeshFromBlock (ref Chunk chunk)
    // {
    //     var l = new List<Vector3>();
    //     for (int i = 0; i < Chunk.Size; i++)
    //     {
    //         for (int j = 0; j < Chunk.Size; j++)
    //         {
    //             for (int k = 0; k < Chunk.Size; k++)
    //             {
    //                 l.AddRange (chunk.Blocks[i,j,k].Vertices);
    //             }
    //         }
    //     }
    //     var r = l.ToArray ();
    //     var t = new int [r.Length];
    //     for (int u = 0; u < r.Length; u++)
    //     {
    //         t[u] = u;
    //     }
    //     chunk.TerrainMesh.vertices = r;
    //     chunk.TerrainMesh.triangles = t;
    // }
    // public void GenerateMesh (ref Chunk chunk)
    // {
    //     GenerateMeshInBlock (ref chunk).Complete();
    //     GenerateMeshFromBlock (ref chunk);
    // }
    public void GenerateMesh (ref Chunk chunk)
    {
        var l = new List<Vector3>();
        for (int i = 0; i < Chunk.Size; i++)
        {
            for (int j = 0; j < Chunk.Size; j++)
            {
                for (int k = 0; k < Chunk.Size; k++)
                {
                    blockFabric.GenerateMeshValue (ref chunk,i,j,k);
                    l.AddRange (chunk.Blocks[i,j,k].Vertices);
                }
            }
        }
        var r = l.ToArray ();
        //var h = new Vector2 [r.Length];
        var t = new int [r.Length];
        for (int u = 0; u < r.Length; u++)
        {
            //h[u] = new Vector2 (r[u].x, r[u].z);
            t[u] = u;
        }
        chunk.TerrainMesh.vertices = r;
        chunk.TerrainMesh.triangles = t;
        //chunk.TerrainMesh.uv = h;
    }
}

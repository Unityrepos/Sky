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
        var i = new NativeArray <Point> ((Chunk.Size+1)*(Chunk.Size+1)*(Chunk.Size+1), Allocator.TempJob);
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
    public void GenerateMesh (ref Chunk chunk)
    {
        var verticesList = new List<Vector3>();
        for (int i = 0; i < Chunk.Size; i++)
        {
            for (int j = 0; j < Chunk.Size; j++)
            {
                for (int k = 0; k < Chunk.Size; k++)
                {
                    verticesList.AddRange (blockFabric.GenerateMeshValue (ref chunk,i,j,k));
                }
            }
        }
        chunk.TerrainMesh.vertices = verticesList.ToArray ();
        var vetircesListLength = chunk.TerrainMesh.vertices.Length;
        var trianglesList = new int [vetircesListLength];
        chunk.TerrainMesh.uv = new Vector2[vetircesListLength];
        for (int u = 0; u < vetircesListLength; u++)
        {
            trianglesList[u] = u;
        }
        chunk.TerrainMesh.triangles = trianglesList;
    }
}

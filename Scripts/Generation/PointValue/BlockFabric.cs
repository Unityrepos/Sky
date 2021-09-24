using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class BlockFabric
{
    MarchingCubesSmoothTriangles marchingCubes = new MarchingCubesSmoothTriangles ();

    public Block Create (Vector3 position)
    {
        var i = new Block ();
        i.Position = position;
        i.Vertices = new Vector3 [0];
        i.Triangles = new int [0];
        return i;
    }
    public void GenerateMeshValue (ref Chunk chunk, int x, int y, int z)
    {
        var i = new float[8];
        i[0] = chunk.Points[x,  y,  z].     Value;
        i[1] = chunk.Points[x+1,y,  z].     Value;
        i[2] = chunk.Points[x+1,y,z+1].     Value;
        i[3] = chunk.Points[x,  y,z+1].     Value;
        i[4] = chunk.Points[x,  y+1,  z].   Value;
        i[5] = chunk.Points[x+1,y+1,  z].   Value;
        i[6] = chunk.Points[x+1,y+1,z+1].   Value;
        i[7] = chunk.Points[x,  y+1,z+1].   Value;
        //var f = new Vector3[0];
        var g = new int[0];
        marchingCubes.Triangulate (i, out chunk.Blocks[x,y,z].Vertices, out g);//out f, out g);
        //chunk.Blocks[x,y,z].Vertices = f;
        for (int l = 0; l < chunk.Blocks[x,y,z].Vertices.Length; l++)
        {
            chunk.Blocks[x,y,z].Vertices[l] += chunk.Blocks[x,y,z].Position;
        }
    }
    public Chunk GenerateMeshValue (Chunk chunk, int x, int y, int z)
    {
        var i = new float[8];
        i[0] = chunk.Points[x,  y,  z].     Value;
        i[1] = chunk.Points[x+1,y,  z].     Value;
        i[2] = chunk.Points[x+1,y,z+1].     Value;
        i[3] = chunk.Points[x,  y,z+1].     Value;
        i[4] = chunk.Points[x,  y+1,  z].   Value;
        i[5] = chunk.Points[x+1,y+1,  z].   Value;
        i[6] = chunk.Points[x+1,y+1,z+1].   Value;
        i[7] = chunk.Points[x,  y+1,z+1].   Value;
        var f = new Vector3[0];
        var g = new int[0];
        marchingCubes.Triangulate (i, out f, out g);
        chunk.Blocks[x,y,z].Vertices = f;
        for (int l = 0; l < chunk.Blocks[x,y,z].Vertices.Length; l++)
        {
            chunk.Blocks[x,y,z].Vertices[l] += chunk.Blocks[x,y,z].Position;
        }
        return chunk;
    }
    // public void GenerateMeshValue (int chunk, int x, int y, int z)
    // {
    //     var i = new float[8];
    //     i[0] = chunkArray[chunk].Points[x,  y,  z].     Value;
    //     i[1] = chunkArray[chunk].Points[x+1,y,  z].     Value;
    //     i[2] = chunkArray[chunk].Points[x+1,y,z+1].     Value;
    //     i[3] = chunkArray[chunk].Points[x,  y,z+1].     Value;
    //     i[4] = chunkArray[chunk].Points[x,  y+1,  z].   Value;
    //     i[5] = chunkArray[chunk].Points[x+1,y+1,  z].   Value;
    //     i[6] = chunkArray[chunk].Points[x+1,y+1,z+1].   Value;
    //     i[7] = chunkArray[chunk].Points[x,  y+1,z+1].   Value;
    //     var f = new Vector3[0];
    //     var g = new int[0];
    //     marchingCubes.Triangulate (i, out f, out g);
    //     chunkArray[chunk].Blocks[x,y,z].Vertices = f;
    //     for (int l = 0; l < chunkArray[chunk].Blocks[x,y,z].Vertices.Length; l++)
    //     {
    //         chunkArray[chunk].Blocks[x,y,z].Vertices[l] += chunkArray[chunk].Blocks[x,y,z].Position;
    //     }
    // }
}

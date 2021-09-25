using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class BlockFabric
{
    MarchingCubesSmoothVertices marchingCubes = new MarchingCubesSmoothVertices ();

    public Vector3[] GenerateMeshValue (ref Chunk chunk, int x, int y, int z)
    {
        Vector3[] output;
        var i = new float[8];
        i[0] = chunk.Points[x,  y,  z].     Value;
        i[1] = chunk.Points[x+1,y,  z].     Value;
        i[2] = chunk.Points[x+1,y,z+1].     Value;
        i[3] = chunk.Points[x,  y,z+1].     Value;
        i[4] = chunk.Points[x,  y+1,  z].   Value;
        i[5] = chunk.Points[x+1,y+1,  z].   Value;
        i[6] = chunk.Points[x+1,y+1,z+1].   Value;
        i[7] = chunk.Points[x,  y+1,z+1].   Value;
        marchingCubes.Triangulate (i, out output);
        for (int l = 0; l < output.Length; l++)
        {
            output[l] += (Vector3)chunk.Position * Chunk.PointSize * Chunk.Size + new Vector3 (x + .5f, y + .5f, z + .5f);
        }
        return output;
    }
}

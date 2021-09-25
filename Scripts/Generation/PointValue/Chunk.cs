using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public Vector3Int Position;
    public static int Size;
    public static float PointSize;
    public Point[,,] Points;
    public Mesh TerrainMesh;
    public bool IsEmpty
    {
        get
        {
            foreach (var j in Points)
            {
                if (j.Value != 0)
                    return false;
            }
            return true;
        }
    }
}

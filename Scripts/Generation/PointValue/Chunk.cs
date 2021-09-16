using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Chunk
{
    public Vector3Int Position;
    public static int Size;
    public static float PointSize;
    public Point[,,] Points;
    public Block[,,] Blocks;
    public Mesh TerrainMesh;
}

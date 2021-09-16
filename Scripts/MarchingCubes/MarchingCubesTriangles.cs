using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingCubesTriangles : MarchingCubesVertices
{
    public void Triangulate (bool[] corners, out Vector3[] outputVertices, out int [] outputTriangles)
    {
        Triangulate (corners, out outputVertices);
        outputTriangles = new int [outputVertices.Length];
        for (int i = 0; i < outputTriangles.Length; i++)
        {
            outputTriangles[i] = i;
        }
    }
}

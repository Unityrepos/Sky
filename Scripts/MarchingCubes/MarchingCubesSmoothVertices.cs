using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingCubesSmoothVertices : MarchingCubesVertices
{
    private static float limit;

    public static float Limit { set => limit = Mathf.Clamp(value, 0, 1); }

    public void Triangulate (float[] corners, out Vector3[] outputVertices)
    {
        bool[] isCorners = new bool[8];
        for (int i = 0; i < isCorners.Length; i++)
        {
            isCorners[i] = corners[i] > limit;
        }
        var index = CornersToInt(isCorners);
        var arraySize = 0;
        for (int i = 0; i < 16; i++)
        {
            if (triangulationTable [index, i] != -1)
            {
                arraySize ++;
            }
        }
        outputVertices = new Vector3 [arraySize];
        var j = 0;
        for (int i = 0; i < 16; i++)
        {
            if (triangulationTable [index, i] != -1)
            {
                outputVertices[j] = TriangulationToVector (triangulationTable [index, i], corners);
                j++;
            }
        }
    }
    private Vector3 TriangulationToVector (int i, in float[] corners)
    {
        Vector3 output;
        switch (i)
        {
            case 0:
            output = new Vector3 ((limit - corners[0]) / (corners[1] - corners[0]), 0, 0);
            break;
            case 1:
            output = new Vector3 (1, 0, (limit - corners[1]) / (corners[2] - corners[1]));
            break;
            case 2:
            output = new Vector3 ((limit - corners[3]) / (corners[2] - corners[3]), 0, 1);
            break;
            case 3:
            output = new Vector3 (0, 0, (limit - corners[0]) / (corners[3] - corners[0]));
            break;
            case 4:
            output = new Vector3 ((limit - corners[4]) / (corners[5] - corners[4]), 1, 0);
            break;
            case 5:
            output = new Vector3 (1, 1, (limit - corners[5]) / (corners[6] - corners[5]));
            break;
            case 6:
            output = new Vector3 ((limit - corners[7]) / (corners[6] - corners[7]), 1, 1);
            break;
            case 7:
            output = new Vector3 (0, 1, (limit - corners[4]) / (corners[7] - corners[4]));
            break;
            case 8:
            output = new Vector3 (0f, (limit - corners[0]) / (corners[4] - corners[0]), 0);
            break;
            case 9:
            output = new Vector3 (1, (limit - corners[1]) / (corners[5] - corners[1]), 0);
            break;
            case 10:
            output = new Vector3 (1, (limit - corners[2]) / (corners[6] - corners[2]), 1);
            break;
            case 11:
            output = new Vector3 (0, (limit - corners[3]) / (corners[7] - corners[3]), 1);
            break;
            default:
            output = new Vector3 (0,0,0);
            break;
        }
        return output;
    }
}

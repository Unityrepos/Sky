using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointFabric
{
    public Point Create (Vector3 position, float scale)
    {
        var i = new Point ();
        i.Position = position;
        i.Value = GenerateValue (position, scale, 10);//().Perlin ();
        return i;
    }
    private float GenerateValue (Vector3 point, float scale, int octaves)
    {
        float i = 0;
        float y = 0;
        for (int u = 0; u < octaves; u++)
        {
            i += Mathf.PerlinNoise ((point.x + .01f) * scale * Mathf.Pow (2, u), (point.z+ .01f) * scale * Mathf.Pow (2, u)) / Mathf.Pow (2, u);
            y += 1 / Mathf.Pow (2, u);
        }
        i /= y;
        i *= 128;
        i -= point.y;
        return i;
    }
}

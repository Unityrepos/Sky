using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointFabric
{
    static public float BiomeSize = 1;
    public Point Create (Vector3 position, float scale)
    {
        var i = new Point ();
        //i.Position = position;
        i.Value = GenerateValue (position, scale, 7);
        return i;
    }
    private float GenerateValue (Vector3 point, float scale, int octaves)
    {
        float i = 0;
        float y = 0;
        // var n = Vector2.one / 1000;
        var biomeNoise = MathU.EndSmooth (Mathf.PerlinNoise ((point.x + .01f) * scale * BiomeSize, (point.z+ .01f) * scale * BiomeSize));
        var octaveNoise = MathU.EndSmooth (Mathf.PerlinNoise ((point.x + 1000.01f) * scale * BiomeSize, (point.z+ 1000.01f) * scale * BiomeSize));
        octaveNoise = octaveNoise < biomeNoise ? biomeNoise : octaveNoise;
        for (int u = 0; u < (octaveNoise) * (octaves); u++)
        {
            i += Mathf.PerlinNoise ((point.x + .01f) * scale * Mathf.Pow (2, u), (point.z+ .01f) * scale * Mathf.Pow (2, u)) / Mathf.Pow (2, u);
            y += 1 / Mathf.Pow (2, u);
        }
        i /= y;
        i *= 128 * biomeNoise;
        i -= point.y;
        return Mathf.Clamp (i, 0, 1);
    }
}

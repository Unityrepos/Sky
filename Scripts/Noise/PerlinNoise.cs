using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PerlinNoise 
{
    private static Vector2 [ , ] vectors;
    private static int length;
    private static bool haveNoise;
    public static void NoiseGenerator (int length)
    {
        if (!haveNoise)
        {
            haveNoise = true;
            PerlinNoise.length = length;
            vectors = new Vector2 [length, length];
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    vectors [i, j] = new Vector2 (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
                }
            }
        }
    }
    
    private static float Berp (float a, float b, float c, float d, Vector2 t)
    {
        t = new Vector2 (t.x * t.x * (3 - 2 * t.x), t.y * t.y * (3 - 2 * t.y));
        return Mathf.Lerp (Mathf.Lerp (a, b, t.y), Mathf.Lerp (c, d, t.y), t.x);
    }
    private static float Scalar (Vector2 a, Vector2 b)
    {
        return a.x * b.x + a.y * b.y;
    }
    public static float Perlin (this Vector2 position)
    {
        var positionL = new Vector2 ((position.x + length) % length, (position.y + length) % length);
        var first = Scalar (new Vector2 (positionL.x - Mathf.Floor(positionL.x), positionL.y - Mathf.Floor(positionL.y)), vectors [((int)Mathf.Floor(positionL.x) % length + length) % length, ((int)Mathf.Floor(positionL.y) % length + length) % length]);
        var second = Scalar (new Vector2 (positionL.x - Mathf.Floor(positionL.x), positionL.y - Mathf.Ceil(positionL.y)), vectors [((int)Mathf.Floor(positionL.x) % length + length) % length, ((int)Mathf.Ceil(positionL.y) % length + length) % length]);
        var third = Scalar (new Vector2 (positionL.x - Mathf.Ceil(positionL.x), positionL.y - Mathf.Floor(positionL.y)), vectors [((int)Mathf.Ceil(positionL.x) % length + length) % length, ((int)Mathf.Floor(positionL.y) % length + length) % length]);
        var fourth = Scalar (new Vector2 (positionL.x - Mathf.Ceil(positionL.x), positionL.y - Mathf.Ceil(positionL.y)), vectors [((int)Mathf.Ceil(positionL.x) % length + length) % length, ((int)Mathf.Ceil(positionL.y) % length + length) % length]);
        return Berp (first, second, third, fourth, new Vector2 (((position.x % 1) + 1) % 1, ((position.y % 1) + 1) % 1));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PerlinNoise3D
{
    private static Vector3 [ , , ] vectors;
    private static int length;
    private static bool haveNoise;
    private static bool haveSeed;

    public static void SeedGenerator (int seed)
    {
        if (!haveSeed)
        {
            haveSeed = true;
            Random.seed = seed;
        }
    }
    public static void NoiseGenerator (int length)
    {
        if (!haveNoise)
        {
            haveNoise = true;
            PerlinNoise3D.length = length;
            vectors = new Vector3 [length, length, length];
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    for (int k = 0; k < length; k++)
                    {
                        vectors [i, j, k] = new Vector3 (Random.value, Random.value, Random.value);
                    }
                }
            }
        }
    }
    private static float Terp (float a, float b, float c, float d, float e, float f, float g, float h, Vector3 t)
    {
        t = new Vector3 (t.x * t.x * (3 - 2 * t.x), t.y * t.y * (3 - 2 * t.y), t.z * t.z * (3 - 2 * t.z));
        return Mathf.Lerp (Mathf.Lerp (Mathf.Lerp (a, b, t.y), Mathf.Lerp (c, d, t.y), t.x), Mathf.Lerp (Mathf.Lerp (e, f, t.y), Mathf.Lerp (g, h, t.y), t.x), t.z);
    }
    private static float Scalar (Vector3 a, Vector3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }
    public static float Perlin (this Vector3 position)
    {
        var positionL = new Vector3 ((position.x + length) % length, (position.y + length) % length, (position.z + length) % length);
        var first = Scalar (new Vector3 (positionL.x - Mathf.Floor(positionL.x), positionL.y - Mathf.Floor(positionL.y), positionL.z - Mathf.Floor(positionL.z)), vectors [((int)Mathf.Floor(positionL.x) % length + length) % length, ((int)Mathf.Floor(positionL.y) % length + length) % length, ((int)Mathf.Floor(positionL.z) % length + length) % length]);
        var second = Scalar (new Vector3 (positionL.x - Mathf.Floor(positionL.x), positionL.y - Mathf.Ceil(positionL.y), positionL.z - Mathf.Floor(positionL.z)), vectors [((int)Mathf.Floor(positionL.x) % length + length) % length, ((int)Mathf.Ceil(positionL.y) % length + length) % length, ((int)Mathf.Floor(positionL.z) % length + length) % length]);
        var third = Scalar (new Vector3 (positionL.x - Mathf.Ceil(positionL.x), positionL.y - Mathf.Floor(positionL.y), positionL.z - Mathf.Floor(positionL.z)), vectors [((int)Mathf.Ceil(positionL.x) % length + length) % length, ((int)Mathf.Floor(positionL.y) % length + length) % length, ((int)Mathf.Floor(positionL.z) % length + length) % length]);
        var fourth = Scalar (new Vector3 (positionL.x - Mathf.Ceil(positionL.x), positionL.y - Mathf.Ceil(positionL.y), positionL.z - Mathf.Floor(positionL.z)), vectors [((int)Mathf.Ceil(positionL.x) % length + length) % length, ((int)Mathf.Ceil(positionL.y) % length + length) % length, ((int)Mathf.Floor(positionL.z) % length + length) % length]);
        var fifth = Scalar (new Vector3 (positionL.x - Mathf.Floor(positionL.x), positionL.y - Mathf.Floor(positionL.y), positionL.z - Mathf.Ceil(positionL.z)), vectors [((int)Mathf.Floor(positionL.x) % length + length) % length, ((int)Mathf.Floor(positionL.y) % length + length) % length, ((int)Mathf.Ceil(positionL.z) % length + length) % length]);
        var sixth = Scalar (new Vector3 (positionL.x - Mathf.Floor(positionL.x), positionL.y - Mathf.Ceil(positionL.y), positionL.z - Mathf.Ceil(positionL.z)), vectors [((int)Mathf.Floor(positionL.x) % length + length) % length, ((int)Mathf.Ceil(positionL.y) % length + length) % length, ((int)Mathf.Ceil(positionL.z) % length + length) % length]);
        var seventh = Scalar (new Vector3 (positionL.x - Mathf.Ceil(positionL.x), positionL.y - Mathf.Floor(positionL.y), positionL.z - Mathf.Ceil(positionL.z)), vectors [((int)Mathf.Ceil(positionL.x) % length + length) % length, ((int)Mathf.Floor(positionL.y) % length + length) % length, ((int)Mathf.Ceil(positionL.z) % length + length) % length]);
        var eighth = Scalar (new Vector3 (positionL.x - Mathf.Ceil(positionL.x), positionL.y - Mathf.Ceil(positionL.y), positionL.z - Mathf.Ceil(positionL.z)), vectors [((int)Mathf.Ceil(positionL.x) % length + length) % length, ((int)Mathf.Ceil(positionL.y) % length + length) % length, ((int)Mathf.Ceil(positionL.z) % length + length) % length]);
        return Terp (first, second, third, fourth, fifth, sixth, seventh, eighth, new Vector3 (((position.x % 1) + 1) % 1, ((position.y % 1) + 1) % 1, ((position.z % 1) + 1) % 1));
    }
}

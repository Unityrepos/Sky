using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class MathU
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
            MathU.length = length;
            vectors = new Vector3 [length, length, length];
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    for (int k = 0; k < length; k++)
                    {
                        vectors [i, j, k] = new Vector3 (Random.value * 2 - 1, Random.value * 2 - 1, Random.value * 2 - 1);
                    }
                }
            }
        }
    }
    private static float TerpSmooth (float a, float b, float c, float d, float e, float f, float g, float h, Vector3 t)
    {
        t = new Vector3 (t.x * t.x * (3 - 2 * t.x), t.y * t.y * (3 - 2 * t.y), t.z * t.z * (3 - 2 * t.z));
        return Mathf.Lerp (Berp (a, b, c, d, new Vector2 (t.y, t.z)), Berp (e, f, g, h, new Vector2 (t.y, t.z)), t.x);
    }
    private static float Terp (float a, float b, float c, float d, float e, float f, float g, float h, Vector3 t)
    {
        return Mathf.Lerp (Berp (a, b, c, d, new Vector2 (t.y, t.z)), Berp (e, f, g, h, new Vector2 (t.y, t.z)), t.x);
    }
    private static float Scalar (Vector3 a, Vector3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }
    private static float Border (float a, int b)
    {
        return ((a % b) + b) % b;
    }
    private static float Border (float a)
    {
        return ((a % length) + length) % length;
    }
    private static Vector3 Border (Vector3 a, int b)
    {
        return new Vector3 (Border (a.x, b), Border (a.y, b), Border (a.z, b));
    }
    private static Vector3 Border (Vector3 a)
    {
        return new Vector3 (Border (a.x, length), Border (a.y, length), Border (a.z, length));
    }
    public static float Perlin (this Vector3 position)
    {
        var positionB = Border (position);
        var positionF = new Vector3 (Mathf.Floor (positionB.x), Mathf.Floor (positionB.y), Mathf.Floor (positionB.z));
        var positionC = new Vector3 (Mathf.Ceil (positionB.x), Mathf.Ceil (positionB.y), Mathf.Ceil (positionB.z));
        var points = new float [2, 2, 2];
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    var mainDirection =
                    points [i, j, k] = Scalar (new Vector3 (positionB.x - (i == 0 ? positionF.x : positionC.x), 
                                                            positionB.y - (j == 0 ? positionF.y : positionC.y), 
                                                            positionB.z - (k == 0 ? positionF.z : positionC.z)), 
                    vectors [   (int)(Border((i == 0) ? positionF.x : positionC.x)), 
                                (int)(Border((j == 0) ? positionF.y : positionC.y)), 
                                (int)(Border((k == 0) ? positionF.z : positionC.z))]);
                }
            }
        }
        return TerpSmooth (points[0, 0, 0], points[0, 0, 1], points[0, 1, 0], points[0, 1, 1], points[1, 0, 0], points[1, 0, 1], points[1, 1, 0], points[1, 1, 1], Border (position, 1))/1.5f + .5f;
    }
}

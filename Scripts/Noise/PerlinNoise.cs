using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class MathU 
{
    private static float BerpSmooth (float a, float b, float c, float d, Vector2 t)
    {
        t = new Vector2 (t.x * t.x * (3 - 2 * t.x), t.y * t.y * (3 - 2 * t.y));
        return Mathf.Lerp (Mathf.Lerp (a, b, t.y), Mathf.Lerp (c, d, t.y), t.x);
    }
    private static float Berp (float a, float b, float c, float d, Vector2 t)
    {
        return Mathf.Lerp (Mathf.Lerp (a, b, t.y), Mathf.Lerp (c, d, t.y), t.x);
    }
    private static float Scalar (Vector2 a, Vector2 b)
    {
        return a.x * b.x + a.y * b.y;
    }
    public static float EndSmooth (float t)
    {
        return ((3 * Mathf.Pow (t, 14/8)) - 2 * (Mathf.Pow (t, 21/8)));
    }
    public static float Perlin (this Vector2 position)
    {
        var positionL = new Vector2 (Border(position.x), Border(position.y)).normalized;
        var first = Scalar (new Vector2 (   positionL.x - Mathf.Floor(positionL.x), 
                                            positionL.y - Mathf.Floor(positionL.y)), 
        new Vector2 (   vectors [((int)Border(Mathf.Floor(positionL.x))), ((int)Border(Mathf.Floor(positionL.y))), 0].x, 
                        vectors [((int)Border(Mathf.Floor(positionL.x))), ((int)Border(Mathf.Floor(positionL.x))), 0].y));
        var second = Scalar (new Vector2 (  positionL.x - Mathf.Floor(positionL.x), 
                                            positionL.y - Mathf.Ceil(positionL.y)), 
        new Vector2 (   vectors [((int)Border(Mathf.Floor(positionL.x))), ((int)Border(Mathf.Ceil(positionL.x))), 0].x, 
                        vectors [((int)Border(Mathf.Floor(positionL.x))), ((int)Border(Mathf.Ceil(positionL.x))), 0].y));
        var third = Scalar (new Vector2 (   positionL.x - Mathf.Ceil(positionL.x), 
                                            positionL.y - Mathf.Floor(positionL.y)), 
        new Vector2 (   vectors [((int)Border(Mathf.Ceil(positionL.x))), ((int)Border(Mathf.Floor(positionL.x))), 0].x, 
                        vectors [((int)Border(Mathf.Ceil(positionL.x))), ((int)Border(Mathf.Floor(positionL.x))), 0].y));
        var fourth = Scalar (new Vector2 (  positionL.x - Mathf.Ceil(positionL.x), 
                                            positionL.y - Mathf.Ceil(positionL.y)), 
        new Vector2 (   vectors [((int)Border(Mathf.Ceil(positionL.x))), ((int)Border(Mathf.Ceil(positionL.x))), 0].x, 
                        vectors [((int)Border(Mathf.Ceil(positionL.x))), ((int)Border(Mathf.Ceil(positionL.x))), 0].y));
        return BerpSmooth (first, second, third, fourth, new Vector2 (Border(position.x, 1), Border(position.y, 1)));
    }
}

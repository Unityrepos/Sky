using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointFabric
{
    public Point Create (Vector3 position, float scale)
    {
        var i = new Point ();
        i.Position = position;
        i.Value = ((position * scale) + new Vector3(.01f,.01f,.01f)).Perlin ();
        return i;
    }
}

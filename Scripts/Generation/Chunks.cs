using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunks
{
    private Dictionary <Vector3Int, Chunk> chunks = new Dictionary<Vector3Int, Chunk> ();

    public void AddChunk (Vector3Int position, Chunk chunk)
    {
        if (!chunks.ContainsKey (position))
        {
            chunks.Add (position, chunk);
        }
    }
    public void Chunk (Vector3Int position, ref Chunk chunk)
    {
        if (chunks.ContainsKey (position))
        {
            chunk = chunks [position];
        }
    }
    public bool Contains (Vector3Int position)
    {
        return chunks.ContainsKey (position);
    }
}

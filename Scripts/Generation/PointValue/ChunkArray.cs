using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkArray
{
    private Chunk[] chunkArray = new Chunk[1];
    private int counter = 0;

    public void Add (Chunk chunk)
    {
        if (counter==chunkArray.Length)
        {
            var t = chunkArray;
            chunkArray = new Chunk [t.Length * 2];
            for (int i = 0; i < t.Length; i++)
            {
                chunkArray[i] = t[i];
            }
        }
        chunkArray[counter] = chunk;
    }
    public Chunk this [int i]
    {
        get
        {
            return chunkArray[i];
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    Air = 0,
    Grass = 1
}
public struct Point
{
    private byte blockType;
    public float Value;
    public BlockType BlockType
    {
        set
        {
            blockType = (byte)value;
        }
        get
        {
            return (BlockType)blockType;
        }
    }
}

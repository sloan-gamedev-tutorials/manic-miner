using UnityEngine;
using System.Collections;

public class MovableObject
{
    int startFrame;
    int startX;
    int startY;

    public byte Attribute { get; set; }

    public int Left { get; set; }

    public int Right { get; set; }

    public int Frame { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public int FrameDirection { get; set; }
}

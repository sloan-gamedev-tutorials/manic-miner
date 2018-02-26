using System.Collections.Generic;

public class MinerWilly : MovableObject
{
    public List<byte[]> Frames { get; private set; }

    public MinerWilly(List<byte[]> frames, int startX, int startY, int left, int right, int startFrame, byte attr)
    {
        Attribute = attr;
        Frame = startFrame; 
        Left = left;
        Right = right;

        X = startX;
        Y = startY;

        Frames = new List<byte[]>();
        Frames.AddRange(frames);
    }
}

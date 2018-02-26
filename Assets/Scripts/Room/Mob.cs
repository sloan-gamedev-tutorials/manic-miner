public class Mob : MovableObject
{
    public Mob(HorizontalGuardian g)
    {
        Attribute = g.Attribute;
        //startX = g.StartX;
        //startY = g.StartY;
        //startFrame = g.StartFrame;
        Frame = g.StartFrame; // startFrame;
        Left = g.Left;
        Right = g.Right;

        X = g.StartX;
        Y = g.StartY;
    }
}

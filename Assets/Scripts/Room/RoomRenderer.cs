using Com.SloanKelly.ZXSpectrum;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpectrumScreen))]
public class RoomRenderer : MonoBehaviour
{
    SpectrumScreen screen;
    
    public Transform target;

    [Tooltip("The material that has the pixel perfect check box")]
    public Material pixelPerfect;   

    public Sprite minerStartTemp; // TODO: REMOVE THIS FOR PRODUCTION

    public Sprite roomKeyTemp;
    
    void Start()
    {
        screen = GetComponent<SpectrumScreen>();
        var sr = target.GetComponent<SpriteRenderer>();
        sr.sprite = Sprite.Create(screen.Texture, new Rect(0, 0, 256, 192), new Vector2(0, 1), 1f);
    }

    public void DrawScreen(RoomData data, MinerWilly minerWilly, IList<Mob> mobs, string playerScore)
    {
        screen.Clear(7, 0, false);

        DrawMinerWilly(minerWilly, data);
        DrawRoom(data);
        DrawItems(data);
        DrawHorizontalGuardians(mobs, data);
        DrawPortal(data); // Make this the last thing drawn in the room
        DrawRoomTitle(data);
        DrawAirSupply(data);
        DrawScore(playerScore);
    }

    private void DrawMinerWilly(MinerWilly m, RoomData data)
    {
        int attr = data.Attributes[m.Y * 32 + m.X];
        attr &= 0xF8; // XXXXX--- - bit pattern
        attr |= 7;// Miner Willy is always white on whatever background we have

        ZXAttribute attribute = new ZXAttribute((byte)attr);

        screen.FillAttribute(m.X, m.Y, 2, 2, attribute);
        screen.DrawSprite(m.X, m.Y, 2, 2, m.Frames[m.Frame]);
    }

    private void DrawScore(string playerScore)
    {
        // Draw the score
        for (int x = 0; x < 32; x++)
            screen.SetAttribute(x, 19, 6, 0);
        screen.PrintMessage(0, 19, playerScore);
    }

    private void DrawAirSupply(RoomData data)
    {
        // Draw the air supply
        for (int x = 0; x < 10; x++)
            screen.SetAttribute(x, 17, 7, 2);

        for (int x = 10; x < 32; x++)
            screen.SetAttribute(x, 17, 7, 4);

        byte[] airBlock = new byte[] { 0, 0, 255, 255, 255, 255, 0, 0 };

        var airSupplyLength = data.AirSupply.Length;
        var airHead = data.AirSupply.Tip;

        for (int x = 0; x < airSupplyLength; x++)
            screen.DrawSprite(x + 4, 17, 1, 1, airBlock);

        byte[] airTipBlock = new byte[] { 0, 0, (byte)airHead, (byte)airHead, (byte)airHead, (byte)airHead, 0, 0 };
        screen.DrawSprite(4 + airSupplyLength, 17, 1, 1, airTipBlock);

        screen.PrintMessage(0, 17, "AIR");
    }

    private void DrawRoomTitle(RoomData data)
    {
        // Draw the room title
        for (int x = 0; x < 32; x++)
            screen.SetAttribute(x, 16, 0, 6);

        screen.PrintMessage(0, 16, data.RoomName);
    }

    private void DrawItems(RoomData data)
    {
        foreach (var key in data.RoomKeys)
        {
            if (key.Attr == 255) continue;

            int attr = data.Attributes[key.Position.Y * 32 + key.Position.X];
            attr &= 0xF8; // XXXXX--- - bit pattern
            attr |= key.Attr;

            ZXAttribute attribute = new ZXAttribute((byte)attr);

            screen.SetAttribute(key.Position.X, key.Position.Y, attribute);
            screen.DrawSprite(key.Position.X, key.Position.Y, 1, 1, data.KeyShape);
        }
    }

    private void DrawPortal(RoomData data)
    {
        for (int py = 0; py < 2; py++)
        {
            for (int px = 0; px < 2; px++)
            {
                screen.SetAttribute(data.Portal.X + px, data.Portal.Y + py, data.Portal.Attr);
            }
        }

        screen.RowOrderSprite();
        screen.DrawSprite(data.Portal.X, data.Portal.Y, 2, 2, data.Portal.Shape);
    }

    private void DrawRoom(RoomData data)
    {
        for (int y = 0; y < 16; y++)
        {
            for (int x = 0; x < 32; x++)
            {
                int attr = data.Attributes[y * 32 + x];
                if (attr != 0)
                {
                    // HACK: SOmething v. wrong with room #19
                    if (!data.Blocks.ContainsKey(attr)) continue;

                    int ink = attr.GetInk();
                    int paper = attr.GetPaper();
                    bool bright = attr.IsBright();
                    bool flashing = attr.IsFlashing();

                    screen.SetAttribute(x, y, ink, paper, bright, flashing);

                    if (data.Blocks[attr].BlockType == BlockType.Conveyor)
                    {
                        screen.DrawSprite(x, y, 1, 1, data.ConveyorShape);
                    }
                    else
                    {
                        screen.DrawSprite(x, y, 1, 1, data.Blocks[attr].Shape);
                    }
                }
            }
        }
    }

    private void DrawHorizontalGuardians(IList<Mob> mobs, RoomData data)
    {
        foreach (var g in mobs)
        {
            if (g.Attribute == 0) continue;

            screen.FillAttribute(g.X, g.Y, 2, 2, g.Attribute.GetInk(), g.Attribute.GetPaper());
            screen.DrawSprite(g.X, g.Y, 2, 2, data.GuardianGraphics[g.Frame]);
        }
    }
}


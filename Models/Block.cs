using System.Drawing;

namespace Arkanoid.Models;

public class Block  // было internal, стало public
{
    public Rectangle Rect { get; set; }
    public int Health { get; set; }
    public bool IsDestroyed { get; set; }
    public BlockType Type { get; set; }

    public Block(Rectangle rect, int health = 1, BlockType type = BlockType.Blue)
    {
        Rect = rect;
        Health = health;
        Type = type;
        IsDestroyed = false;
    }

    public void HitBlock()
    {
        Health -= 1;
        if (Health <= 0)
        {
            IsDestroyed = true;
        }
    }
}

public enum BlockType  // тоже сделай public
{
    Red,
    Green,
    Blue
}

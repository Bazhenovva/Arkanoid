using System.Drawing;

namespace Arkanoid.Models;

public class Block
{
    public bool HasBonus { get; set; } = false;
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
    public int GetScore() => Type switch
        {
            BlockType.Red => 30,
            BlockType.Green => 20,
            BlockType.Blue => 10,
            _ => 10
        };

}

public enum BlockType
{
    Red,
    Green,
    Blue
}

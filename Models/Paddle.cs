using System.Drawing;

namespace Arkanoid.Models;

internal class Paddle
{
    public Rectangle Rect { get; set; }

    public Paddle(Rectangle rect)
    {
        Rect = rect;
    }

    public void SetPaddlePos(int x)
    {
        Rect = new Rectangle(x, Rect.Y, Rect.Width, Rect.Height);
    }
}
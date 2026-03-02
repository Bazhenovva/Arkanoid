using System.Drawing;

namespace Arkanoid.Models;

public class Ball
{
    public Rectangle Rect { get; set; }
    public int SpeedX { get; set; }
    public int SpeedY { get; set; }
    public int Damage { get; set; } = 1;
    public Ball(Rectangle rect)
    {
        Rect = rect;
    }

    public void SetBallPos(int x, int y)
    {
        Rect = new Rectangle(x, y, Rect.Width, Rect.Height);
    }
}

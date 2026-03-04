namespace Arkanoid.Models;

/// <summary>
/// Мяч в игре "Арканоид".Хранит позицию, скорость и урон.
/// </summary>
public class Ball
{
    /// <summary>
    /// Границы мяча на игровом поле.Включает позицию и размер.
    /// </summary>
    public Rectangle Rect { get;  private set; }

    /// <summary>
    /// Горизонтальная скорость мяча.Положительное значение — вправо, отрицательное — влево.
    /// </summary>
    public int SpeedX { get; set; }

    /// <summary>
    /// Вертикальная скорость мяча.Положительное значение — вниз, отрицательное — вверх.
    /// </summary>
    public int SpeedY { get; set; }

    /// <summary>
    /// Урон мяча по блокам.
    /// По умолчанию 1, бонус увеличивает до 2.
    /// </summary>
    public int Damage { get; set; } = 1;

    /// <summary>
    /// Создаёт новый мяч с заданными границами.
    /// </summary>
    public Ball(Rectangle rect)
    {
        Rect = rect;
    }

    /// <summary>
    /// Обновляет позицию мяча, сохраняя размер.
    /// Вызывается из <see cref="Arkanoid.Game.ArkanoidGame.MoveBall()"/>.
    /// </summary>
    public void SetBallPos(int x, int y) =>
        Rect = new Rectangle(x, y, Rect.Width, Rect.Height);
}

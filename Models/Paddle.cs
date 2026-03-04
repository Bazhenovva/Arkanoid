namespace Arkanoid.Models;

/// <summary>
/// Платформа игрока в игре "Арканоид".Управляет позицией и отрисовкой платформы.
/// </summary>
public class Paddle
{
    /// <summary>
    /// Границы платформы на игровом поле.Включает позицию и размер.
    /// </summary>
    public Rectangle Rect { get; private set; }

    /// <summary>
    /// Создаёт новую платформу с заданными границами.
    /// </summary>
    public Paddle(Rectangle rect)
    {
        Rect = rect;
    }

    /// <summary>
    /// Устанавливает новую горизонтальную позицию платформы.Вертикальная позиция и размер сохраняются.
    /// </summary>
    public void SetPaddlePos(int x) =>
        Rect = new Rectangle(x, Rect.Y, Rect.Width, Rect.Height);
}

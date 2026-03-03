using System.Drawing;

namespace Arkanoid.Models;

/// <summary>
/// Платформа игрока в игре "Арканоид".Управляет позицией и отрисовкой платформы.
/// Используется в <see cref="Arkanoid.Game.ArkanoidGame"/>.
/// </summary>
public class Paddle
{
    /// <summary>
    /// Границы платформы на игровом поле.Включает позицию и размер.
    /// </summary>
    public Rectangle Rect { get; set; }

    /// <summary>
    /// Создаёт новую платформу с заданными границами.
    /// </summary>
    public Paddle(Rectangle rect) => Rect = rect;

    /// <summary>
    /// Устанавливает новую горизонтальную позицию платформы.Вертикальная позиция и размер сохраняются.
    /// Вызывается из <see cref="Arkanoid.Game.ArkanoidGame.MovePaddleTo(int)"/>.
    /// </summary>
    public void SetPaddlePos(int x) =>
        Rect = new Rectangle(x, Rect.Y, Rect.Width, Rect.Height);
}

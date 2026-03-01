using System.Drawing;

namespace Arkanoid.Models;

/// <summary>
/// Представляет блок в игре "Арканоид". Блок имеет цвет (тип), здоровье (вес) и прямоугольную область.
/// При каждом столкновении с мячом здоровье уменьшается на 1. Когда здоровье становится ≤ 0, блок помечается как уничтоженный.
/// </summary>
internal class Block
{
    /// <summary>
    /// Границы блока на игровом поле.
    /// </summary>
    public Rectangle Rect { get; set; }

    /// <summary>
    /// Текущее здоровье блока. Равно его весу при создании.Уменьшается при каждом столкновении с мячом.
    /// </summary>
    public int Health { get; set; }

    /// <summary>
    /// Определяет, уничтожен ли блок.
    /// Устанавливается в true, когда Health <= 0.
    /// </summary>
    public bool IsDestroyed { get; set; }

    /// <summary>
    /// Тип блока (цвет), определяющий его начальное здоровье (вес).
    /// </summary>
    public BlockType Type { get; set; }

    /// <summary>
    /// Создаёт новый блок с указанными границами, здоровьем и типом.
    /// </summary>
    /// <param name="rect">Границы блока (позиция и размер)</param>
    /// <param name="health">Начальное здоровье (вес). По умолчанию = 1.</param>
    /// <param name="type">Тип блока (цвет). По умолчанию = Blue.</param>
    public Block(Rectangle rect, int health = 1, BlockType type = BlockType.Blue)
    {
        Rect = rect;
        Health = health;
        Type = type;
        IsDestroyed = false;
    }

    /// <summary>
    /// Обрабатывает попадание мяча в блок: уменьшает здоровье на 1. Если здоровье становится ≤ 0, блок помечается как уничтоженный.
    /// </summary>
    public void HitBlock()
    {
        Health -= 1;
        if (Health <= 0)
        {
            IsDestroyed = true;
        }
    }
}

/// <summary>
/// Типы блоков по цвету. Определяют начальное здоровье (вес):
/// - Red: 1 удар,
/// - Green: 2 удара,
/// - Blue: 3 удара.
/// </summary>
internal enum BlockType
{
    /// <summary>Красный блок — уничтожается за 3 удара.</summary>
    Red,

    /// <summary>Зелёный блок — уничтожается за 2 удара.</summary>
    Green,

    /// <summary>Синий блок — уничтожается за 1 удар.</summary>
    Blue
}

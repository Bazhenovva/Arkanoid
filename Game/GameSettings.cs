namespace Arkanoid.Game;

public static class GameSettings
{
    // Игровое поле
    public const int Rows = 10;
    public const int Cols = 6;

    // Размеры объектов
    public const int BallWidth = 20;
    public const int BallHeight = 20;
    public const int PaddleWidth = 110;
    public const int PaddleHeight = 20;

    // Скорости
    public const int MinSpeedX = 1;
    public const int MaxSpeedX = 5;
    public const int MinSpeedY = -10;
    public const int MaxSpeedY = -5;

    // Позиции
    public const int StartBlockY = 50;
    public const int BlockBorder = 2;
    public const int BallStartOffsetY = 175; // Расстояние от низа до шара

    // Таймер
    public const int TimerInterval = 10;

    // Бонусы
    public const int BonusChancePercent = 35; // 35% шанс выпадения
    public const int HeavyBallDuration = 12; // секунд
    public const int HeavyBallDamage = 2;

    // Отрисовка UI
    public const int ScoreTextX = 10;
    public const int ScoreTextY = 10;
    public const int BonusTextX = 200; // Отступ справа (ClientSize.Width - BonusTextX)
    public const int BonusTextY = 10;
    public const int ScoreFontSize = 16;
    public const int BonusFontSize = 10;

    // Платформа (разделение на зоны)
    public const int PaddleZones = 3;

    // Координаты отрисовки
    public const int ImagePositionX = 0;
    public const int ImagePositionY = 0;

    // ← НОВЫЕ КОНСТАНТЫ для расчётов
    public const int Half = 2;
    public const int DirectionRandomRange = 2; // 0 или 1
    public const int DirectionLeft = -1;
    public const int DirectionRight = 1;
    public const int BlockTypeRandomRange = 3; // 0, 1, 2
    public const int BonusChanceRandomRange = 100;
    public const int RandomMaxInclusive = 1; // для +1 в Next(min, max+1)

    // Здоровье блоков
    public const int BlockHealthBlue = 1;
    public const int BlockHealthGreen = 2;
    public const int BlockHealthRed = 3;
}

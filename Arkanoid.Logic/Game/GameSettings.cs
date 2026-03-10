namespace Arkanoid.Logic.Game;

/// <summary>
/// Глобальные настройки игры "Арканоид".
/// Содержит константы для размеров, скоростей, позиций и геймплея.
/// Используется в <see cref="ArkanoidGame"/>
/// </summary>
public static class GameSettings
{
    /// <summary>
    /// Количество рядов блоков на игровом поле.
    /// </summary>
    public const int Rows = 10;

    /// <summary>
    /// Количество колонок блоков на игровом поле.
    /// </summary>
    public const int Cols = 6;

    /// <summary>
    /// Ширина мяча в пикселях.
    /// </summary>
    public const int BallWidth = 20;

    /// <summary>
    /// Высота мяча в пикселях.
    /// </summary>
    public const int BallHeight = 20;

    /// <summary>
    /// Ширина платформы игрока в пикселях.
    /// </summary>
    public const int PaddleWidth = 110;

    /// <summary>
    /// Высота платформы игрока в пикселях.
    /// </summary>
    public const int PaddleHeight = 20;

    /// <summary>
    /// Минимальная горизонтальная скорость мяча.
    /// </summary>
    public const int MinSpeedX = 3;

    /// <summary>
    /// Максимальная горизонтальная скорость мяча.
    /// </summary>
    public const int MaxSpeedX = 8;

    /// <summary>
    /// Минимальная вертикальная скорость мяча (отрицательная = вверх).
    /// </summary>
    public const int MinSpeedY = -12;

    /// <summary>
    /// Максимальная вертикальная скорость мяча (отрицательная = вверх).
    /// </summary>
    public const int MaxSpeedY = -8;

    /// <summary>
    /// Начальная координата Y для первого ряда блоков.
    /// </summary>
    public const int StartBlockY = 50;

    /// <summary>
    /// Отступ между блоками в пикселях.
    /// </summary>
    public const int BlockBorder = 2;

    /// <summary>
    /// Расстояние от низа экрана до стартовой позиции мяча.
    /// </summary>
    public const int BallStartOffsetY = 175;

    /// <summary>
    /// Шанс выпадения бонуса при разрушении блока (в процентах).
    /// </summary>
    public const int BonusChancePercent = 35;

    /// <summary>
    /// Длительность действия бонуса "Тяжёлый мяч" в секундах.
    /// </summary>
    public const int HeavyBallDuration = 10;

    /// <summary>
    /// Урон "Тяжёлого мяча" по блокам.
    /// </summary>
    public const int HeavyBallDamage = 2;

    /// <summary>
    /// Координата X для отрисовки счёта на экране.
    /// </summary>
    public const int ScoreTextX = 10;

    /// <summary>
    /// Координата Y для отрисовки счёта на экране.
    /// </summary>
    public const int ScoreTextY = 10;

    /// <summary>
    /// Отступ справа для отрисовки индикатора бонуса.
    /// Рассчитывается как ClientSize.Width - BonusTextX
    /// </summary>
    public const int BonusTextX = 200;

    /// <summary>
    /// Координата Y для отрисовки индикатора бонуса.
    /// </summary>
    public const int BonusTextY = 10;

    /// <summary>
    /// Размер шрифта для отображения счёта.
    /// </summary>
    public const int ScoreFontSize = 16;

    /// <summary>
    /// Размер шрифта для отображения индикатора бонуса.
    /// </summary>
    public const int BonusFontSize = 10;

    /// <summary>
    /// Количество зон на платформе для расчёта угла отскока мяча.
    /// Используется в <see cref="ArkanoidGame.CheckPaddleCollision()"/>.
    /// </summary>
    public const int PaddleZones = 3;

    /// <summary>
    /// Координата X для отрисовки изображений по умолчанию.
    /// </summary>
    public const int ImagePositionX = 0;

    /// <summary>
    /// Координата Y для отрисовки изображений по умолчанию.
    /// </summary>
    public const int ImagePositionY = 0;

    /// <summary>
    /// Диапазон для генерации случайного направления (0 или 1).
    /// </summary>
    public const int DirectionRandomRange = 2;

    /// <summary>
    /// Значение направления влево для горизонтальной скорости.
    /// </summary>
    public const int DirectionLeft = -1;

    /// <summary>
    /// Значение направления вправо для горизонтальной скорости.
    /// </summary>
    public const int DirectionRight = 1;

    /// <summary>
    /// Диапазон для случайного выбора типа блока (0, 1, 2).
    /// </summary>
    public const int BlockTypeRandomRange = 3;

    /// <summary>
    /// Диапазон для генерации процента выпадения бонуса (0–99).
    /// </summary>
    public const int BonusChanceRandomRange = 100;

    /// <summary>
    /// Добавка к максимальному значению в <see cref="System.Random.Next(int, int)"/>
    /// для включения верхней границы диапазона.
    /// </summary>
    public const int RandomMaxInclusive = 1;

    /// <summary>
    /// Здоровье синего блока (уничтожается за 1 удар).
    /// </summary>
    public const int BlockHealthBlue = 1;

    /// <summary>
    /// Здоровье зелёного блока (уничтожается за 2 удара).
    /// </summary>
    public const int BlockHealthGreen = 2;

    /// <summary>
    /// Здоровье красного блока (уничтожается за 3 удара).
    /// </summary>
    public const int BlockHealthRed = 3;

    /// <summary>
    /// Порог урона для активации режима тяжёлого мяча.
    ///</summary>
    public const int HeavyBallDamageThreshold = 1;
}

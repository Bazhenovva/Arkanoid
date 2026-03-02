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
    public const int MinSpeed = 4;
    public const int MaxSpeedX = 8;
    public const int MinSpeedY = -14;
    public const int MaxSpeedY = -8;

    // Позиции
    public const int StartBlockY = 50;
    public const int BlockBorder = 2;

    // Таймер
    public const int TimerInterval = 5;
}

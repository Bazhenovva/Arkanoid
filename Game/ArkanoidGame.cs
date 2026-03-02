// Game/ArkanoidGame.cs
using Arkanoid.Models;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Arkanoid.Game;

public class ArkanoidGame
{
    // Состояние игры
    public GameStatus Status { get; private set; } = GameStatus.NotStarted;

    // Объекты
    public Ball Ball { get; private set; } = null!;
    public Paddle Paddle { get; private set; } = null!;
    public List<Block> Blocks { get; private set; } = [];

    // Границы поля
    public int MinX { get; private set; }
    public int MaxX { get; private set; }
    public int MinY { get; private set; }
    public int MaxY { get; private set; }

    private readonly Random random = new();

    // События для уведомления UI
    public event Action? StateChanged;
    public event Action? GameWon;
    public event Action? GameLost;

    public ArkanoidGame(int width, int height)
    {
        SetFieldSize(width, height);
        InitializeObjects();
    }

    private void SetFieldSize(int width, int height)
    {
        MinX = 0; MaxX = width;
        MinY = 0; MaxY = height;
    }

    private void InitializeObjects()
    {
        // Шар
        var startX = (MaxX - GameSettings.BallWidth) / 2;
        var startY = MaxY - 175;
        Ball = new Ball(new Rectangle(startX, startY, GameSettings.BallWidth, GameSettings.BallHeight));

        var direction = random.Next(0, 2) == 0 ? -1 : 1;
        Ball.SpeedX = random.Next(GameSettings.MinSpeed, GameSettings.MaxSpeedX + 1) * direction;
        Ball.SpeedY = random.Next(GameSettings.MinSpeedY, GameSettings.MaxSpeedY + 1);

        // Платформа
        var paddleX = (MaxX - GameSettings.PaddleWidth) / 2;
        var paddleY = startY + GameSettings.BallHeight;
        Paddle = new Paddle(new Rectangle(paddleX, paddleY, GameSettings.PaddleWidth, GameSettings.PaddleHeight));

        // Блоки
        CreateBlocks();
    }

    private void CreateBlocks()
    {
        var blockWidth = MaxX / GameSettings.Cols;
        var blockHeight = MaxY / (GameSettings.Rows * 2);
        Blocks.Clear();

        for (var row = 0; row < GameSettings.Rows; row++)
        {
            for (var col = 0; col < GameSettings.Cols; col++)
            {
                var x = col * blockWidth;
                var y = GameSettings.StartBlockY + row * blockHeight;

                var rand = random.Next(0, 3);
                var (type, health) = rand switch
                {
                    0 => (BlockType.Blue, 1), 1 => (BlockType.Green, 2), _ => (BlockType.Red, 3)
                };

                Blocks.Add(new Block(
                    new Rectangle(x + GameSettings.BlockBorder / 2, y + GameSettings.BlockBorder / 2, blockWidth - GameSettings.BlockBorder, blockHeight - GameSettings.BlockBorder), health: health, type: type));
            }
        }
    }

    public void Start()
    {
        if (Status == GameStatus.NotStarted)
        {
            Status = GameStatus.Running;
            StateChanged?.Invoke();
        }
    }

    public void Update()
    {
        if (Status != GameStatus.Running)
        {
            return;
        }

        MoveBall();
        CheckWallCollisions();
        CheckPaddleCollision();
        CheckBlockCollisions();
        CheckGameOver();
    }

    private void MoveBall()
    {
        Ball.SetBallPos(Ball.Rect.X + Ball.SpeedX, Ball.Rect.Y + Ball.SpeedY);
    }

    private void CheckWallCollisions()
    {
        if (Ball.Rect.Left <= MinX || Ball.Rect.Right >= MaxX)
        {
            Ball.SpeedX = -Ball.SpeedX;
        }

        if (Ball.Rect.Top <= MinY)
        {
            Ball.SpeedY = -Ball.SpeedY;
        }
    }

    private void CheckPaddleCollision()
    {
        if (!Ball.Rect.IntersectsWith(Paddle.Rect) || Ball.SpeedY <= 0)
        {
            return;
        }

        Ball.SpeedY = -Ball.SpeedY;

        var hitPos = Ball.Rect.X + Ball.Rect.Width / 2 - Paddle.Rect.X;
        var third = Paddle.Rect.Width / 3;

        Ball.SpeedX = hitPos < third
            ? -random.Next(GameSettings.MinSpeed, GameSettings.MaxSpeedX + 1)
            : hitPos < 2 * third
                ? 0
                : random.Next(GameSettings.MinSpeed, GameSettings.MaxSpeedX + 1);

        Ball.SpeedY = random.Next(GameSettings.MinSpeedY, GameSettings.MaxSpeedY + 1);
    }

    private void CheckBlockCollisions()
    {
        foreach (var block in Blocks)
        {
            if (!block.IsDestroyed && Ball.Rect.IntersectsWith(block.Rect))
            {
                block.HitBlock();
                Ball.SpeedY = -Ball.SpeedY;

                if (block.IsDestroyed && IsGameWon())
                {
                    Status = GameStatus.Won;
                    GameWon?.Invoke();
                }
                break;
            }
        }
    }

    private void CheckGameOver()
    {
        if (Ball.Rect.Top > MaxY)
        {
            Status = GameStatus.Lost;
            GameLost?.Invoke();
        }
    }

    private bool IsGameWon() => Blocks.TrueForAll(b => b.IsDestroyed);

    // Методы для управления из UI
    public void MovePaddleTo(int x)
    {
        var newX = x - Paddle.Rect.Width / 2;
        newX = Math.Max(MinX, Math.Min(newX, MaxX - Paddle.Rect.Width));
        Paddle.SetPaddlePos(newX);

        // Если игра ещё не началась — двигаем шар вместе с платформой
        if (Status == GameStatus.NotStarted)
        {
            var ballX = Paddle.Rect.X + (Paddle.Rect.Width - Ball.Rect.Width) / 2;
            ballX = Math.Max(MinX, Math.Min(ballX, MaxX - Ball.Rect.Width));
            Ball.SetBallPos(ballX, Ball.Rect.Y);
        }

        StateChanged?.Invoke();
    }

    public void ResizeField(int width, int height)
    {
        SetFieldSize(width, height);
        // При ресайзе можно либо пересоздать объекты, либо масштабировать — пока оставляем как есть
        InitializeObjects();
        StateChanged?.Invoke();
    }
}

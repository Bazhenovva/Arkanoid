using Arkanoid.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Arkanoid;

public partial class Form1 : Form
{
    // === ИСХОДНЫЕ ПОЛЯ ARKANOID (без изменений) ===
    private bool gameStarted = false;
    private Ball ball = null!;
    private Paddle paddle = null!;
    private int MinX, MaxX, MaxY, MinY;
    private List<Block> Blocks = [];
    private const int Rows = 10;
    private const int Cols = 6;
    private readonly Random random = new();

    // === BUFFER (техника из SnowfallForm) ===
    private Bitmap? buffer;
    private Graphics? bufferGraphics;
    // =========================================

    public Form1()
    {
        InitializeComponent();
        this.DoubleBuffered = true;
        this.KeyPreview = true;
    }

    // Инициализация буфера (один раз)
    private void InitBuffer()
    {
        bufferGraphics?.Dispose();
        buffer?.Dispose();

        if (ClientSize.Width > 0 && ClientSize.Height > 0)
        {
            buffer = new Bitmap(ClientSize.Width, ClientSize.Height);
            bufferGraphics = Graphics.FromImage(buffer);
            bufferGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }
    }

    // Рендер в буфер (логика отрисовки из Form1_Paint, но рисуем в bufferGraphics)
    private void RenderToBuffer()
    {
        if (buffer == null || bufferGraphics == null)
        {
            return;
        }

        // Фон (как в оригинале)
        try
        {
            var bg = Properties.Resources.backgroundImg;
            if (bg != null)
            {
                bufferGraphics.DrawImage(bg, 0, 0, ClientSize.Width, ClientSize.Height);
            }
            else
            {
                bufferGraphics.Clear(Color.Black);
            }
        }
        catch
        {
            bufferGraphics.Clear(Color.Black);
        }

        // Шар и платформа (как в оригинале)
        bufferGraphics.FillEllipse(Brushes.White, ball.Rect);
        bufferGraphics.FillRectangle(Brushes.DarkViolet, paddle.Rect);

        // Блоки (как в оригинале)
        foreach (var block in Blocks)
        {
            if (!block.IsDestroyed)
            {
                Brush brush = block.Type switch
                {
                    BlockType.Red => Brushes.Red,
                    BlockType.Green => Brushes.Green,
                    BlockType.Blue => Brushes.Blue,
                    _ => Brushes.Blue
                };
                bufferGraphics.FillRectangle(brush, block.Rect);
                bufferGraphics.DrawRectangle(Pens.Black, block.Rect);
            }
        }
    }

    // Прямой вывод буфера на экран (как в SnowfallForm)
    private void BlitBufferToScreen()
    {
        if (buffer != null)
        {
            using var g = CreateGraphics();
            g.DrawImage(buffer, 0, 0);
        }
    }

    // === Form1_Load: инициализация (как в оригинале) ===
    private void Form1_Load(object sender, EventArgs e)
    {
        MinX = 0;
        MaxX = ClientSize.Width;
        MinY = 0;
        MaxY = ClientSize.Height;

        // Инициализация буфера
        InitBuffer();

        // Шар: 20x20 (как в оригинале)
        var ballWidth = 20;
        var ballHeight = 20;
        var startXBall = (MaxX - ballWidth) / 2;
        var startYBall = MaxY - 175;
        ball = new Ball(new Rectangle(startXBall, startYBall, ballWidth, ballHeight));

        var direction = random.Next(0, 2) == 0 ? -1 : 1;
        ball.SpeedX = random.Next(1, 5) * direction;
        ball.SpeedY = random.Next(-10, -5);

        // Платформа: 110x20 (как в оригинале)
        var paddleWidth = 110;
        var paddleHeight = 20;
        var startXPaddle = (MaxX - paddleWidth) / 2;
        var startYPaddle = startYBall + ballHeight;
        paddle = new Paddle(new Rectangle(startXPaddle, startYPaddle, paddleWidth, paddleHeight));

        // -------- БЛОКИ (как в оригинале) --------
        var blockWidth = MaxX / Cols;
        var blockHeight = MaxY / (Rows * 2);
        var startBlockY = 50;
        var blockBorder = 2;
        Blocks = [];

        for (var row = 0; row < Rows; row++)
        {
            for (var col = 0; col < Cols; col++)
            {
                var x = col * blockWidth;
                var y = startBlockY + row * blockHeight;

                var rand = random.Next(0, 3);
                BlockType type;
                int health;

                switch (rand)
                {
                    case 0:
                        type = BlockType.Blue;
                        health = 1;
                        break;
                    case 1:
                        type = BlockType.Green;
                        health = 2;
                        break;
                    default:
                        type = BlockType.Red;
                        health = 3;
                        break;
                }

                Blocks.Add(new Block(
                    new Rectangle(x + blockBorder / 2, y + blockBorder / 2, blockWidth - blockBorder, blockHeight - blockBorder),
                    health: health,
                    type: type));
            }
        }

        // Первая отрисовка через буфер
        RenderToBuffer();
        BlitBufferToScreen();
    }

    // Пересоздание буфера при изменении размера (обязательно для этого подхода)
    private void Form1_Resize(object sender, EventArgs e)
    {
        if (ClientSize.Width > 0 && ClientSize.Height > 0)
        {
            MinX = 0; MaxX = ClientSize.Width;
            MinY = 0; MaxY = ClientSize.Height;
            InitBuffer();
            RenderToBuffer();
            BlitBufferToScreen();
        }
    }

    // === MouseMove: логика без изменений ===
    private void Form1_MouseMove(object sender, MouseEventArgs e)
    {
        var newX = e.X - paddle.Rect.Width / 2;
        if (newX < MinX)
        {
            newX = MinX;
        }

        if (newX > MaxX - paddle.Rect.Width)
        {
            newX = MaxX - paddle.Rect.Width;
        }
        paddle.SetPaddlePos(newX);

        if (!gameStarted)
        {
            var ballX = paddle.Rect.X + (paddle.Rect.Width - ball.Rect.Width) / 2;
            if (ballX < MinX)
            {
                ballX = MinX;
            }

            if (ballX > MaxX - ball.Rect.Width)
            {
                ballX = MaxX - ball.Rect.Width;
            }
            ball.SetBallPos(ballX, ball.Rect.Y);

            // Вместо Invalidate(): рендер в буфер + прямой вывод
            RenderToBuffer();
            BlitBufferToScreen();
        }
    }

    // === MouseClick: без изменений ===
    private void Form1_MouseClick(object sender, MouseEventArgs e)
    {
        if (!gameStarted)
        {
            gameStarted = true;
        }
        timer.Start();
    }

    // === Timer_Tick: вся игровая логика как в оригинале ===
    private void Timer_Tick(object sender, EventArgs e)
    {
        ball.SetBallPos(ball.Rect.X + ball.SpeedX, ball.Rect.Y + ball.SpeedY);

        if (ball.Rect.Left <= MinX || ball.Rect.Right >= MaxX)
        {
            ball.SpeedX = -ball.SpeedX;
        }
        if (ball.Rect.Top <= MinY)
        {
            ball.SpeedY = -ball.SpeedY;
        }
        if (ball.Rect.IntersectsWith(paddle.Rect) && ball.SpeedY > 0)
        {
            ball.SpeedY = -ball.SpeedY;
            var hitPos = ball.Rect.X + ball.Rect.Width / 2 - paddle.Rect.X;
            var third = paddle.Rect.Width / 3;
            if (hitPos < third)
            {
                ball.SpeedX = -random.Next(1, 5);
            }
            else if (hitPos < 2 * third)
            {
                ball.SpeedX = 0;
            }
            else
            {
                ball.SpeedX = random.Next(1, 5);
            }
            ball.SpeedY = random.Next(-10, -5);
        }

        foreach (var block in Blocks)
        {
            if (!block.IsDestroyed && ball.Rect.IntersectsWith(block.Rect))
            {
                block.HitBlock();
                ball.SpeedY = -ball.SpeedY;

                if (block.IsDestroyed && IsGameWon())
                {
                    // Финальный рендер перед закрытием
                    RenderToBuffer();
                    BlitBufferToScreen();
                    timer.Stop();
                    MessageBox.Show("Вы выиграли ", "Победа", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                    return;
                }
                break;
            }
        }

        // === ЗАМЕНА Invalidate() на буферный рендер ===
        RenderToBuffer();
        BlitBufferToScreen();
        // ============================================

        if (ball.Rect.Top > MaxY)
        {
            timer.Stop();
            MessageBox.Show("Вы проиграли", "Поражение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
    }

    private bool IsGameWon() => Blocks.All(b => b.IsDestroyed);


}

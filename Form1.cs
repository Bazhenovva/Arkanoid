using Arkanoid.Classes;
using Arkanoid.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Arkanoid;

public partial class Form1 : Form
{
    private bool gameStarted = false;
    private Ball ball = null!;
    private Paddle paddle = null!;
    private int MinX, MaxX, MaxY, MinY;
    private List<Block> Blocks = [];
    private const int Rows = 10;
    private const int Cols = 6;
    private readonly Random random = new();

    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Paint(object sender, PaintEventArgs e)
    {
        //  фон из ресурсов
        try
        {
            var bg = Properties.Resources.backgroundImg;
            if (bg != null)
            {
                e.Graphics.DrawImage(bg, 0, 0, ClientSize.Width, ClientSize.Height);
            }
            else
            {
                e.Graphics.Clear(Color.Black);
            }
        }
        catch
        {
            e.Graphics.Clear(Color.Black);
        }

        // Отрисовка шара и платформы
        e.Graphics.FillEllipse(Brushes.White, ball.Rect);
        e.Graphics.FillRectangle(Brushes.Orange, paddle.Rect);

        // Отрисовка блоков
        foreach (var block in Blocks)
        {
            if (!block.IsDestroyed)
            {
                Brush brush = block.Type switch
                {
                    BlockType.Red => Brushes.Red, BlockType.Green => Brushes.Green, BlockType.Blue => Brushes.Blue, _ => Brushes.Blue
                };
                e.Graphics.FillRectangle(brush, block.Rect);
                e.Graphics.DrawRectangle(Pens.Black, block.Rect);
            }
        }
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        MinX = 0;
        MaxX = ClientSize.Width;
        MinY = 0;
        MaxY = ClientSize.Height;

        // Шар: 20x20
        var ballWidth = 20;
        var ballHeight = 20;
        var startXBall = (MaxX - ballWidth) / 2;
        var startYBall = MaxY - 175;
        ball = new Ball(new Rectangle(startXBall, startYBall, ballWidth, ballHeight));

        var direction = random.Next(0, 2) == 0 ? -1 : 1;
        ball.SpeedX = random.Next(1, 5) * direction;
        ball.SpeedY = random.Next(-10, -5);

        // Платформа: 110x20
        var paddleWidth = 110;
        var paddleHeight = 20;
        var startXPaddle = (MaxX - paddleWidth) / 2;
        var startYPaddle = startYBall + ballHeight;
        paddle = new Paddle(new Rectangle(startXPaddle, startYPaddle, paddleWidth, paddleHeight));

        // Блоки
        var blockWidth = MaxX / Cols;
        var blockHeight = MaxY / (Rows * 2);
        var startBlockY = 50;
        var blockBorder = 2;
        Blocks = [];

        for (var row = 0; row < Rows; row++)
        {
            var blockType = row switch
            {
                < 3 => BlockType.Red,
                < 6 => BlockType.Green,
                _ => BlockType.Blue
            };

            for (var col = 0; col < Cols; col++)
            {
                var x = col * blockWidth;
                var y = startBlockY + row * blockHeight;
                Blocks.Add(new Block(
                    new Rectangle(x + blockBorder / 2, y + blockBorder / 2, blockWidth - blockBorder, blockHeight - blockBorder),
                    health: 1,
                    type: blockType));
            }
        }
    }

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
            Invalidate();
        }
    }

    private void Form1_MouseClick(object sender, MouseEventArgs e)
    {
        if (!gameStarted)
        {
            gameStarted = true;
        }
        timer.Start();
    }

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
                    Invalidate();
                    timer.Stop();
                    MessageBox.Show("Вы выиграли :)", "Победа", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                break;
            }
        }

        Invalidate();

        if (ball.Rect.Top > MaxY)
        {
            timer.Stop();
            MessageBox.Show("Вы проиграли ", "Поражение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
    }

    private bool IsGameWon() => Blocks.All(b => b.IsDestroyed);
}

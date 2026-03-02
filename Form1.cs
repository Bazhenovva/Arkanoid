using Arkanoid.Game;
using Arkanoid.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Arkanoid;

public partial class Form1 : Form
{
    private ArkanoidGame game = null!;

    // Buffer для отрисовки (как в SnowfallForm)
    private Bitmap? buffer;
    private Graphics? bufferGraphics;

    public Form1()
    {
        InitializeComponent();
        this.DoubleBuffered = true;
        this.KeyPreview = true;
    }

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

    private void RenderToBuffer()
    {
        if (buffer == null || bufferGraphics == null)
        {
            return;
        }

        // Фон
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
        catch { bufferGraphics.Clear(Color.Black); }

        // Шар и платформа
        bufferGraphics.FillEllipse(Brushes.White, game.Ball.Rect);
        bufferGraphics.FillRectangle(Brushes.DarkViolet, game.Paddle.Rect);

        // Блоки
        foreach (var block in game.Blocks)
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

    private void BlitBufferToScreen()
    {
        if (buffer != null)
        {
            using var g = CreateGraphics();
            g.DrawImage(buffer, 0, 0);
        }
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        // Создаём игру с размерами формы
        game = new ArkanoidGame(ClientSize.Width, ClientSize.Height);

        // Подписываемся на события игры
        game.StateChanged += () => { RenderToBuffer(); BlitBufferToScreen(); };
        game.GameWon += () =>
        {
            timer.Stop();
            MessageBox.Show("Вы выиграли!", "Победа", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        };
        game.GameLost += () =>
        {
            timer.Stop();
            MessageBox.Show("Вы проиграли", "Поражение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        };

        InitBuffer();
        RenderToBuffer();
        BlitBufferToScreen();
    }

    private void Form1_Resize(object sender, EventArgs e)
    {
        if (ClientSize.Width > 0 && ClientSize.Height > 0)
        {
            game.ResizeField(ClientSize.Width, ClientSize.Height);
            InitBuffer();
            RenderToBuffer();
            BlitBufferToScreen();
        }
    }

    private void Form1_MouseMove(object sender, MouseEventArgs e)
    {
        game.MovePaddleTo(e.X);
        RenderToBuffer();
        BlitBufferToScreen();
    }

    private void Form1_MouseClick(object sender, MouseEventArgs e)
    {
        game.Start();
        timer.Start();
    }

    private void Form1_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            timer.Stop();
            Close();
        }
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        game.Update(); // Вся логика здесь
        RenderToBuffer(); // Отрисовка текущего состояния
        BlitBufferToScreen();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        bufferGraphics?.Dispose();
        buffer?.Dispose();
        base.OnFormClosing(e);
    }
}

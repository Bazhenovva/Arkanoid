using Arkanoid.Game;
using Arkanoid.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Arkanoid;

public partial class Form1 : Form
{
    private ArkanoidGame game = null!;
    private Bitmap? buffer;
    private Graphics? bufferGraphics;
    private Font scoreFont = new Font("Arial", GameSettings.ScoreFontSize, FontStyle.Bold);
    private Font bonusFont = new Font("Arial", GameSettings.BonusFontSize, FontStyle.Italic);

    public Form1()
    {
        InitializeComponent();
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

        try
        {
            var bg = Properties.Resources.backgroundImg;
            if (bg != null)
            {
                bufferGraphics.DrawImage(bg, GameSettings.ImagePositionX, GameSettings.ImagePositionY, ClientSize.Width, ClientSize.Height);
            }
            else
            {
                bufferGraphics.Clear(Color.Black);
            }
        }
        catch { bufferGraphics.Clear(Color.Black); }

        bufferGraphics.FillEllipse(Brushes.White, game.Ball.Rect);
        bufferGraphics.FillRectangle(Brushes.DarkViolet, game.Paddle.Rect);

        foreach (var block in game.Blocks)
        {
            if (!block.IsDestroyed)
            {
                var brush = block.Type switch
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

        // Отрисовка счёта
        bufferGraphics.DrawString(
            $"Очки: {game.Score}",
            scoreFont,
            Brushes.White,
            GameSettings.ScoreTextX,
            GameSettings.ScoreTextY);

        // Отрисовка индикатора тяжёлого мяча
        if (game.Ball.Damage > 1)
        {
            bufferGraphics.DrawString(
                " ТЯЖЁЛЫЙ МЯЧ!",
                bonusFont,
                Brushes.Orange,
                ClientSize.Width - GameSettings.BonusTextX,
                GameSettings.BonusTextY);
        }
    }

    private void BlitBufferToScreen()
    {
        if (buffer != null)
        {
            using var g = CreateGraphics();
            g.DrawImage(buffer, GameSettings.ImagePositionX, GameSettings.ImagePositionY);
        }
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        game = new ArkanoidGame(ClientSize.Width, ClientSize.Height);

        game.StateChanged += () => { RenderToBuffer(); BlitBufferToScreen(); };
        game.ScoreChanged += (score) => { RenderToBuffer(); BlitBufferToScreen(); };
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

    private void Timer_Tick(object sender, EventArgs e)
    {
        game.Update();
        RenderToBuffer();
        BlitBufferToScreen();
    }
}

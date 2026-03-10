using Arkanoid.Logic.Game;
using Arkanoid.Logic.Models;

namespace Arkanoid.WinForms.Forms;

/// <summary>
/// Главная форма игры "Арканоид". Отвечает за отрисовку, обработку ввода и связь с игровым ядром.
/// Использует <see cref="ArkanoidGame"/> для логики и <see cref="GameSettings"/> для настроек.
/// </summary>
public partial class GameForm : Form
{
    private ArkanoidGame game = null!;
    private Bitmap? buffer;
    private Graphics? bufferGraphics;
    private Font scoreFont = new ("Arial", GameSettings.ScoreFontSize, FontStyle.Bold);
    private Font bonusFont = new ("Arial", GameSettings.BonusFontSize, FontStyle.Italic);

    private Image? backgroundImage;

    /// <summary>
    /// Создаёт новую форму игры и инициализирует компоненты.
    /// </summary>
    public GameForm()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Инициализирует буфер двойной буферизации для плавной отрисовки.
    /// </summary>
    private void InitBuffer()
    {
        if (ClientSize.Width <= 0 || ClientSize.Height <= 0)
        {
            return;
        }

        buffer = new Bitmap(ClientSize.Width, ClientSize.Height);
        bufferGraphics = Graphics.FromImage(buffer);
        bufferGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

    }

    /// <summary>
    /// Отрисовывает текущее состояние игры в буфер.
    /// </summary>
    private void RenderToBuffer()
    {
        if (backgroundImage != null)
        {
            bufferGraphics.DrawImage(backgroundImage, GameSettings.ImagePositionX, GameSettings.ImagePositionY, ClientSize.Width, ClientSize.Height);
        }
        else
        {
            bufferGraphics.Clear(Color.Black);
        }

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
                    _ => Brushes.Blue
                };
                bufferGraphics.FillRectangle(brush, block.Rect);
                bufferGraphics.DrawRectangle(Pens.Black, block.Rect);
            }
        }

        bufferGraphics.DrawString(
            $"Очки: {game.Score}",
            scoreFont,
            Brushes.White,
            GameSettings.ScoreTextX,
            GameSettings.ScoreTextY);

        if (game.Ball.Damage > GameSettings.HeavyBallDamageThreshold)
        {
            bufferGraphics.DrawString(
                " ТЯЖЁЛЫЙ МЯЧ!",
                bonusFont,
                Brushes.Orange,
                ClientSize.Width - GameSettings.BonusTextX,
                GameSettings.BonusTextY);
        }
    }

    /// <summary>
    /// Копирует буфер на экран формы
    /// </summary>
    private void BlitBufferToScreen()
    {
        if (buffer != null)
        {
            using (var graphics = CreateGraphics())
            {
                graphics.DrawImage(buffer, GameSettings.ImagePositionX, GameSettings.ImagePositionY);
            }
        }
    }

    /// <summary>
    /// Обновляет экран: перерисовывает буфер и выводит на форму.
    /// </summary>
    private void UpdateDisplay()
    {
        RenderToBuffer();
        BlitBufferToScreen();
    }

    /// <summary>
    /// Обработчик события победы в игре.
    /// </summary>
    private void OnGameWon()
    {
        timer.Stop();
        MessageBox.Show("Вы выиграли!", "Победа", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Close();
    }

    /// <summary>
    /// Обработчик события поражения в игре.
    /// </summary>
    private void OnGameLost()
    {
        timer.Stop();
        MessageBox.Show("Вы проиграли", "Поражение", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Close();
    }

    /// <summary>
    /// Загрузка формы: инициализация игры и подписка на события.
    /// </summary>
    private void Form1_Load(object sender, EventArgs e)
    {
        backgroundImage = Properties.Resources.backgroundImg;
        game = new ArkanoidGame(ClientSize.Width, ClientSize.Height);

        game.StateChanged += UpdateDisplay;
        game.ScoreChanged += _ => UpdateDisplay();
        game.GameWon += OnGameWon;
        game.GameLost += OnGameLost;

        InitBuffer();
        UpdateDisplay();
    }

    /// <summary>
    /// Обработка движения мыши для управления ракеткой.
    /// </summary>
    private void Form1_MouseMove(object sender, MouseEventArgs e)
    {
        game.MovePaddleTo(e.X);
    }

    /// <summary>
    /// Обработка клика мыши для запуска игры.
    /// </summary>
    private void Form1_MouseClick(object sender, MouseEventArgs e)
    {
        game.Start();
        timer.Start();
    }

    /// <summary>
    /// Тик таймера: обновление игровой логики и отрисовка.
    /// </summary>
    private void Timer_Tick(object sender, EventArgs e)
    {
        game.Update();
        UpdateDisplay();
    }
}

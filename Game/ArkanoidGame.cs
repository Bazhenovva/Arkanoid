using Arkanoid.Models;

namespace Arkanoid.Game;

/// <summary>
/// Ядро игры "Арканоид". Управляет логикой: физика мяча, коллизии, бонусы, счёт.
/// Используется в <see cref="GameForm"/> для обновления состояния игры.
/// </summary>
public class ArkanoidGame
{
    public const int HeavyBallDamageThreshold = 1;

    /// <summary>
    /// Время окончания действия бонуса "Тяжёлый мяч".
    /// </summary>
    private DateTime heavyBallEndTime = DateTime.MinValue;

    /// <summary>
    /// Текущий счёт игрока.
    /// </summary>
    public int Score { get; private set; }

    /// <summary>
    /// Событие изменения счёта. Передаёт новое значение счёта.
    /// Подписывается <see cref="GameForm"/> для обновления UI.
    /// </summary>
    public event Action<int>? ScoreChanged;

    /// <summary>
    /// Текущее состояние игры.
    /// </summary>
    private GameStatus Status { get; set; }

    /// <summary>
    /// Мяч игры.
    /// </summary>
    public Ball Ball { get; private set; } = null!;

    /// <summary>
    /// Платформа игрока.
    /// </summary>
    public Paddle Paddle { get; private set; } = null!;

    /// <summary>
    /// Список блоков на игровом поле.
    /// </summary>
    public List<Block> Blocks { get; } = new List<Block>();

    /// <summary>
    /// Минимальная координата X игрового поля.
    /// </summary>
    private int MinX { get; set; }

    /// <summary>
    /// Максимальная координата X игрового поля.
    /// </summary>
    private int MaxX { get; set; }

    /// <summary>
    /// Минимальная координата Y игрового поля.
    /// </summary>
    private int MinY { get; set; }

    /// <summary>
    /// Максимальная координата Y игрового поля.
    /// </summary>
    private int MaxY { get; set; }

    private readonly Random random = new();

    /// <summary>
    /// Событие изменения состояния игры. Вызывается при любом изменении, требующем перерисовки.
    /// </summary>
    public event Action? StateChanged;

    /// <summary>
    /// Событие победы. Вызывается при уничтожении всех блоков.
    /// </summary>
    public event Action? GameWon;

    /// <summary>
    /// Событие поражения. Вызывается при падении мяча за нижнюю границу.
    /// </summary>
    public event Action? GameLost;

    /// <summary>
    /// Создаёт новую игру с заданными размерами поля.
    /// Инициализирует мяч, платформу и блоки.
    /// </summary>
    public ArkanoidGame(int width, int height)
    {
        SetFieldSize(width, height);
        InitializeObjects();
    }

    /// <summary>
    /// Устанавливает размеры игрового поля.
    /// </summary>
    private void SetFieldSize(int width, int height)
    {
        MinX = 0; MaxX = width;
        MinY = 0; MaxY = height;
    }

    /// <summary>
    /// Инициализирует игровые объекты: мяч, платформу, блоки.
    /// </summary>
    private void InitializeObjects()
    {
        var startX = (MaxX - GameSettings.BallWidth) / GameSettings.Half;
        var startY = MaxY - GameSettings.BallStartOffsetY;
        Ball = new Ball(new Rectangle(startX, startY, GameSettings.BallWidth, GameSettings.BallHeight));

        var direction = random.Next(0, GameSettings.DirectionRandomRange) == 0
            ? GameSettings.DirectionLeft
            : GameSettings.DirectionRight;
        Ball.SpeedX = random.Next(GameSettings.MinSpeedX, GameSettings.MaxSpeedX + GameSettings.RandomMaxInclusive) * direction;
        Ball.SpeedY = random.Next(GameSettings.MinSpeedY, GameSettings.MaxSpeedY + GameSettings.RandomMaxInclusive);

        var paddleX = (MaxX - GameSettings.PaddleWidth) / GameSettings.Half;
        var paddleY = startY + GameSettings.BallHeight;
        Paddle = new Paddle(new Rectangle(paddleX, paddleY, GameSettings.PaddleWidth, GameSettings.PaddleHeight));

        CreateBlocks();
    }

    /// <summary>
    /// Создаёт сетку блоков с случайными типами и бонусами.
    /// </summary>
    private void CreateBlocks()
    {
        var blockWidth = MaxX / GameSettings.Cols;
        var blockHeight = MaxY / (GameSettings.Rows * GameSettings.Half);
        Blocks.Clear();

        for (var row = 0; row < GameSettings.Rows; row++)
        {
            for (var col = 0; col < GameSettings.Cols; col++)
            {
                var x = col * blockWidth;
                var y = GameSettings.StartBlockY + row * blockHeight;

                var rand = random.Next(0, GameSettings.BlockTypeRandomRange);
                var (type, health) = rand switch
                {
                    0 => (BlockType.Blue, GameSettings.BlockHealthBlue),
                    1 => (BlockType.Green, GameSettings.BlockHealthGreen),
                    _ => (BlockType.Red, GameSettings.BlockHealthRed)
                };

                var block = new Block(
                    new Rectangle(
                        x + (GameSettings.BlockBorder / GameSettings.Half),
                        y + (GameSettings.BlockBorder / GameSettings.Half),
                        blockWidth - GameSettings.BlockBorder,
                        blockHeight - GameSettings.BlockBorder),
                    health: health,
                    type: type);

                block.HasBonus = random.Next(0, GameSettings.BonusChanceRandomRange) < GameSettings.BonusChancePercent;
                Blocks.Add(block);
            }
        }
    }

    /// <summary>
    /// Запускает игру.
    /// Меняет статус на <see cref="GameStatus.Running"/>.
    /// </summary>
    public void Start()
    {
        if (Status == GameStatus.NotStarted)
        {
            Status = GameStatus.Running;
            StateChanged?.Invoke();
        }
    }

    /// <summary>
    /// Обновляет состояние игры.
    /// Вызывается каждый кадр из <see cref="GameForm.Timer_Tick"/>.
    /// </summary>
    public void Update()
    {
        if (Status != GameStatus.Running)
        {
            return;
        }

        MoveBall();
        CheckHeavyBallExpiration();
        CheckWallCollisions();
        CheckPaddleCollision();
        CheckBlockCollisions();
        CheckGameOver();
    }

    /// <summary>
    /// Перемещает мяч на следующую позицию.
    /// </summary>
    private void MoveBall()
    {
        Ball.SetBallPos(Ball.Rect.X + Ball.SpeedX, Ball.Rect.Y + Ball.SpeedY);
    }

    /// <summary>
    /// Проверяет столкновение мяча с границами поля (стенами).
    /// </summary>
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

    /// <summary>
    /// Проверяет столкновение мяча с платформой игрока.
    /// Меняет направление мяча в зависимости от зоны удара.
    /// </summary>
    private void CheckPaddleCollision()
    {
        if (!Ball.Rect.IntersectsWith(Paddle.Rect) || Ball.SpeedY <= 0)
        {
            return;
        }

        Ball.SpeedY = -Ball.SpeedY;

        var hitPos = Ball.Rect.X + Ball.Rect.Width / GameSettings.Half - Paddle.Rect.X;
        var zoneWidth = Paddle.Rect.Width / GameSettings.PaddleZones;

        Ball.SpeedX = hitPos < zoneWidth
            ? -random.Next(GameSettings.MinSpeedX, GameSettings.MaxSpeedX + GameSettings.RandomMaxInclusive)
            : hitPos < GameSettings.Half * zoneWidth
                ? 0
                : random.Next(GameSettings.MinSpeedX, GameSettings.MaxSpeedX + GameSettings.RandomMaxInclusive);

        Ball.SpeedY = random.Next(GameSettings.MinSpeedY, GameSettings.MaxSpeedY + GameSettings.RandomMaxInclusive);
    }

    /// <summary>
    /// Проверяет столкновение мяча с блоками.
    /// При разрушении блока начисляет очки и проверяет условие победы.
    /// </summary>
    private void CheckBlockCollisions()
    {
        foreach (var block in Blocks)
        {
            if (!block.IsDestroyed && Ball.Rect.IntersectsWith(block.Rect))
            {
                block.Health -= Ball.Damage;
                Ball.SpeedY = -Ball.SpeedY;

                if (block.Health <= 0)
                {
                    block.IsDestroyed = true;
                    Score += block.GetScore();
                    ScoreChanged?.Invoke(Score);

                    if (block.HasBonus)
                    {
                        Ball.Damage = GameSettings.HeavyBallDamage;
                        heavyBallEndTime = DateTime.Now.AddSeconds(GameSettings.HeavyBallDuration);
                    }

                    if (IsGameWon())
                    {
                        Status = GameStatus.Won;
                        GameWon?.Invoke();
                    }
                }
                break;
            }
        }
    }

    /// <summary>
    /// Проверяет, истёк ли срок действия бонуса "Тяжёлый мяч".
    /// </summary>
    private void CheckHeavyBallExpiration()
    {
        if (DateTime.Now > heavyBallEndTime && Ball.Damage > HeavyBallDamageThreshold)
        {
            Ball.Damage = 1;
        }
    }

    /// <summary>
    /// Проверяет условие проигрыша (мяч упал за нижнюю границу).
    /// </summary>
    private void CheckGameOver()
    {
        if (Ball.Rect.Top > MaxY)
        {
            Ball.Damage = 1;
            Status = GameStatus.Lost;
            GameLost?.Invoke();
        }
    }

    /// <summary>
    /// Проверяет, уничтожены ли все блоки (условие победы).
    /// </summary>
    private bool IsGameWon() => Blocks.TrueForAll(b => b.IsDestroyed);

    /// <summary>
    /// Двигает платформу к указанной позиции X.
    /// Если игра не начата — перемещает и мяч вместе с платформой.
    /// </summary>

    public void MovePaddleTo(int x)
    {
        var newX = x - Paddle.Rect.Width / GameSettings.Half;
        newX = Math.Max(MinX, Math.Min(newX, MaxX - Paddle.Rect.Width));
        Paddle.SetPaddlePos(newX);

        if (Status == GameStatus.NotStarted)
        {
            var ballX = Paddle.Rect.X + (Paddle.Rect.Width - Ball.Rect.Width) / GameSettings.Half;
            ballX = Math.Max(MinX, Math.Min(ballX, MaxX - Ball.Rect.Width));
            Ball.SetBallPos(ballX, Ball.Rect.Y);
        }

        StateChanged?.Invoke();
    }

    /// <summary>
    /// Изменяет размеры игрового поля и пересоздаёт объекты.
    /// Вызывается при изменении размера окна формы.
    /// </summary>
    public void ResizeField(int width, int height)
    {
        SetFieldSize(width, height);
        InitializeObjects();
        StateChanged?.Invoke();
    }
}

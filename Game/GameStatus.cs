namespace Arkanoid.Game;

/// <summary>
/// Состояния игры "Арканоид".
/// Используется в <see cref="ArkanoidGame"/>.
/// </summary>
public enum GameStatus
{
    /// <summary>Игра не начата.</summary>
    NotStarted,

    /// <summary>Игра активна.</summary>
    Running,

    /// <summary>Игрок победил.</summary>
    Won,

    /// <summary>Игрок проиграл.</summary>
    Lost
}

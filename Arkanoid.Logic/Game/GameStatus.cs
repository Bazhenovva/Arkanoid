namespace Arkanoid.Logic.Game;

/// <summary>
/// Состояния игры "Арканоид".
/// Используется в <see cref="ArkanoidGame"/>.
/// </summary>
public enum GameStatus
{
    /// <summary>
    /// Игра еще не началась.
    /// </summary>
    NotStarted,

    /// <summary>
    /// Игра в процессе
    /// </summary>
    Running,

    /// <summary>
    /// Игрок победил.
    /// </summary>
    Won,

    /// <summary>
    /// Игрок проиграл.
    /// </summary>
    Lost
}

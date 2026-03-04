namespace Arkanoid.Models
{
    /// <summary>
    /// Представляет собой блок (кирпич)
    /// Содержит информацию о состоянии, здоровье и типе блока.
    /// </summary>
    public class Block
    {
        // Константы для очков.
        // Теперь баланс игры можно изменить в одном месте, не лазая по логике.
        private const int ScoreRed = 30;
        private const int ScoreGreen = 20;
        private const int ScoreBlue = 10;
        private const int ScoreDefault = 10;

        /// <summary>
        /// Указывает, содержит ли блок бонус при разрушении.
        /// </summary>
        public bool HasBonus { get; set; }

        /// <summary>
        /// Прямоугольная область блока, используемая для отрисовки и проверки коллизий.
        /// </summary>
        public Rectangle Rect { get; set; }

        /// <summary>
        /// Текущее количество здоровья блока. При достижении 0 блок уничтожается.
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// Флаг, указывающий, был ли блок полностью разрушен.
        /// </summary>
        public bool IsDestroyed { get; set; }

        /// <summary>
        /// Тип блока, определяющий его цвет и стоимость очков.
        /// </summary>
        public BlockType Type { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Block"/>.
        /// </summary>
        public Block(Rectangle rect, int health = 1, BlockType type = BlockType.Blue)
        {
            Rect = rect;
            Health = health;
            Type = type;
            IsDestroyed = false;
        }

        /// <summary>
        /// Наносит урон блоку. Уменьшает здоровье на 1.
        /// Если здоровье падает до 0 или ниже, помечает блок как уничтоженный.
        /// </summary>
        public void HitBlock()
        {
            Health -= 1;
            if (Health <= 0)
            {
                IsDestroyed = true;
            }
        }

        /// <summary>
        /// Возвращает количество очков, которое получает игрок за разрушение этого блока.
        /// Зависит от типа блока.
        /// </summary>
        public int GetScore() => Type switch
        {
            BlockType.Red => ScoreRed,
            BlockType.Green => ScoreGreen,
            BlockType.Blue => ScoreBlue,
            _ => ScoreDefault
        };
    }

    /// <summary>
    /// Перечисление типов блоков, определяющее их визуальный стиль и сложность.
    /// </summary>
    public enum BlockType
    {
        Red,
        Green,
        Blue
    }
}

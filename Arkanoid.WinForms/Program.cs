using Arkanoid.WinForms.Forms;

namespace Arkanoid.WinForms;

/// <summary>
/// Точка входа в приложение Arkanoid.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Главная точка входа в приложение. Запускает игру Arkanoid.
    /// </summary>
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new GameForm());
    }
}

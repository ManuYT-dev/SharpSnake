using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SnakeSpiel
{
    public partial class MainWindow : Window
    {
        public static Dictionary<string, Page> pages = []; // [homepage, game, settings, pause, scoreboard]
        public MainWindow()
        {
            SnakeLogger.init("log_file_snake");
            SnakeLogger.logger.Information($"Spiel wird gestarted");
            InitializeComponent();
            GameSettings.Init(1, 10, 5, 1, 20);
            pages.Add("homepage", new Homepage());
            pages.Add("pause", new PauseMenue());
            pages.Add("settings", new Subwindow_GameSettings());
            pages.Add("scoreboard", new Scoreboard());
            pages.Add("game", null);
            GoToPage("homepage");
            SnakeLogger.logger.Information($"Spiel wurde erfolgreich gestarted");
        }

        public static bool GoToPage(string pageName)
        {
            if (pages.ContainsKey(pageName) && pages[pageName] != null)
            {
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.Frame1.Navigate(pages[pageName]);
                SnakeLogger.logger.Debug($"Neue Page \"{pageName}\" geladen");
                return true;
            }
            SnakeLogger.logger.Warning($"Die Page \"{pageName}\" wurde nicht gefunden.");
            return false;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Frame1.Content is Game activeGame)
            {
                activeGame.GameWindow_KeyDown(sender, e);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SnakeSpiel
{
    public partial class Scoreboard : Page
    {
        public Scoreboard()
        {
            InitializeComponent();
        }

        private void RefreshScoreboard()
        {
            SnakeLogger.logger.Debug($"Scoreboard wird gerefreshed");
            List<ScoreEntry> topScores = ScoreboardManager.LoadScores();
            this.ScoreList.ItemsSource = topScores;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GoToPage("homepage");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshScoreboard();
        }
    }
}

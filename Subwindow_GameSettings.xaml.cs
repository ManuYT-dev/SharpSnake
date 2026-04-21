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
using System.Windows.Shapes;

namespace SnakeSpiel
{
    public partial class Subwindow_GameSettings : Page
    {
        public Subwindow_GameSettings()
        {
            InitializeComponent();
            this.UpdateValues();
        }

        private void UpdateValues()
        {
            Speed.Value = GameSettings.Speed;
            FieldSize.Value = GameSettings.FieldSize;
            InitialLength.Value = GameSettings.InitialLength;
            Essen.Value = GameSettings.Food;
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            SnakeLogger.logger.Debug("Settings werden applied");
            GameSettings.Init((int)Speed.Value, (int)FieldSize.Value, (int)InitialLength.Value, (int)Essen.Value, GameSettings.CellSize);
            MainWindow.pages.Remove("game");
            this.NavigationService.Navigate(MainWindow.pages["homepage"]);
        }
    }
}

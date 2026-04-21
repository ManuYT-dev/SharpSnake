using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class Homepage : Page
    {
        public Homepage()
        {
            InitializeComponent();
        }

        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.pages["game"] = new Game();
            MainWindow.GoToPage("game");
        }

        private void Button_Scoreboard_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GoToPage("scoreboard");
        }

        private void Button_Settings_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GoToPage("settings");
        }

        private void Button_Continue_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.GoToPage("game"))
            {
                ((Game)MainWindow.pages["game"]).Start();
            }
        }

        private void Button_Load_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists("savegame.json"))
            {
                if (!MainWindow.pages.TryGetValue("game", out Page game))
                {
                    MainWindow.pages.Add("game", new Game());
                }
                else if (MainWindow.pages["game"] == null)
                {
                    MainWindow.pages["game"] = new Game();
                }
                ((Game)MainWindow.pages["game"]).LoadFromJson();
                MainWindow.GoToPage("game");
                ((Game)MainWindow.pages["game"]).Start();
            }
        }
    }
}

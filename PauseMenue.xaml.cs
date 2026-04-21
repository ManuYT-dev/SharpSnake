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
    public partial class PauseMenue : Page
    {
        public PauseMenue()
        {
            InitializeComponent();
        }

        private void Button_Continue_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GoToPage("game");
            ((Game)MainWindow.pages["game"]).Start();
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            ((Game)MainWindow.pages["game"]).SaveToJson();
        }

        private void Button_MainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GoToPage("homepage");
        }
    }
}


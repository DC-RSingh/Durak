using Client.ViewModels;
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

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for MainPageView.xaml
    /// </summary>
    public partial class MainPageView : UserControl
    {
        public MainPageView()
        {
            InitializeComponent();
        }
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            GameView gameView = new GameView();
            DataContext = gameView;
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutView aboutView = new AboutView();
            DataContext = aboutView;
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.pagat.com/beating/podkidnoy_durak.html");
        }

        private void GameOptions_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExitMenu_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();
        }
        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainPageView mainPage = new MainPageView();
            DataContext = mainPage;
            
        }
    }
}

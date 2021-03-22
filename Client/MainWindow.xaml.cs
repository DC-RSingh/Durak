using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CardLib;
using CardUI;
using Client.ViewModels;
using Client.Views;


namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            var icoPath = Directory.GetParent(Environment.CurrentDirectory).
                Parent.
                FullName; // Really Scuffed Icon Path Retrieval
            ConsoleManager.SetConsoleIcon(new Icon($"{icoPath}/icon.ico"));
            InitializeComponent();
            Logger.Start();

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
            this.Close();
        }
        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainPageView mainPage = new MainPageView();
            DataContext = mainPage;

        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            ConsoleManager.Show();  // Show the Console
            var icoPath = Directory.GetParent(Environment.CurrentDirectory).
                Parent.
                FullName; // Really Scuffed Icon Path Retrieval
            ConsoleManager.SetConsoleIcon(new Icon($"{icoPath}/icon.ico"));
            InitializeComponent();
            Logger.Start(); // Start Logging
            
            GameplayTest.Play();
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new GameViewModel();
            InitializeComponent();
            //TODO: RESET GAME 
            //ResetGame();
            GameView gameView = new GameView();
            gameView.Play();

            //Delete Game object
            //Recreate
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new AboutViewModel();
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.pagat.com/beating/podkidnoy_durak.html");
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            //Add a MainMenu View 
            //DataContext = new GameViewModel();
        }

    }
}

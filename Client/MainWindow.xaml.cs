using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
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
            InitializeComponent();
            Logger.Start();
        }

        private void PlayMenu_Click(object sender, RoutedEventArgs e)
        {
            GameView gameView = new GameView();
            DataContext = gameView;
        }

        private void AboutMenu_Click(object sender, RoutedEventArgs e)
        {
            AboutView aboutView = new AboutView();
            DataContext = aboutView;
        }

        private void HelpMenu_Click(object sender, RoutedEventArgs e)
        {
            HelpView helpWindow = new HelpView {Name = "GameRules"};

            helpWindow.Show();
        }

        private void GameOptionsMenu_Click(object sender, RoutedEventArgs e)
        {
            GameSettingsView gameSettingsView = new GameSettingsView();
            DataContext = gameSettingsView;
        }
        private void ExitMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainPageView dataContext = new MainPageView();
            DataContext = dataContext;

        }
    }
}

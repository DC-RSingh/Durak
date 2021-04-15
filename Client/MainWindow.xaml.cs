using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
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
            InitializeComponent();
            Closing += MainWindow_Closing;
            Logger.Start();
            Statistics.ParseOrCreateStatsFile();
        }

        private void PlayMenu_Click(object sender, RoutedEventArgs e)
        {
            if (AbortGamePrompt() == MessageBoxResult.No) return;
            GameView gameView = new GameView(Statistics.DeckSize, Statistics.PlayerName);
            DataContext = gameView;
        }

        private void AboutMenu_Click(object sender, RoutedEventArgs e)
        {
            if (AbortGamePrompt() == MessageBoxResult.No) return; 
            AboutView aboutView = new AboutView();
            DataContext = aboutView;
        }

        private void HelpMenu_Click(object sender, RoutedEventArgs e)
        {
            //if (AbortGamePrompt() == MessageBoxResult.No) return;
            // TODO: HelpView is still using old empty view, brings up new window and does not change
            HelpView helpWindow = new HelpView {Name = "GameRules"};

            helpWindow.Show();
        }

        private void GameOptionsMenu_Click(object sender, RoutedEventArgs e)
        {
            if (AbortGamePrompt() == MessageBoxResult.No) return;
            GameSettingsView gameSettingsView = new GameSettingsView();
            DataContext = gameSettingsView;
        }
        private void ExitMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            if (AbortGamePrompt() == MessageBoxResult.No) return;
            MainPageView dataContext = new MainPageView();
            DataContext = dataContext;
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (AbortGamePrompt() == MessageBoxResult.No)
            {
                e.Cancel = true;
                return;
            }

            Statistics.UpdateGame();
        }

        private static MessageBoxResult AbortGamePrompt()
        {
            if (GameViewModel.GameInProgress) 
                return MessageBox.Show("Abort your current game? This game will still be added to your statistics.", "Abort Game?",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning);

            return MessageBoxResult.Cancel;
        }
    }
}

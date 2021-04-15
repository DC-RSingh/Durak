using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using CardUI;
using Client.ViewModels;
using Client.Views;
using Logging;

// TODO: Make app single instance?
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
            DataContextChanged += (sender, args) => { GameViewModel.GameInProgress = false; };
            Logger.Start();
            Logger.Log("Program launched...", LoggingLevel.Log, typeof(MainWindow));
            Statistics.ParseOrCreateStatsFile();
        }

        private void PlayMenu_Click(object sender, RoutedEventArgs e)
        {
            //CheckIfGameExists();
            GameView gameView = new GameView(Statistics.DeckSize, Statistics.PlayerName);
            DataContext = gameView;
        }

        private void AboutMenu_Click(object sender, RoutedEventArgs e)
        {
            //CheckIfGameExists();
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
            //CheckIfGameExists();
            GameSettingsView gameSettingsView = new GameSettingsView();
            DataContext = gameSettingsView;
        }
        private void ExitMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            //CheckIfGameExists();
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

            Logger.Log("Game of Durak aborted!", source: DataContext.GetType());

        }

        private static MessageBoxResult AbortGamePrompt()
        {
            if (GameViewModel.GameInProgress) 
                return MessageBox.Show("Abort your current game? This game will not be added to your statistics.", "Abort Game?",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning);

            return MessageBoxResult.Cancel;
        }

        private void CheckIfGameExists()
        {
            if (AbortGamePrompt() == MessageBoxResult.No) return;
            Logger.Log("Game of Durak aborted!", source: DataContext.GetType());
        }
    }
}

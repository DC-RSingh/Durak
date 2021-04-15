/**
 * OOP 4200 - Final Project - Durak
 * 
 * MainWindow.xaml.cs supports MainWindow.xaml. It displays the options the user has to navigate 
 * through the different views of the application
 * 
 * @author      Raje Singh, Fleur Blanckaert, Gabriel Dietrich, Dalton Young
 * @version     1.0
 * @since       2021-03 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using CardUI;
using Client.ViewModels;
using Client.Views;
using Logging;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes the MainWindow
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += MainWindow_Closing;
            DataContextChanged += (sender, args) => { GameViewModel.GameInProgress = false; };
            Logger.Start();
            Logger.Log("Program launched...", LoggingLevel.Log, typeof(MainWindow));
            Statistics.ParseOrCreateStatsFile();
        }

        /// <summary>
        /// Takes the user to PlayMenu when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayMenu_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfGameExists()) return;
            GameView gameView = new GameView(Statistics.DeckSize, Statistics.PlayerName);
            DataContext = gameView;
        }

        /// <summary>
        /// Takes the user to the About menu when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutMenu_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfGameExists()) return;
            AboutView aboutView = new AboutView();
            DataContext = aboutView;
        }

        /// <summary>
        /// Takes the user to the Help menu when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpMenu_Click(object sender, RoutedEventArgs e)
        {
            HelpView helpWindow = new HelpView {Name = "GameRules"};
            helpWindow.Show();
        }

        /// <summary>
        /// Takes the user to the Game options menu wehn clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameOptionsMenu_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfGameExists()) return;
            GameSettingsView gameSettingsView = new GameSettingsView();
            DataContext = gameSettingsView;
        }

        /// <summary>
        /// Exits the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Takes the user to the main menu when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfGameExists()) return;
            MainPageView dataContext = new MainPageView();
            DataContext = dataContext;
        }

        /// <summary>
        /// Closes the main window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (AbortGamePrompt() == MessageBoxResult.No)
            {
                e.Cancel = true;
                return;
            }

            Logger.Log("Game of Durak aborted!", source: typeof(MainWindow));

        }

        /// <summary>
        /// Aborts the game
        /// </summary>
        /// <returns>Retrns Message box result</returns>
        private static MessageBoxResult AbortGamePrompt()
        {
            if (GameViewModel.GameInProgress) 
                return MessageBox.Show("Abort your current game? This game will not be added to your statistics.", "Abort Game?",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning);

            return MessageBoxResult.Cancel;
        }

        /// <summary>
        /// Checks if game is still going on
        /// </summary>
        /// <returns></returns>
        private bool CheckIfGameExists()
        {
            if (AbortGamePrompt() == MessageBoxResult.No) return true;
            Logger.Log("Game of Durak aborted!", source: typeof(MainWindow));
            return false;
        }
    }
}

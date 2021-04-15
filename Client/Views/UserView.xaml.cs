/**
 * OOP 4200 - Final Project - Durak
 * 
 * UserView.xaml.cs supports UserView.xaml. In this view the user is asked to enter name, however they do
 * have the options to skip and start a game with a default name, simply by clicking the Skip button
 * 
 * @author      Raje Singh, Fleur Blanckaert, Gabriel Dietrich, Dalton Young
 * @version     1.0
 * @since       2021-04 
 */

using System;
using System.Windows;
using System.Windows.Controls;
using CardUI;
using Client.ViewModels;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : UserControl
    {
        /// <summary>
        /// Initializes the UserView
        /// </summary>
        public UserView()
        {
            InitializeComponent();
            this.DataContext = null;
        }

        /// <summary>
        /// Validates player name, updates the player name, and starts the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            var nameString = string.IsNullOrWhiteSpace(tbName.Text)
                ? tbName.Text
                : tbName.Text.Substring(0, Math.Min(tbName.Text.Length, 40));
            
            if (nameString == string.Empty)
            {
                
                if (tbName.Text == string.Empty)
                {
                    lblError.Content = "Please enter your name.";
                    return;
                }
                nameString = Statistics.PlayerName;
            }
            Statistics.UpdatePlayerName(nameString);
            GameView gameView = new GameView(Statistics.DeckSize, Statistics.PlayerName);
            DataContext = gameView;
        }

        /// <summary>
        /// Starts the game with the default player name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Skip_Click(object sender, RoutedEventArgs e)
        {
            GameView gameView = new GameView(Statistics.DeckSize, Statistics.PlayerName);
            DataContext = gameView;
        }
    }
}

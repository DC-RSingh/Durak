/**
 * OOP 4200 - Final Project - Durak
 * 
 * GameSettingsView.xaml.cs supports GameSettingsView.xaml. It allows the user to choose the number of cards
 * they want the playing deck to contain
 * 
 * @author      Raje Singh, Fleur Blanckaert, Gabriel Dietrich, Dalton Young
 * @version     1.0
 * @since       2021-04 
 */

using CardLib;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using CardUI;
using Client.ViewModels;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for GameSettingsView.xaml. 
    /// </summary>
    public partial class GameSettingsView : UserControl
    {
        /// <summary>
        /// Initializes the GameSettingsView
        /// </summary>
        public GameSettingsView()
        {
            InitializeComponent();
            this.DataContext = null;
        }

        /// <summary>
        /// Gets and sets ParentControl
        /// </summary>
        public UserControl ParentControl { get; set; }

        /// <summary>
        /// Determines what happens when the user clicks on the Play button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            var chosenDeckSize = DeckSize.ThirtySix;


            if (rb1.IsChecked == true)
            {
                chosenDeckSize = DeckSize.FiftyTwo;
            }
            else if (rb2.IsChecked == true)
            {
                chosenDeckSize = DeckSize.ThirtySix;
            }
            else if (rb3.IsChecked == true)
            {
                chosenDeckSize = DeckSize.Twenty;
            }

            Statistics.UpdateSettings(chosenDeckSize);

            GameView gameView = new GameView(Statistics.DeckSize, Statistics.PlayerName);
            DataContext = gameView;
        }
    }
}

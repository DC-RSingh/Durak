/**
 * OOP 4200 - Final Project - Durak
 * 
 * MainPageView.xaml.cs supports MainPageView.xaml. It is the first window that appers when the application
 * starts and it give the user the options to select Play, About, Game Options, Help, and Statistics
 * 
 * @author      Raje Singh, Fleur Blanckaert, Gabriel Dietrich, Dalton Young
 * @version     1.0
 * @since       2021-03 
 */

using Client.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for MainPageView.xaml
    /// </summary>
    public partial class MainPageView : UserControl
    {
        /// <summary>
        /// Initializes the MainPageView
        /// </summary>
        public MainPageView()
        {
            InitializeComponent();
            this.DataContext = null;
        }

        /// <summary>
        /// Starts the game when the user clicks on the play button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            UserView userView = new UserView();
            DataContext = userView;
        }

        /// <summary>
        /// Opens the AboutView when the user click on the about button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutView aboutView = new AboutView();
            DataContext = aboutView;
        }

        /// <summary>
        /// Opens the GameSettingsView when the user clicks on the Game Options button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameOptions_Click(object sender, RoutedEventArgs e)
        {
            GameSettingsView gameSettingsView = new GameSettingsView();
            DataContext = gameSettingsView;
        }

        /// <summary>
        /// Opens the MainHelpView when the user clicks on the Help button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            MainHelpView helpWindow = new MainHelpView();
            DataContext = helpWindow;
        }

        /// <summary>
        /// Opens the StatisticsView when the user clicks on the Statistics button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Statistics_Click(object sender, RoutedEventArgs e)
        {
            StatisticsView statisticsView = new StatisticsView();
            DataContext = statisticsView;
        }
    }
}

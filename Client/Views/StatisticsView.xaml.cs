/**
 * OOP 4200 - Final Project - Durak
 * 
 * StatisticsView.xaml.cs supports the StatisticsView.xaml. It display the number of games played,
 * including how many games were won, lost, the ties, and the name of player.
 * 
 * @author      Raje Singh, Fleur Blanckaert, Gabriel Dietrich, Dalton Young
 * @version     1.0
 * @since       2021-04 
 */

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using CardUI;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for StatisticsView.xaml
    /// </summary>
    public partial class StatisticsView : UserControl
    {
        /// <summary>
        /// Initializes the StatisticsView
        /// </summary>
        public StatisticsView()
        {
            InitializeComponent();            
            this.DataContext = null;
            GenerateStatistics();
        }

        /// <summary>
        /// Resets all the statistics back to zero
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            Statistics.Reset();
            GenerateStatistics();
        }

        /// <summary>
        /// This method will generate statistics
        /// </summary>
        private void GenerateStatistics()
        {
            Statistics.ParseOrCreateStatsFile();
            lbl1.Content = $"{Statistics.PlayerName}";
            lblWins.Content = $"Number of Wins: {Statistics.PlayerWins}";
            lblLosses.Content = $"Number of Losses: {Statistics.PlayerLosses}";
            lblTies.Content = $"Number of Ties: {Statistics.PlayerTies}";
            lblTotal.Content = $"Number of Games Played: {Statistics.PlayerTotal}";
            
        }
    }
}

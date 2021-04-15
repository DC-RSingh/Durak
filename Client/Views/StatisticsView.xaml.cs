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
        public StatisticsView()
        {
            InitializeComponent();
            this.DataContext = null;
            GenerateStatistics();
        }
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("This will reset all of your stats (including your name). Are you sure?", "Reset Stats?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.No) return;
            Statistics.Reset();
            GenerateStatistics();
            MessageBox.Show("Statistics Have been Reset!", "Stats Reset!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        /// <summary>
        /// This method will generate statistics
        /// </summary>
        private void GenerateStatistics()
        {
            lbl1.Content = $"{Statistics.PlayerName}";
            lblWins.Content = $"Number of Wins: {Statistics.PlayerWins}";
            lblLosses.Content = $"Number of Losses: {Statistics.PlayerLosses}";
            lblTies.Content = $"Number of Ties: {Statistics.PlayerTies}";
            lblTotal.Content = $"Number of Games Played: {Statistics.PlayerTotal}";
        }
    }
}

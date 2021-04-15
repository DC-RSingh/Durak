using System;
using System.IO;
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
            Statistics.Reset();
            StatisticsView statisticsView = new StatisticsView();
            DataContext = statisticsView;
        }

        /// <summary>
        /// This method will generate statistics
        /// </summary>
        private void GenerateStatistics()
        {
        // Stores file path in the system
        var pwd = Directory.GetCurrentDirectory();
        var fileName = System.IO.Path.Combine(pwd, "GameLog");
        var read = System.IO.Path.Combine(fileName, "statistics.txt");
          

        if (File.Exists(read))
        {
            // Store each line in array of strings
            string[] option = File.ReadAllLines(read);
            lbl1.Content = $"{option[0]}";
            lblWins.Content = $"Number of Wins: {option[1]}";
            lblLosses.Content = $"Number of Losses: {option[2]}";
            lblTies.Content = $"Number of Ties: {option[3]}";
            lblTotal.Content = $"Number of Games Played: {option[4]}";
        }
        
        }
    }
}

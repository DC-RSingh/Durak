using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            Statistics();
        }

        private void Statistics()
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

            // By using StreamReader
            if (File.Exists(read))
            {
                // Reads file line by line
                StreamReader Textfile = new StreamReader(read);
                string line;

                while ((line = Textfile.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }

                Textfile.Close();

                Console.Read();
            }
            Console.WriteLine();
        }

        public void UpdateNewGameStats()
        {

            var pwd = Directory.GetCurrentDirectory();
            var fileName = System.IO.Path.Combine(pwd, "GameLog");
            var textFile = System.IO.Path.Combine(fileName, "statistics.txt");
            // Store each line in array of strings
            string[] option = File.ReadAllLines(textFile);

            
            if (File.Exists(textFile))
            {
                
                var name = option[0];
                var wins = option[1];
                var losses = option[2];
                var ties = option[3];
                var total = option[4];

                // Create the file and use streamWriter to write text to it.
                //If the file existence is not check, this will overwrite said file.
                //Use the using block so the file can close and vairable disposed correctly
                using (StreamWriter writer = File.CreateText(textFile))
                {
                    writer.WriteLine(Int32.Parse(wins) + 1);
                }
            }          


        }
    }
}

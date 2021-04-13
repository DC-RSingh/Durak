using CardLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CardUI
{
    class Statistics
    {


        Label lblWins;
        #region METHODS

        private void Generate(Label lblWins, Label lblLosses, Label lblTies, Label lblTotal)
        {
            // Stores file path in the system
            var pwd = Directory.GetCurrentDirectory();
            var fileName = System.IO.Path.Combine(pwd, "GameLog");
            var read = System.IO.Path.Combine(fileName, "game_statistics.txt");
            var readWins = System.IO.Path.Combine(fileName, "wins.txt");
            var readLosses = System.IO.Path.Combine(fileName, "losses.txt");
            var readTies = System.IO.Path.Combine(fileName, "ties.txt");
            var readTotal = System.IO.Path.Combine(fileName, "total.txt");

            //// Reads entire file
            //if (File.Exists(readWins))
            //{
            //    // Reads and displays content
            //    string streamWins = File.ReadAllText(readWins);
            //    Console.WriteLine(streamWins);
            //}

            // To read a text file line by line
            if (File.Exists(readWins))
            {
                // Store each line in array of strings
                string[] linesWins = File.ReadAllLines(readWins);

                lblWins.Content = $"Number of Wins: {linesWins[0]}";
            }

            //// Reads entire file
            //if (File.Exists(readLosses))
            //{
            //    // Reads and displays content
            //    string streamLosses = File.ReadAllText(readLosses);
            //    Console.WriteLine(streamLosses);
            //}

            // To read a text file line by line
            if (File.Exists(readLosses))
            {
                // Store each line in array of strings
                string[] linesLosses = File.ReadAllLines(readLosses);

                lblLosses.Content = $"Number of Losses: {linesLosses[1]}";
            }

            //// Reads entire file
            //if (File.Exists(readTies))
            //{
            //    // Reads and displays content
            //    string streamTies = File.ReadAllText(readTies);
            //    Console.WriteLine(streamTies);
            //}

            // To read a text file line by line
            if (File.Exists(readTies))
            {
                // Store each line in array of strings
                string[] linesTies = File.ReadAllLines(readTies);

                lblTies.Content = $"Number of Ties: {linesTies[2]}";
            }

            //// Reads entire file
            //if (File.Exists(readTotal))
            //{
            //    // Reads and displays content
            //    string streamTotal = File.ReadAllText(readTotal);
            //    Console.WriteLine(streamTotal);
            //}

            // To read a text file line by line
            if (File.Exists(readTotal))
            {
                // Store each line in array of strings
                string[] linesTotal = File.ReadAllLines(readTotal);

                lblTotal.Content = $"Number of Games Played: {linesTotal[3]}";
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


            #endregion

        }
    }
}

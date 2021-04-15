using System;
using System.IO;

namespace CardUI
{
    public class Statistics
    {

        #region METHODS
        public static void UpdateGame()
        {
            var pwd = Directory.GetCurrentDirectory();
            var fileName = System.IO.Path.Combine(pwd, "Stats");
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
                    writer.WriteLine(name);
                    writer.WriteLine(wins);
                    writer.WriteLine(losses);
                    writer.WriteLine(ties);
                    writer.WriteLine(Int32.Parse(total) + 1);

                }
            }
        }

        public static void UpdateWins()
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
                    writer.WriteLine(name);
                    writer.WriteLine(Int32.Parse(wins) + 1);
                    writer.WriteLine(losses);
                    writer.WriteLine(ties);
                    writer.WriteLine(Int32.Parse(total) + 1);

                }
            }
        }

        public static void UpdateLosses()
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
                    writer.WriteLine(name);
                    writer.WriteLine(wins);
                    writer.WriteLine(Int32.Parse(losses) + 1);
                    writer.WriteLine(ties);
                    writer.WriteLine(Int32.Parse(total) + 1);

                }
            }
        }

        public static void UpdateTies()
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
                    writer.WriteLine(name);
                    writer.WriteLine(wins);
                    writer.WriteLine(losses);
                    writer.WriteLine(Int32.Parse(ties) + 1);
                    writer.WriteLine(Int32.Parse(total) + 1);
                }
            }
        }

        public static void Reset()
        {
            //string fileName = @"log/pregame_settings.txt";
            var pwd = Directory.GetCurrentDirectory();
            var fileName = System.IO.Path.Combine(pwd, "GameLog");
            var textFile = System.IO.Path.Combine(fileName, "statistics.txt");

            //Check if the file exists
            if (!Directory.Exists(fileName))
            {
                Directory.CreateDirectory(fileName);
            }

            // Create the file and use streamWriter to write text to it.
            //If the file existence is not check, this will overwrite said file.
            //Use the using block so the file can close and vairable disposed correctly
            using (StreamWriter writer = File.CreateText(textFile))
            {
                writer.WriteLine("");
                writer.WriteLine("0");
                writer.WriteLine("0");
                writer.WriteLine("0");
                writer.WriteLine("0");
            }
        }
        #endregion
    }
}
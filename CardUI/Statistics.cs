using System;
using System.IO;
using System.Windows;
using CardLib;

// TODO: Wrong library?
// TODO: Maybe more firm check of the file or serializable format?
// TODO: Parse or Create is pretty much required before every property called. Might want to
namespace CardUI
{
    public static class Statistics
    {

        #region FIELDS AND PROPERTIES

        public static string PlayerName { get; private set; } = "Player 1 (Human)";
        public static int PlayerWins { get; private set; } = 0;
        public static int PlayerLosses { get; private set; } = 0;
        public static int PlayerTies { get; private set; } = 0;
        public static int PlayerTotal { get; private set; } = 0;

        public static DeckSize DeckSize { get; private set; } = DeckSize.ThirtySix;

        private static readonly string _currentDir = Directory.GetCurrentDirectory();
        public static string StatsDirPath => System.IO.Path.Combine(_currentDir, "DurakStats");
        public static string StatsFilePath => System.IO.Path.Combine(StatsDirPath, "durak_statistics.txt");

        public static string SettingsFilePath => System.IO.Path.Combine(StatsDirPath, "pregame_settings.txt");

        #endregion

        #region PUBLIC METHODS

        public static void UpdateSettings(DeckSize newDeckSize)
        {
            DeckSize = newDeckSize;
            ParseOrCreateStatsFile();

            using (StreamWriter writer = File.CreateText(SettingsFilePath))
            {
                writer.WriteLine(DeckSize);
            }
        }

        public static void UpdatePlayerName(string name)
        {
            ParseOrCreateStatsFile();
            PlayerName = name;

            using (StreamWriter writer = File.CreateText(StatsFilePath))
            {
                writer.WriteLine(PlayerName);
                writer.WriteLine(PlayerWins);
                writer.WriteLine(PlayerLosses);
                writer.WriteLine(PlayerTies);
                writer.WriteLine(PlayerTotal);
            }
        }

        public static void UpdateGame()
        {
            ParseOrCreateStatsFile();

            using (StreamWriter writer = File.CreateText(StatsFilePath))
            {
                writer.WriteLine(PlayerName);
                writer.WriteLine(PlayerWins);
                writer.WriteLine(PlayerLosses);
                writer.WriteLine(PlayerTies);
                writer.WriteLine(PlayerTotal + 1);

            }

        }

        public static void UpdateWins()
        {
            ParseOrCreateStatsFile();

            using (StreamWriter writer = File.CreateText(StatsFilePath))
            {
                writer.WriteLine(PlayerName);
                writer.WriteLine(PlayerWins);
                writer.WriteLine(PlayerLosses);
                writer.WriteLine(PlayerTies);
                writer.WriteLine(PlayerTotal + 1);

            }
        }

        public static void UpdateLosses()
        {
            ParseOrCreateStatsFile();
            
            using (StreamWriter writer = File.CreateText(StatsFilePath))
            {
                writer.WriteLine(PlayerName);
                writer.WriteLine(PlayerWins);
                writer.WriteLine(PlayerLosses + 1);
                writer.WriteLine(PlayerTies);
                writer.WriteLine(PlayerTotal + 1);

            }

        }

        public static void UpdateTies()
        {

            ParseOrCreateStatsFile();
            
            using (StreamWriter writer = File.CreateText(StatsFilePath))
            {
                writer.WriteLine(PlayerName);
                writer.WriteLine(PlayerWins);
                writer.WriteLine(PlayerLosses);
                writer.WriteLine(PlayerTies + 1);
                writer.WriteLine(PlayerTotal + 1);
            }
        }

        public static void Reset()
        {
            ParseOrCreateStatsFile();

            using (var writer = File.CreateText(StatsFilePath))
            {
                writer.WriteLine("Player 1 (Human)");
                writer.WriteLine("0");
                writer.WriteLine("0");
                writer.WriteLine("0");
                writer.WriteLine("0");
            }
        }

        public static void ResetSettings()
        {
            ParseOrCreateStatsFile();

            using (StreamWriter writer = File.CreateText(SettingsFilePath))
            {
                writer.WriteLine(DeckSize.ThirtySix);
            }
        }

        /// <summary>
        /// Creates the Stats Directory if it does not exist. Also creates the stats file if it does not exist. If it does, parses the stats contained in the file
        /// and sets the static properties of the <see cref="Statistics"/> class.
        /// </summary>
        public static void ParseOrCreateStatsFile()
        {
            //Check if the dir exists
            if (!Directory.Exists(StatsDirPath))
            {
                Directory.CreateDirectory(StatsDirPath);
            }

            if (File.Exists(StatsFilePath))
            {
                // Store each line in array of strings
                string[] option = File.ReadAllLines(StatsFilePath);

                try
                {
                    // Parse Player Stats
                    PlayerName = option[0];
                    PlayerWins = int.Parse(option[1]);
                    PlayerLosses = int.Parse(option[2]);
                    PlayerTies = int.Parse(option[3]);
                    PlayerTotal = int.Parse(option[4]);
                }
                catch (Exception)
                {
                    MessageBox.Show($"{StatsFilePath} has been corrupted! Resetting...", "Corrupted Stats!", MessageBoxButton.OK, MessageBoxImage.Error);
                    Reset();
                }
            }
            else
            {
                // Create the file and use streamWriter to write text to it.
                //If the file existence is not check, this will overwrite said file.
                //Use the using block so the file can close and variable disposed correctly
                using (var writer = File.CreateText(StatsFilePath))
                {
                    writer.WriteLine("Player 1 (Human)");
                    writer.WriteLine("0");
                    writer.WriteLine("0");
                    writer.WriteLine("0");
                    writer.WriteLine("0");
                }
            }

            if (File.Exists(SettingsFilePath))
            {
                var deckSize = File.ReadAllLines(SettingsFilePath);

                try
                {
                    DeckSize = (DeckSize) Enum.Parse(typeof(DeckSize), deckSize[0]);
                }
                catch (Exception)
                {
                    MessageBox.Show($"{SettingsFilePath} has been corrupted! Resetting...", "Corrupted Settings!", MessageBoxButton.OK, MessageBoxImage.Error);
                    ResetSettings();
                }
            }
            else
            {
                using (StreamWriter writer = File.CreateText(SettingsFilePath))
                {
                    writer.WriteLine(DeckSize.ThirtySix);
                }
            }
        }

        #endregion
    }
}
/**
 * OOP 4200 - Final Project - Durak
 * 
 * Statistics.cs is class used to let the user know how many games were played, as well as number
 * of wins, losses, and ties, including the name of the user playing.
 * 
 * @author      Raje Singh, Fleur Blanckaert, Gabriel Dietrich, Dalton Young
 * @version     1.0
 * @since       2021-04 
 */

using System;
using System.IO;
using System.Windows;
using CardLib;

namespace CardUI
{
    public static class Statistics
    {

        #region FIELDS AND PROPERTIES

        /// <summary>
        /// Gets and sets the name of the player
        /// </summary>
        public static string PlayerName { get; private set; } = "Player 1 (Human)";
        
        /// <summary>
        /// Gets and sets the wins
        /// </summary>
        public static int PlayerWins { get; private set; } = 0;
        
        /// <summary>
        /// Gets and sets the losses
        /// </summary>
        public static int PlayerLosses { get; private set; } = 0;
        
        /// <summary>
        /// Gets and sets the ties
        /// </summary>
        public static int PlayerTies { get; private set; } = 0;

        /// <summary>
        /// Gets and sets the total number of games played
        /// </summary>
        public static int PlayerTotal { get; private set; } = 0;

        /// <summary>
        /// Gets and sets the deck size
        /// </summary>
        public static DeckSize DeckSize { get; private set; } = DeckSize.ThirtySix;

        /// <summary>
        /// Represents the current directory
        /// </summary>
        private static readonly string _currentDir = Directory.GetCurrentDirectory();

        /// <summary>
        /// Represents the Directory used for the stats
        /// </summary>
        public static string StatsDirPath => System.IO.Path.Combine(_currentDir, "DurakStats");

        /// <summary>
        /// Represents the text file used to store the stats
        /// </summary>
        public static string StatsFilePath => System.IO.Path.Combine(StatsDirPath, "durak_statistics.txt");

        /// <summary>
        /// Represents the text file to determine the pre game settings including name and deck size
        /// </summary>
        public static string SettingsFilePath => System.IO.Path.Combine(StatsDirPath, "pregame_settings.txt");

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Updates the settings
        /// </summary>
        /// <param name="newDeckSize">Represents the new deck size</param>
        public static void UpdateSettings(DeckSize newDeckSize)
        {
            ParseOrCreateStatsFile();
            DeckSize = newDeckSize;

            using (StreamWriter writer = File.CreateText(SettingsFilePath))
            {
                writer.WriteLine(DeckSize);
            }
        }

        /// <summary>
        /// Updates the player name
        /// </summary>
        /// <param name="name">Represents the name of the player</param>
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

        /// <summary>
        /// Updates the number of games played by adding 1 to the total
        /// </summary>
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

        /// <summary>
        /// Updates the number of wins by adding 1 to the total of wins
        /// </summary>
        public static void UpdateWins()
        {
            ParseOrCreateStatsFile();

            using (StreamWriter writer = File.CreateText(StatsFilePath))
            {
                writer.WriteLine(PlayerName);
                writer.WriteLine(PlayerWins + 1);
                writer.WriteLine(PlayerLosses);
                writer.WriteLine(PlayerTies);
                writer.WriteLine(PlayerTotal + 1);

            }
        }

        /// <summary>
        /// Updates the total losses by adding one to the total of losses
        /// </summary>
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

        /// <summary>
        /// Updates the ties by adding one to the total of ties
        /// </summary>
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

        /// <summary>
        /// Resets the statistics count, placing them back to 0
        /// </summary>
        public static void Reset()
        {
            ParseOrCreateStatsFile();
            PlayerName = "Player 1 (Human)";
            PlayerWins = 0;
            PlayerLosses = 0;
            PlayerTies = 0;
            PlayerTotal = 0;

            using (var writer = File.CreateText(StatsFilePath))
            {
                writer.WriteLine("Player 1 (Human)");
                writer.WriteLine("0");
                writer.WriteLine("0");
                writer.WriteLine("0");
                writer.WriteLine("0");
            }
        }

        /// <summary>
        /// Makes use of StreamWriter to reset the statistics
        /// </summary>
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
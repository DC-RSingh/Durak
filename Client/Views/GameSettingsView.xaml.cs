﻿using CardLib;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Client.ViewModels;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for GameSettingsView.xaml
    /// </summary>
    public partial class GameSettingsView : UserControl
    {
        public GameSettingsView()
        {
            InitializeComponent();
            this.DataContext = null;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            //string fileName = @"log/pregame_settings.txt";
            var pwd = Directory.GetCurrentDirectory();
            var fileName = System.IO.Path.Combine(pwd, "GameLog");
            var textFile = System.IO.Path.Combine(fileName, "pregame_settings.txt");
            var chosenDeckSize = DeckSize.ThirtySix;

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

                writer.WriteLine(chosenDeckSize);
            }

            GameView gameView = new GameView(chosenDeckSize);
            DataContext = gameView;


        }
    }
}

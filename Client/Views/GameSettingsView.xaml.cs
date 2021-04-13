﻿using CardLib;
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
    /// Interaction logic for GameSettingsView.xaml
    /// </summary>
    public partial class GameSettingsView : UserControl
    {
        public GameSettingsView()
        {
            InitializeComponent();
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {

           

            //string fileName = @"log/pregame_settings.txt";
            var pwd = Directory.GetCurrentDirectory();
            var fileName = System.IO.Path.Combine(pwd, "GameLog");
            var textFile = System.IO.Path.Combine(fileName, "pregame_settings.txt");

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
                    writer.WriteLine(DeckSize.FiftyTwo);
                }
                else if (rb2.IsChecked == true)
                {
                    writer.WriteLine(DeckSize.ThirtySix);
                }
                else if (rb3.IsChecked == true)
                {
                    writer.WriteLine(DeckSize.Twenty);
                }
            }

            GameView gameView = new GameView();
            DataContext = gameView;


        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using CardUI;
using Client.Views;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            Closing += MainWindow_Closing;
            Logger.Start();
            Statistics.ParseOrCreateStatsFile();
        }

        private void PlayMenu_Click(object sender, RoutedEventArgs e)
        {
            GameView gameView = new GameView(Statistics.DeckSize, Statistics.PlayerName);
            DataContext = gameView;
        }

        private void AboutMenu_Click(object sender, RoutedEventArgs e)
        {
            AboutView aboutView = new AboutView();
            DataContext = aboutView;
        }

        private void HelpMenu_Click(object sender, RoutedEventArgs e)
        {
            HelpView helpWindow = new HelpView();

            helpWindow.Name = "GameRules";

            helpWindow.Show();
        }

        private void GameOptionsMenu_Click(object sender, RoutedEventArgs e)
        {
            GameSettingsView gameSettingsView = new GameSettingsView();
            DataContext = gameSettingsView;
        }
        private void ExitMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainPageView dataContext = new MainPageView();
            DataContext = dataContext;
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!(DataContext is GameView)) return;

            if (AbortGamePrompt() == MessageBoxResult.No)
            {
                e.Cancel = true;
                return;
            }

            Statistics.UpdateGame();
        }

        public static MessageBoxResult AbortGamePrompt()
        {
            return MessageBox.Show("Abort your current game? This game will still be added to your statistics.", "Abort Game?",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);
        }
    }
}

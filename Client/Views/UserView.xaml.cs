using System;
using System.Windows;
using System.Windows.Controls;
using CardUI;
using Client.ViewModels;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : UserControl
    {
        public UserView()
        {
            InitializeComponent();
            this.DataContext = null;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            var nameString = string.IsNullOrWhiteSpace(tbName.Text)
                ? tbName.Text
                : tbName.Text.Substring(0, Math.Min(tbName.Text.Length, 40));
            // TODO: Could just use the value from the file when the press ok without giving a name.
            if (nameString == string.Empty)
            {
                var result = MessageBox.Show("You did not enter a name! Continue without entering a name?",
                    "Continue without Entering Name?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No) return;
                nameString = Statistics.PlayerName;
            }
            Statistics.UpdatePlayerName(nameString);
            GameView gameView = new GameView(Statistics.DeckSize, Statistics.PlayerName);
            DataContext = gameView;
        }
    }
}

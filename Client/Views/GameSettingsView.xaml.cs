using CardLib;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using CardUI;
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

        public UserControl ParentControl { get; set; }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            var chosenDeckSize = DeckSize.ThirtySix;


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

            Statistics.UpdateSettings(chosenDeckSize);

            GameView gameView = new GameView(Statistics.DeckSize, Statistics.PlayerName);
            DataContext = gameView;
        }
    }
}

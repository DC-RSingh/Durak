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
            var nameString = tbName.Text;
            // TODO: Could just use the value from the file when the press ok without giving a name.
            if (tbName.Text.Trim() == string.Empty)
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

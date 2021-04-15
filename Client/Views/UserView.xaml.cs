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

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            var nameString = tbName.Text;
            if (tbName.Text.Trim() == string.Empty)
            {                
                if (tbName.Text == string.Empty)
                {
                    lblError.Content = "Please enter your name.";
                    return;
                }
                nameString = Statistics.PlayerName;
            }
            Statistics.UpdatePlayerName(nameString);
            GameView gameView = new GameView(Statistics.DeckSize, Statistics.PlayerName);
            DataContext = gameView;
        }

        private void Skip_Click(object sender, RoutedEventArgs e)
        {
            GameView gameView = new GameView(Statistics.DeckSize, Statistics.PlayerName);
            DataContext = gameView;
        }
    }
}

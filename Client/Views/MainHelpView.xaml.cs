using System;
using System.Collections.Generic;
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
    /// Interaction logic for MainHelpView.xaml
    /// </summary>
    public partial class MainHelpView : UserControl
    {
        public MainHelpView()
        {
            InitializeComponent();
            GenerateText();
            this.DataContext = null;
        }

        private void GenerateText()
        {
            txtObjective.Text = "The object of the game is for a player to get rid of all his/her " +
                                "cards before the other player. Whoever is left with cards on their  " +
                                "hands at the end of the game is the ‘Durak’ (“fool” in Russian).";

            txtGameSetup.Text = "Durak can be played with a deck of 20, 36, or 52 cards. " +
                                "After the application shuffles the cards on the deck, 6 cards are " +
                                "distributed randomly to each player.The rest of the deck is placed on a" +
                                " draw pile.The top card is flipped over to determine the trump suit for the " +
                                "round and placed under the draw pile in a way that it can be seen.";


        }
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            GameView gameView = new GameView();
            DataContext = gameView;
        }

    }
}

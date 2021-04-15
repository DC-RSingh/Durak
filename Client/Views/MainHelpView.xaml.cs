using System.Windows;
using System.Windows.Controls;
using CardUI;

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
            txtHeader1.Text = "Game Objectives";
            txtObjective.Text = "The object of the game is for a player to get rid of all his/her " +
                "cards before the other player. Whoever is left with cards on their  " +
                "hands at the end of the game is the ‘Durak’ (“fool” in Russian).";

            txtHeader2.Text = "\nGame Setup";
            txtGameSetup.Text = "Durak can be played with a deck of 20, 36, or 52 cards. " +
                "After the application shuffles the cards on the deck, 6 cards are " +
                "distributed randomly to each player.The rest of the deck is placed on a" +
                " draw pile.The top card is flipped over to determine the trump suit for the " +
                "round and placed under the draw pile in a way that it can be seen.";

            txtHeader3.Text = "\nGame Play";
            txtGamePlay.Text = "•The application randomly picks a player to be the attacker, and consequently the other player will be the defender. " +
                "\n\n•An attack consists of a player playing a card into the middle. A defense is made by beating the card played. An attack card can be " +
                "beaten by a higher - ranking card in the same suit, or any trump card if the attack card is a non - trump suited card.\n\n•If an attack is " +
                "successfully defended, another attack can be made, but the new attack card must be the same rank as a previously played card(i.e., the same rank " +
                "as the first attack card or the defending card).\n\n•The attacker can keep attacking as long as they have cards in their hands(e.g., keeps clicking on " +
                "playable cards from their hand when it is their turn), however the attack may end before that if the attacker cannot or choose not to attack further(click " +
                "the “Pass” button).\n\n•When a defender wins, the cards from the middle are discarded. If a defender cannot defend an attack, all the cards from the middle are " +
                "added to the player’s hand and the player loses his / her turn to attack.After the attacker passed their turn, the defender then becomes the attacker.\n\n•Before the " +
                "next attack begins, any player under six cards will have cards automatically drawn from the remaining deck to get back to six cards on hand. Once the remaining deck runs out, " +
                "o card draw occurs after an attack.\n\n•A defender can choose not to play on an attacking card(clicking the “Pass” button) even if the defender is able to beat the card played. The " +
                "application automatically adds the cards in the middle to the defender’s hand and the player loses the turn to attack.";

            txtHeader4.Text = "\r\nReferences";
            txtReferences.Text = "How to play durak. (n.d.). Retrieved April 15, 2021, from https://gathertogethergames.com/durak. \r\n\nBall, M. (2021, January 14). 2 PLAYER DURAK - Learn To Play With " +
                "Gamerules.com. Game Rules. https://gamerules.com/rules/2-player-durak/#:%7E:text=In%20Durak%2C%20each%20trick%20is,their%20hand%20to%20lead%20first. \n\n";

        }
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            GameView gameView = new GameView(Statistics.DeckSize, Statistics.PlayerName);
            DataContext = gameView;
        }

    }
}

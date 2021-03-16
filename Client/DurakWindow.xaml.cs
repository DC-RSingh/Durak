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
using System.Windows.Shapes;
using CardLib;
using CardLib.CardLib;
using CardUI;

namespace Client
{
    /// <summary>
    /// Interaction logic for DurakWindow.xaml
    /// </summary>
    public partial class DurakWindow : Window
    {
        //PictureBox[] cards;
        private Deck<PlayingCard> playDeck;
        private Cards dicardPile;   // Where Discarded Cards Go
        private Cards inPlayPile;   // Where the attacking and defending cards go
        private Player[] players;

        public DurakWindow()
        {
            InitializeComponent();
            //DisplayHand();
            Play();

        }

        public void Play()
        {
            #region Deck and Discard Pile Init
            var deck = new Deck<PlayingCard>();

            var discardPile = new Cards();

            var river = new Cards();

            deck.Shuffle();

            #endregion

            #region Player 1 Init
            var player1 = new Player("Test Player", deck.Draw(6));



            DisplayPlayer(player1);

            #endregion

            #region Player 2 Init
            var player2 = new Player("AI", deck.Draw(6));

            //DisplayPlayer(player2);
        }
            #endregion
            // Testing Displaying the Card
            private void DisplayHand()
            {
            // Create Face Up Card
            var test_card = new PlayingCard(Suit.Club, Rank.King, Face.Up);

            imgPlayingCard = new Image { Source = test_card.GetImage() };

            // TODO: Can do a lot with this, crop it and what not with CroppedBitMap
            // Refer to: https://www.c-sharpcorner.com/UploadFile/mahesh/using-xaml-image-in-wpf/
            // Create Image to Add to Form, set Dimensions, Source
            //var dynamicImage = new Image {Width = 70, Height = 107, Source = test_card.GetImage()};
            imgPlayingCard = new Image { Width = 70, Height = 107, Source = test_card.GetImage() };

            imgPlayingCard.MouseDown += (sender, args) =>
             {
                 imgPlayingCard.Margin = new Thickness(0,-300,0,0);
                 MessageBox.Show("You played " + test_card.ToString());
             };


            // Add Image to Form
            DurakRoot.Children.Add(imgPlayingCard);
        }

        private void DisplayPlayer(Player player1)
        {
            int x = -50;
            int y = -50;
            for (int i = 0; i < player1.Hand.Count; i++)
            {
                //How to extract rank and suit from cards?
                PlayingCard test_card = new PlayingCard(player1.Hand.ElementAt(i).Suit, player1.Hand.ElementAt(i).Rank, player1.Hand.ElementAt(i).Face);

                Image img = new Image { Width = 70, Height = 107, Source = test_card.GetImage() };
                
                img.Margin = new Thickness(x, 0, 0, 0);
                x += 50;
                DurakRoot.Children.Add(img);

                img.MouseLeftButtonDown += (sender, args) =>
                {
                    img.Margin = new Thickness(y, x, 0, 0);
                    y += -50;
                    MessageBox.Show("You played " + test_card.ToString());
                };

            }
        }

        private static void DisplayDeck(Deck<CardBase> s)
        {
            for (var i = 0; i < s.Size; i++)
            {
                MessageBox.Show(s.GetCard(i).ToString());
            }
        }
    }

}


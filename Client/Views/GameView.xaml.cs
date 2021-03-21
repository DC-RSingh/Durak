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
using CardLib;
using CardLib.CardLib;
using CardUI;
using Client.ViewModels;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : UserControl
    {
        public GameView()
        {
            
            InitializeComponent();
            Play();

        }

        #region FIELDS AND PROPERTIES

        /// <summary>
        /// Delclares a deck of cards
        /// </summary>
        private Deck<PlayingCard> playDeck;
        
        /// <summary>
        /// Declares a new player 
        /// </summary>
        private Player[] players;

        /// <summary>
        /// The amount, in points, that CardBox controls are enlarged when hovered over. 
        /// </summary>
        private const int POP = 25;

        /// <summary>
        /// The height of an image box control 
        /// </summary>
        private const int regularHeight = 107;

        /// <summary>
        /// The width of a image box control 
        /// </summary>
        private const int regularWidth = 75;

        /// <summary>
        /// The regular size of a CardBox control
        /// </summary>
        static private Size regularSize = new Size(75, 107);


        #endregion EVENT HANDLERS
        #region
        #endregion


 
        #region METHODS


        public void Play()
        {
            #region Deck and Discard Pile Init
            var deck = new Deck<PlayingCard>();
            deck.Shuffle();
            #endregion

            #region Player 1 Init
            var player1 = new Player("Test Player", deck.Draw(6));
            DisplayHand(player1);
            #endregion

            #region Player 2 Init
            var player2 = new Player("AI", deck.Draw(6));
            DisplayHand(player2);
            #endregion
        }

        // Testing Displaying the Card
        private void DisplayHand(Player player)
        {
            //Canvas myCanvas = new Canvas();
            Canvas canvas = pnlPlayerHand;

            for (int i = 0; i < player.Hand.Count; i++)
            {
                // Turn AI cards down
                if (player.PlayerName == "AI")
                {
                    player.Hand.ElementAt(i).Face = Face.Down;
                    canvas = pnlAIHand;
                }

                PlayingCard test_card = new PlayingCard(player.Hand.ElementAt(i).Suit, player.Hand.ElementAt(i).Rank, player.Hand.ElementAt(i).Face);

                //Dynamically creates a new image
                Image img = new Image { Width = regularWidth, Height = regularHeight, Source = test_card.UpdateCardImage() };

                //Sets image quality to high
                RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);

                //Adds image to the canvas panel
                canvas.Children.Add(img);

                img.MouseEnter += (sender, args) =>
                {
                    // Convert sender to a CardBox
                    img = sender as Image;

                    // If the conversion worked
                    if (img != null)
                    {
                        // if the card is in the home panel...
                        if (img.Parent == canvas)
                        {
                            // Enlarge the card for visual effect
                            img.Height += POP;
                            img.Width += POP;

                            // move the card to the top edge of the panel.
                            Canvas.SetTop(img, 0);
                        }
                        else
                        {
                            //TODO: Disable images
                            DisableImage(img);
                        }
                    }
                };

                img.MouseLeave += (sender, args) =>
                {
                    // Enlarge the card for visual effect
                    img.Height = regularHeight;
                    img.Width = regularWidth;

                    // move the card to the top edge of the panel.
                    Canvas.SetTop(img, POP);
                };

                img.MouseLeftButtonDown += (sender, args) =>
                {
                    // Convert sender to a CardBox
                    img = sender as Image;

                    // If the conversion worked
                    if (img != null)
                    {
                        
                        // if the card is in the home panel...
                        if (img.Parent == canvas)
                        {
                            // Remove the card from the home panel
                            
                            canvas.Children.Remove(img);
                            pnlPlayingField.Children.Add(img);
                        }
                        else
                        {
                            //TODO: Disable images
                            DisableImage(img);

                        }
                    }

                    // Realign the cards 
                    RealignCards(canvas);
                    RealignCards(pnlPlayingField);
                };
            }
            RealignCards(canvas);
        }

        #endregion

        #region HELPER METHODS

        /// <summary>
        /// Repositions the cards in a panel so that they are evenly distributed in the area available.
        /// </summary>
        /// <param name="panelHand"></param>
        private void RealignCards(Canvas canvasHand)
        {
            // Determine the number of cards/controls in the panel.
            int myCount = canvasHand.Children.Count;

            // If there are any cards in the panel
            if (myCount > 0)
            {
                // Determine how wide one card/control is.

                int cardWidth = 50;
                // Determine where the left-hand edge of a card/control placed 
                // in the middle of the panel should be  
                int startPoint = (int)((canvasHand.Width - cardWidth) / 2);

                // An offset for the remaining cards
                int offset = 0;

                // If there are more than one cards/controls in the panel
                if (myCount > 1)
                {
                    // Determine how wide one card/control is.
                    offset = (int)((canvasHand.Width - cardWidth - 2 * POP) / (myCount - 1));

                    // If the offset is bigger than the card/control width, i.e. there is lots of room, 
                    // set the offset to the card width. The cards/controls will not overlap at all.
                    if (offset > cardWidth)
                        offset = cardWidth;


                    // Determine width of all the cards/controls 
                    int allCardsWidth = (myCount - 1) * offset + cardWidth;

                    startPoint = (int)((canvasHand.Width - allCardsWidth) / 2);

                }

                // Aligning the cards: Note that I align them in reserve order from how they
                // are stored in the controls collection. This is so that cards on the left 
                // appear underneath cards to the right. This allows the user to see the rank
                // and suit more easily.
                //// Align the "first" card (which is the last control in the collection)
                Canvas.SetTop(canvasHand.Children[myCount - 1], POP);
                Canvas.SetRight(canvasHand.Children[myCount - 1], startPoint);


                // for each of the remaining controls, in reverse order.
                for (int index = myCount - 2; index >= 0; index--)
                {
                    // Align the current card
                    Canvas.SetTop(canvasHand.Children[index], POP);
                    Canvas.SetRight(canvasHand.Children[index], Canvas.GetRight(canvasHand.Children[index + 1]) + offset);
                }

            }
        }

        /// <summary>
        /// Disables an image in the playing field 
        /// </summary>
        /// <param name="img"></param>
        private void DisableImage(Image img)
        {
            img.Height = regularHeight;
            img.Width = regularWidth;
        }
        #endregion
    }
}

using CardLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CardUI
{
    public class Animate
    {
        #region FIELDS AND PROPERTIES 
        /// <summary>
        /// Card Image object =
        /// </summary>
        public Image cardImage = new Image();

        /// <summary>
        /// The amount, in points, that CardBox controls are enlarged when hovered over. 
        /// </summary>
        private const int POP = 25;

        #endregion

        #region CONSTRUCTORS

        public Animate(PlayingCard card)
        {
            cardImage = card.UpdateCardImage();
        }

        #endregion

        #region METHODS
        /// <summary>
        /// Reference: https://stackoverflow.com/questions/35969705/double-animations-with-image-in-wpf-form
        /// </summary>
        /// <param name="target"></param>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        public static void MoveTo(Image target, double newX, double newY)
        {
           
            //storyBoard.Begin();
            var top = Canvas.GetTop(target);
            var left = Canvas.GetLeft(target);
            top = Double.IsNaN(top) ? 0 : top;
            left = Double.IsNaN(left) ? 0 : left;

            var storyboard = new Storyboard();

            DoubleAnimation doubleAnimation = new DoubleAnimation(top, newY, TimeSpan.FromSeconds(1));
            Storyboard.SetTarget(doubleAnimation, target);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(Canvas.TopProperty));
            storyboard.Children.Add(doubleAnimation);

            DoubleAnimation doubleAnimation2 = new DoubleAnimation(left, newX, TimeSpan.FromSeconds(1));
            Storyboard.SetTarget(doubleAnimation2, target);
            Storyboard.SetTargetProperty(doubleAnimation2, new PropertyPath(Canvas.LeftProperty));
            storyboard.Children.Add(doubleAnimation2);

            storyboard.Begin();
        }
        
        /// <summary>
        /// Deals the deck of cards to players at the start of the game 
        /// </summary>
        /// <param name="deck"></param>
        public static void Deal(Deck<PlayingCard> deck, Canvas canvas)
        {
            
            for (int i = 1; i < deck.Size; i++)
            {

                PlayingCard player_card = deck.GetCard(i);
                player_card.Face = Face.Down;
                Image img1 = player_card.UpdateCardImage();

                //MoveTo Controls 

                //Adds image to the canvas panel
                canvas.Children.Add(img1);

            }
        }

        /// <summary>
        /// Repositions the cards in a panel so that they are evenly distributed in the area available.
        /// </summary>
        /// Reference: Macdonald, T. (2019, February 23). Windows form ui demo part 1 - introduction. 
        /// Retrieved March 23, 2021, from https://www.youtube.com/watch?v=-n21vAPvrtg&amp;list=PLfNfAX7mRzNrF6dkHk31E4xEINXUFe5IM
        /// <param name="panelHand"></param>
        public static void RealignCards(Canvas canvasHand)
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

    }
}
#endregion
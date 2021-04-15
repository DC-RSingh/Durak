/**
 * OOP 4200 - Final Project - Durak
 * 
 * PlayingCard.cs is a class used to represent the cards being played on the game.
 * It makes use of the images saved within the application
 * 
 * @author      Raje Singh, Fleur Blanckaert, Gabriel Dietrich, Dalton Young
 * @version     1.0
 * @since       2021-03 
 */

using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using CardLib;

namespace CardUI
{
    /** IMAGE ATTRIBUTION
*   ==================
*   Original playing card images in the cards folder and everthing location within the pack folder are 
*   created by the American Contract Bridge Leauge
*   52 playing cards. (n.d.). Retrieved April 15, 2021, 
*   from http://acbl.mybigcommerce.com/52-playing-cards/
*/

    /// <summary>
    /// Extends the <see cref="Card"/> class and adds functionality for use in a 2D GUI.
    /// </summary>
    public class PlayingCard : Card
    {
        #region FIELD AND PROPERTIES
       
        // Constant Declaration (path to images and pack)
        private const string CARD_PATH = @"pack://application:,,,/Resources/Cards/";
        private const string PACK_PATH = @"pack://application:,,,/Resources/Pack/";
        
        // TODO: Dynamic Card Back
        private string CURRENT_BACK = "gray_back.png";

        /// <summary>
        /// The height of an image box control 
        /// </summary>
        private const int regularHeight = 107;

        /// <summary>
        /// The width of a image box control 
        /// </summary>
        private const int regularWidth = 75;

        /// <summary>
        /// The image that represents the current <see cref="PlayingCard"/>'s state, depending on it's <see cref="CardBase.Face"/>.
        /// </summary>
        public Image CardImage { get; private set; }

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Initializes an instance of Playing Card with the given <see cref="Suit"/> and <see cref="Rank"/>. The <see cref="Face"/> is set as <see cref="Face.Up"/> by default.
        /// <para>
        /// Updates the <see cref="CardImage"/> with the image representing this card Face up.
        /// </para>
        /// </summary>
        /// 
        /// <param name="suit">The suit of the card.</param>
        /// <param name="rank">The rank of the card.</param>
        public PlayingCard(Suit suit, Rank rank) : base(suit, rank)
        {
            UpdateCardImage();
        }

        /// <summary>
        /// Initializes an instance of Playing Card with the given <see cref="Suit"/>, <see cref="Rank"/> and <see cref="Face"/>.
        /// <para>
        /// Updates the <see cref="CardImage"/> with the image representing this card with the face provided.
        /// </para>
        /// </summary>
        /// <param name="suit">Represents the suit of the card</param>
        /// <param name="rank">Represents the rank of the card</param>
        /// <param name="face">Represents if the card is facing up or down</param>
        public PlayingCard(Suit suit, Rank rank, Face face) : base(suit, rank, face)
        {
            UpdateCardImage();
        }

        #endregion

        #region PRIVATE METHODS

        /// <summary>
        /// Gets the image to be displayed depending on which card
        /// </summary>
        /// <param name="newSuit">Represents the new suit of the card</param>
        /// <param name="newRank">Represents the new rank of the card</param>
        /// <param name="newFace">Represents if the new card is facing up or down</param>
        /// <returns></returns>
        private BitmapImage GetCardImage(Suit newSuit, Rank newRank, Face newFace)
        {

            if (Face == Face.Down)
            {
                return GetImage(PACK_PATH + CURRENT_BACK);
            }
            
            return GetImage($"{CARD_PATH + Rank + Suit}.png");
        }

        /// <summary>
        /// Gets the path for the image used on each card
        /// </summary>
        /// <param name="path">Represents the path where the cards are saved in the application</param>
        /// <returns></returns>
        private BitmapImage GetImage(string path)
        {
            BitmapImage card = new BitmapImage();
            card.BeginInit();
            card.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            card.EndInit();
            return card;
        }

        #endregion

        #region PUBLIC METHODS 

        /// <summary>
        /// Updates the image displayed for the cards
        /// </summary>
        /// <returns>Returns the image</returns>
        public Image UpdateCardImage()
        {
            Image img = new Image { Width = regularWidth, Height = regularHeight, Source = GetCardImage(Suit, Rank, Face) };

            //Sets image quality to high
            RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);

            CardImage = img;

            return img;
        }

        #endregion

    }
}
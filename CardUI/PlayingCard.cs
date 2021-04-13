using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using CardLib;

namespace CardUI
{
    public class PlayingCard : Card
    {
        #region FIELD AND PROPERTIES
       
        //Constant Declaration
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

        #endregion

        #region CONSTRUCTORS

        //private BitmapImage PlayingCardImage { get; set; }

        public Image CardImage { get; private set; }

        public PlayingCard(Suit suit, Rank rank) : base(suit, rank)
        {
            //PlayingCardImage = GetCardImage(suit, rank, Face.Down);
            UpdateCardImage();
        }

        public PlayingCard(Suit suit, Rank rank, Face face) : base(suit, rank, face)
        {
            //PlayingCardImage = GetCardImage(suit, rank, face);
            UpdateCardImage();
        }

        // Class Methods
        public Image UpdateCardImage()
        {
            Image img = new Image { Width = regularWidth, Height = regularHeight, Source = GetCardImage(Suit, Rank, Face) };

            //Sets image quality to high
            RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);

            CardImage = img;

            return img;
        }

        // Private Methods
        private BitmapImage GetCardImage(Suit newSuit, Rank newRank, Face newFace)
        {

            if (Face == Face.Down)
            {
                return GetImage(PACK_PATH + CURRENT_BACK);
            }
            
            return GetImage($"{CARD_PATH + Rank + Suit}.png");
        }

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

        #endregion

        #region EVENTS and EVENT HANDLERS

        /// <summary>
        /// An event handler to load the event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_Load(object sender, EventArgs e)
        {
            UpdateCardImage();
        }

        /// <summary>
        /// An event the client can handle when the users clicks a control 
        /// </summary>
        public event EventHandler Click;

        /// <summary>
        /// An event the client can handle when the users flips a card 
        /// </summary>
        public event EventHandler CardFlipped;

        #endregion

    }
}
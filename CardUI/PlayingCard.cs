using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        ///// <summary>
        ///// Gets and sets the underlying card image object
        ///// </summary>
        //private PlayingCard myCard;
        //public PlayingCard Card
        //{
        //    set
        //    {
        //        myCard = value;
        //        UpdateCardImage();
        //    }
        //    get { return myCard; }
        //}

        ///// <summary>
        ///// Gets and sets the card suit 
        ///// </summary>
        //public Suit Suit
        //{
        //    set
        //    {
        //        Card.Suit = value;
        //        UpdateCardImage();
        //    }
        //    get { return Card.Suit; }
        //}

        ///// <summary>
        ///// Gets and sets the card rank 
        ///// </summary>
        //public Rank Rank
        //{
        //    set
        //    {
        //        Card.Rank = value;
        //        UpdateCardImage();
        //    }
        //    get { return Card.Rank; }
        //}

        ///// <summary>
        ///// Gets and sets the card face
        ///// </summary>
        //public Face Face
        //{
        //    set
        //    {
        //        //if value is different from underlying object's FaceUp property 
        //        if (myCard.Face != Face.Up) //then the card is flipping over 
        //        {
        //            myCard.Face = Face.Down; //Change the card property 
        //            UpdateCardImage(); //Update image front or back 
        //        }
        //        else
        //        {
        //            myCard.Face = Face.Up;
        //            UpdateCardImage(); //Update image front or back 
        //        }
        //    }
        //    get { return Card.Face; }
        //}
        #endregion

        #region CONSTRUCTORS

        public BitmapImage PlayingCardImage { get; set; }

        public PlayingCard(Suit suit, Rank rank) : base(suit, rank)
        {
            PlayingCardImage = GetCardImage(suit, rank, Face.Down);
        }

        public PlayingCard(Suit suit, Rank rank, Face face) : base(suit, rank, face)
        {
            PlayingCardImage = GetCardImage(suit, rank, face);
        }

        // Class Methods
        public BitmapImage UpdateCardImage()
        {
            return PlayingCardImage;
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

        #region EVENTS and EVENT HANDLERS

        /// <summary>
        /// An event handler to load the event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardBox_Load(object sender, EventArgs e)
        {
            UpdateCardImage();
        }

        /// <summary>
        /// An event the client can handle when the users clicks a control 
        /// </summary>
        new public event EventHandler Click;

        /// <summary>
        /// An event the client can handle when the users flips a card 
        /// </summary>
        new public event EventHandler CardFlipped;

        #endregion

    }
}
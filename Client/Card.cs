using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Client
{
    public class Card
    {
        //Constant Declaration
        const string CARD_PATH = @"pack://application:,,,/Resources/Cards/";
        const string PACK_PATH = @"pack://application:,,,/Resources/Pack/";

        public Rank rank;
        public Suit suit;
        public Face face;

        public BitmapImage PlayingCardImage { get; set; }

        /// <summary>
        /// Builds card based on suit, rank, and if it is facing up or down
        /// </summary>
        /// <param name="newSuit">Represents the suit of the card</param>
        /// <param name="newRank">Represents the rank of the card</param>
        /// <param name="newFace">Represents if card is facing up or down</param>
        public Card(Suit newSuit, Rank newRank, Face newFace)
        {
            suit = newSuit;
            rank = newRank;
            face = newFace;
            PlayingCardImage = GetCardImage(newSuit, newRank, newFace);
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Card()
        {
            suit = Suit.Blank;
            rank = Rank.Ace;
            face = Face.Down;
        }

        /// <summary>
        /// Used to show Card fields
        /// </summary>
        public string ToString()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// If card facing up or down
        /// </summary>
        public Face Face;
        /// <summary>
        /// Card's suit
        /// </summary>
        public Suit Suit;
        /// <summary>
        /// Card's Rank
        /// </summary>
        public Rank Rank;

    public BitmapImage GetCardImage(Suit newSuit, Rank newRank, Face newFace)
        {

            if (newFace == Face.Down)
            {
                return PlayingCard.GetImage(PACK_PATH + "gray_back.png");
            }
            else
            {
                return PlayingCard.GetImage(PACK_PATH + "gray_back.png"); 
            }
        }
    }
}

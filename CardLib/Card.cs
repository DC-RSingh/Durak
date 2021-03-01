using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Durak.CardLib
{
    class Card
    {

        /// <summary>
        /// Builds card based on suit, rank, and if it is facing up or down
        /// </summary>
        /// <param name="newSuit">Represents the suit of the card</param>
        /// <param name="newRank">Represents the rank of the card</param>
        /// <param name="newFace">Represents if card is facing up or down</param>
        public Card(Suit newSuit, Rank newRank, Face newFace)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        private Card()
        {
            throw new System.NotImplementedException();
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace CardLib
{
    /// <summary>
    /// A cloneable collection that holds concrete instances of <see cref="CardBase"/>. 
    /// </summary>
    public class Cards : List<CardBase>, ICloneable
    {
        public object Clone()
        {
            var newCards = new Cards();
            foreach (var sourceCard in this)
            {
                newCards.Add((CardBase)sourceCard.Clone());
            }

            return newCards;
        }

        /// <summary>
        /// Utility method for copying card instances into another Cards instances.
        /// This implementation assumes that source and target collections are the same size.
        /// </summary>
        /// <param name="targetCards"></param>
        public void CopyTo(Cards targetCards)
        {
            for (int index = 0; index < this.Count; index++)
            {
                targetCards[index] = this[index];
            }
        }

        /// <summary>
        /// Removes a Card from the list and returns the card removed.
        /// </summary>
        /// <param name="pos">The position of the card in the list.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns>The CardBase object that was removed at the specified position.</returns>
        public CardBase Retrieve(int pos)
        {
            var card = this[pos];
            RemoveAt(pos);
            return card;
        }

        /// <summary>
        /// Gets a card at the specified position.
        /// </summary>
        /// <remarks> Exactly the same as <see cref="List{T}"/> ElementAt. Only a semantic difference.</remarks>
        /// <param name="pos">The position of the card in the list.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>The card at the specified position.</returns>
        public CardBase CardAt(int pos)
        {
            return this.ElementAt(pos);
        }

        public override string ToString()
        {
            var cardList = "";
            for (var index = 0; index < this.Count; index++)
            {
                var cardBase = this[index];
                if (index != this.Count - 1)
                    cardList += $"{index + 1}: {cardBase}, ";
                else
                    cardList += $"{index + 1}: {cardBase}";
            }

            return cardList;
        }
    }
}

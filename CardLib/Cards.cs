﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CardLib
{
    // TODO: Add Comments
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
        /// Utility method for copying card instances into another Cards instance-used in Deck.Shuffle().
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
        /// <returns>The CardBase object that was removed at the specified position.</returns>
        public CardBase Retrieve(int pos)
        {
            var card = this[pos];
            RemoveAt(pos);
            return card;
        }

        /// <summary>
        /// Gets a card at the specified position. Exactly the same as <see cref="List{T}"/> ElementAt.
        /// </summary>
        /// <param name="pos">The position of the card in the list.</param>
        /// <returns>The card at the specified position.</returns>
        public CardBase CardAt(int pos)
        {
            return this.ElementAt(pos);
        }
    }
}

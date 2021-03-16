using System;
using System.Collections;
using System.Collections.Generic;

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
    }
}

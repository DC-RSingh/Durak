/**
 * OOP 4200 - Final Project - Durak
 * 
 * Cards.cs is used to action all the cards that will be availble to be played within the game.
 * 
 * @author      Raje Singh, Fleur Blanckaert, Gabriel Dietrich, Dalton Young
 * @version     1.0
 * @since       2021-02 
 */

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
        /// <summary>
        /// USed to clones the cards
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Overrides the ToString method, used to get the card list
        /// </summary>
        /// <returns>Returns the card list</returns>
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

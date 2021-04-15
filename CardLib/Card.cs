/**
 * OOP 4200 - Final Project - Durak
 * 
 * Card.cs represents the cards of the game. The cards can have different ranks, suits, 
 * and be facing up or down.
 * 
 * @author      Raje Singh, Fleur Blanckaert, Gabriel Dietrich, Dalton Young
 * @version     1.0
 * @since       2021-02 
 */

namespace CardLib
{
    /// <summary>
    /// Represents a standard Playing Card with a Rank, Suit and a Face.
    /// </summary>
    public class Card : CardBase
    {
        /// <summary>
        /// Copies the nonstatic fields to a new object
        /// </summary>
        /// <returns>MemberwiseClone() method</returns>
        public override object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        /// Creates an instance of a Face Up <see cref="Card"/> with its own <seealso cref="Suit"/> and <seealso cref="Rank"/>.
        /// </summary>
        /// <param name="newSuit">The suit of the card.</param>
        /// <param name="newRank">The rank of the card.</param>
        public Card(Suit newSuit, Rank newRank) : base(newSuit, newRank)
        {
        }

        /// <summary>
        /// Creates an instance of a <see cref="Card"/> with its own <seealso cref="Suit"/>, <seealso cref="Rank"/> and
        /// <seealso cref="Face"/> status.
        /// </summary>
        /// <param name="newSuit">The suit of the card.</param>
        /// <param name="newRank">The rank of the card.</param>
        /// <param name="newFace">The Face status of the card.</param>
        public Card(Suit newSuit, Rank newRank, Face newFace) : base(newSuit, newRank, newFace)
        {
        }

    }
}
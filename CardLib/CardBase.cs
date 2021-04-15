using System;
using System.Diagnostics;

namespace CardLib
{
    /// <summary>
    /// Represents a Playing Card with it's own <see cref="CardLib.Suit"/>, <seealso cref="CardLib.Rank"/> and
    /// <seealso cref="CardLib.Face"/>. This class is abstract.
    /// </summary>
    public abstract class CardBase : ICloneable
    {
        /// <summary>
        /// Event that is invoked when the Face of the card is changed.
        /// </summary>
        public event EventHandler CardFlipped;

        #region Public Methods

        public abstract object Clone();

        public override bool Equals(object card)
        {
            return this == (CardBase)card;
        }

        public override int GetHashCode()
        {
            return 13 * (int)Suit + (int)Rank;
        }

        public override string ToString()
        {
            return Convert.ToBoolean(Face) ? $"{Rank} of {Suit}s" : "Face Down";
        }

        /// <summary>
        /// Flips the card's current <see cref="Face"/>.
        /// </summary>
        public void Flip()
        {
            Face = Face == Face.Up ? Face.Down : Face.Up;
        }

        #endregion

        #region Static Members

        /// <summary>
        /// Signifies whether instances of CardBase should use a Trump Suit.
        /// </summary>
        public static bool UseTrumps = false;

        /// <summary>
        /// The Trump Suit to use when <see cref="UseTrumps"/> is set to true.
        /// Can be set even when <see cref="UseTrumps"/> is false, but will be ignored.
        /// </summary>
        public static Suit Trump = Suit.Club;

        /// <summary>
        /// Signifies whether Aces should have their high value used or not.
        /// </summary>
        public static bool IsAceHigh = true;

        #endregion

        #region Properties
        /// <summary>
        /// The <see cref="CardLib.Suit"/> of the Card.
        /// </summary>
        public readonly Suit Suit;

        /// <summary>
        /// The <see cref="CardLib.Rank"/> of the Card.
        /// </summary>
        public readonly Rank Rank;

        // Private Face variable
        private Face _face;

        /// <summary>
        /// The <see cref="CardLib.Face"/> of the Card.
        /// </summary>
        public Face Face
        {
            get => _face;
            set
            {
                if (_face == value) return;
                _face = value;
                CardFlipped?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a Face Up card with a Suit and Rank.
        /// </summary>
        /// <param name="newSuit">The suit of the card.</param>
        /// <param name="newRank">The rank of the card.</param>
        protected CardBase(Suit newSuit, Rank newRank)
        {
            Suit = newSuit;
            Rank = newRank;
            Face = Face.Up;
        }

        /// <summary>
        /// Creates a card with the specified Suit, Rank and Face.
        /// </summary>
        /// <param name="newSuit">The suit of the card.</param>
        /// <param name="newRank">The rank of the card.</param>
        /// <param name="newFace">The face of the card.</param>
        protected CardBase(Suit newSuit, Rank newRank, Face newFace) : this(newSuit, newRank)
        {
            Face = newFace;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Checks if two instances of <see cref="CardBase"/> are equal.
        /// This is irrespective of the Card's <seealso cref="Face"/>.
        /// </summary>
        /// <param name="card1">The first card.</param>
        /// <param name="card2">The second card.</param>
        /// <returns>true if both the Suit and Rank of the Card are equal, false otherwise.</returns>
        public static bool operator ==(CardBase card1, CardBase card2)
        {
            Debug.Assert(!(card1 is null), nameof(card1) + " != null");
            Debug.Assert(!(card2 is null), nameof(card2) + " != null");
            return card1.Suit == card2.Suit && (card1.Rank == card2.Rank);
        }

        /// <summary>
        /// Checks if two instances of <see cref="CardBase"/> are not equal. Uses <seealso cref="CardBase.operator=="/>.
        /// </summary>
        /// <param name="card1">The first card.</param>
        /// <param name="card2">The second card.</param>
        /// <returns>true if <paramref name="card1"/> and <paramref name="card2"/> are equal, false otherwise.</returns>
        public static bool operator !=(CardBase card1, CardBase card2)
        {
            return !(card1 == card2);
        }

        /// <summary>
        /// Checks whether <paramref name="card1"/> is greater in value than <paramref name="card2"/>.
        /// </summary>
        /// <param name="card1"></param>
        /// <param name="card2"></param>
        /// <returns>true if <paramref name="card1"/> is of the same suit and higher rank or if <see cref="UseTrumps"/>
        /// is enabled and <paramref name="card1"/> is of the <see cref="Trump"/> Suit and <paramref name="card2"/> is not.
        /// Always returns false if <paramref name="card1"/> and <paramref name="card2"/> are of different Suits.
        /// </returns>
        public static bool operator >(CardBase card1, CardBase card2)
        {
            if (card1.Suit == card2.Suit)
            {
                if (IsAceHigh)
                {
                    if (card1.Rank == Rank.Ace)
                    {
                        return card2.Rank != Rank.Ace;
                    }

                    if (card2.Rank == Rank.Ace)
                        return false;
                    return (card1.Rank > card2.Rank);
                }

                return (card1.Rank > card2.Rank);
            }

            return UseTrumps && (card1.Suit == Trump);
        }

        /// <summary>
        /// Checks whether <paramref name="card1"/> is less in value than <paramref name="card2"/>.
        /// </summary>
        /// <param name="card1">The first card.</param>
        /// <param name="card2">The second card.</param>
        /// <returns>true if <paramref name="card1"/> is less than <paramref name="card2"/>, false otherwise.</returns>
        public static bool operator <(CardBase card1, CardBase card2)
        {
            return !(card1 >= card2);
        }

        /// <summary>
        /// Checks if <paramref name="card1"/> is greater than or equal in value of <paramref name="card2"/>.
        /// </summary>
        /// <param name="card1">The first card.</param>
        /// <param name="card2">The second card.</param>
        /// <returns>true if <paramref name="card1"/> is greater than or equal to <paramref name="card2"/>, false otherwise.</returns>
        public static bool operator >=(CardBase card1, CardBase card2)
        {
            if (card1.Suit == card2.Suit)
            {
                if (IsAceHigh)
                {
                    if (card1.Rank == Rank.Ace)
                    {
                        return true;
                    }

                    if (card2.Rank == Rank.Ace)
                        return false;
                    return (card1.Rank >= card2.Rank);
                }

                return (card1.Rank >= card2.Rank);
            }

            return UseTrumps && (card1.Suit == Trump);
        }

        /// <summary>
        /// Checks if <paramref name="card1"/> is less than or equal in value of <paramref name="card2"/>.
        /// </summary>
        /// <param name="card1">The first card.</param>
        /// <param name="card2">The second card.</param>
        /// <returns>true if <paramref name="card1"/> is less than or equal to <paramref name="card2"/>, false otherwise.</returns>
        public static bool operator <=(CardBase card1, CardBase card2)
        {
            return !(card1 > card2);
        }

        #endregion
    }
}

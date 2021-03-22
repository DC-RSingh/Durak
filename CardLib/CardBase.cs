using System;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;

namespace CardLib
{
    public abstract class CardBase : ICloneable
    {
        public event EventHandler CardFlipped;

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

        #region Static Members

        public static bool UseTrumps = false;
        public static Suit Trump = Suit.Club;
        public static bool IsAceHigh = true;

        #endregion

        #region Properties

        public readonly Suit Suit;
        public readonly Rank Rank;

        private Face _face;
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

        protected CardBase(Suit newSuit, Rank newRank)
        {
            Suit = newSuit;
            Rank = newRank;
            Face = Face.Up;
        }

        protected CardBase(Suit newSuit, Rank newRank, Face newFace) : this(newSuit, newRank)
        {
            Face = newFace;
        }

        #endregion

        #region Operators

        public static bool operator ==(CardBase card1, CardBase card2)
        {
            Debug.Assert(card1 != null, nameof(card1) + " != null");
            Debug.Assert(card2 != null, nameof(card2) + " != null");
            return card1.Suit == card2.Suit && (card1.Rank == card2.Rank);
        }

        
        public static bool operator !=(CardBase card1, CardBase card2)
        {
            return !(card1 == card2);
        }

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

            if (UseTrumps && (card2.Suit == Trump))
                return false;
            return true;

        }

        public static bool operator <(CardBase card1, CardBase card2)
        {
            return !(card1 >= card2);
        }

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

            if (UseTrumps && (card2.Suit == Trump))
                return false;
            return true;
        }

        public static bool operator <=(CardBase card1, CardBase card2)
        {
            return !(card1 > card2);
        }
#endregion
    }
}

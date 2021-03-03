using System;

namespace CardLib
{
    public class Card : ICloneable
    {
        public static bool useTrumps = false;

        public static Suit trump = Suit.Club;

        public static bool isAceHigh = true;

        public object Clone()
        {
            return MemberwiseClone();
        }

        public readonly Suit Suit;
        public readonly Rank Rank;
        public Face Face;

        public Card(Suit newSuit, Rank newRank)
        {
            Suit = newSuit;
            Rank = newRank;
            Face = Face.Up;
        }

        public Card(Suit newSuit, Rank newRank, Face newFace)
        {
            // TODO: Repeating Code, make code DRY
            Suit = newSuit;
            Rank = newRank;
            Face = newFace;
        }

        public override string ToString()
        {
            return $"Face {Face} {Rank} of {Suit}s.";
        }

        // Operator Overloads
        public static bool operator ==(Card card1, Card card2)
        {
            return card1.Suit == card2.Suit && (card1.Rank == card2.Rank);
        }

        public static bool operator !=(Card card1, Card card2)
        {
            return !(card1 == card2);
        }

        public override bool Equals(object card)
        {
            return this == (Card) card;
        }

        public override int GetHashCode()
        {
            return 13 * (int) Suit + (int) Rank;
        }

        public static bool operator >(Card card1, Card card2)
        {
            if (card1.Suit == card2.Suit)
            {
                if (isAceHigh)
                {
                    if (card1.Rank == Rank.Ace)
                    {
                        return card2.Rank != Rank.Ace;
                    }

                    if (card2.Rank == Rank.Ace)
                        return false;
                    return(card1.Rank > card2.Rank);
                }

                return (card1.Rank > card2.Rank);
            }

            if (useTrumps && (card2.Suit == trump))
                return false;
            return true;

        }

        public static bool operator <(Card card1, Card card2)
        {
            return !(card1 >= card2);
        }

        public static bool operator >=(Card card1, Card card2)
        {

            if (card1.Suit == card2.Suit)
            {
                if (isAceHigh)
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

            if (useTrumps && (card2.Suit == trump))
                return false;
            return true;

        }

        public static bool operator <=(Card card1, Card card2)
        {
            return !(card1 > card2);
        }
    }
}

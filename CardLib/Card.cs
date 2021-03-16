using System;

namespace CardLib
{
    public class Card : CardBase
    {
        public static bool useTrumps = false;

        {
            return MemberwiseClone();
        }

        {
            return !(card1 >= card2);
        }

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

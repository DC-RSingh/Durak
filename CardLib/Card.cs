namespace CardLib
{
    public class Card : CardBase
    {

        public override object Clone()
        {
            return MemberwiseClone();
        }

        public Card(Suit newSuit, Rank newRank) : base(newSuit, newRank)
        {
        }

        public Card(Suit newSuit, Rank newRank, Face newFace) : base(newSuit, newRank, newFace)
        {
        }

    }
}

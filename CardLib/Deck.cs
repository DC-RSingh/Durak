using System;

namespace CardLib
{
    // TODO: Add Comments
    // TODO: Configure for PlayingCards
    public class Deck : ICloneable
    {
        public object Clone()
        {
            var newDeck = new Deck(cards.Clone() as Cards);
            return newDeck;
        }

        private Deck(Cards newCards)
        {
            cards = newCards;
        }

        private Cards cards = new Cards();

        public Deck(bool useTrumps, Suit trump) : this()
        {
            Card.useTrumps = useTrumps;
            Card.trump = trump;
        }

        public Deck(bool isAceHigh) : this()
        {
            Card.isAceHigh = isAceHigh;
        }

        public Deck(bool isAceHigh, bool useTrumps, Suit trump) : this()
        {
            Card.isAceHigh = isAceHigh;
            Card.useTrumps = useTrumps;
            Card.trump = trump;
        }

        public Deck()
        {
            for (var suitVal = 0; suitVal < 4; suitVal++)
            {
                for (var rankVal = 1; rankVal < 14; rankVal++)
                {
                    cards.Add(new Card((Suit)suitVal, (Rank)rankVal));
                }
            }
        }

        public Card GetCard(int cardNum)
        {
            if (cardNum >= 0 && cardNum <= 51)
                return cards[cardNum];
   
            throw new ArgumentOutOfRangeException(nameof(cardNum), cardNum, "Value must be between 0 and 51.");
        }

        public void Shuffle()
        {
            var newDeck = new Cards();

            var assigned = new bool[52];
            var sourceGen = new Random();

            for (var i = 0; i < 52; i++)
            {

                var destCard = 0;
                var foundCard = false;

                while (foundCard == false)
                {
                    destCard = sourceGen.Next(52);
                    if (assigned[destCard] == false)
                        foundCard = true;
                }

                assigned[destCard] = true;
                newDeck.Add(cards[destCard]);
            }
            newDeck.CopyTo(cards);
        }

    }
}

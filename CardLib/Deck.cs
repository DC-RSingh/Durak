using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace CardLib
{
    // TODO: Add Comments
    // TODO: Configure for different deck sizes 20, 36, 52
    public class Deck<T> : ICloneable where T : CardBase
    {
        // TODO: 20 = 10 to Ace All Suits, 36 = 6 to Ace All Suits
        private readonly Cards _cards = new Cards();

        public int Size => _cards.Count;

        public bool CanDraw => _cards.Count > 0;

        public object Clone()
        {
            var newDeck = new Deck<T>(_cards.Clone() as Cards);
            return newDeck;
        }

        private Deck(Cards newCards)
        {
            _cards = newCards;
        }

        public Deck(bool useTrumps, Suit trump) : this()
        {
            CardBase.UseTrumps = useTrumps;
            CardBase.Trump = trump;
        }

        public Deck(bool isAceHigh) : this()
        {
            CardBase.IsAceHigh = isAceHigh;
        }

        public Deck(bool isAceHigh, bool useTrumps, Suit trump) : this()
        {
            CardBase.IsAceHigh = isAceHigh;
            CardBase.UseTrumps = useTrumps;
            CardBase.Trump = trump;
        }

        public Deck()
        {
            for (var suitVal = 0; suitVal < 4; suitVal++)
            {
                for (var rankVal = 1; rankVal < 14; rankVal++)
                {
                    _cards.Add((T)Activator.CreateInstance(typeof(T), (Suit)suitVal, (Rank)rankVal));
                }
            }
        }

        public T GetCard(int cardNum)
        {
            if (cardNum >= 0 && cardNum <= 51)
                return (T)_cards[cardNum];
   
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
                newDeck.Add(_cards[destCard]);
            }
            newDeck.CopyTo(_cards);
        }

        public T Draw()
        {
            var drawn = _cards.Last();

            _cards.RemoveAt(_cards.Count - 1);

            return (T)drawn;
        }

        public Cards Draw(int amount)
        {
            var cards = new Cards();

            for (var i = 0; i < amount; i++)
            {
                cards.Add(Draw());
            }

            return cards;
        }

        public T DrawAt(int pos)
        {
            var card = GetCard(pos);
            _cards.RemoveAt(pos);
            return card;
        }

    }
}

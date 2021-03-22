using System;
using System.Linq;

namespace CardLib
{
    // TODO: Add Comments
    /// <summary>
    /// Represents a Deck of <see cref="Card"/> that emulates Playing Card deck functionality.
    /// </summary>
    /// <typeparam name="T">A type that is convertible to the class <see cref="Card"/></typeparam>
    public class Deck<T> : ICloneable where T : Card
    {
        /// <summary>
        /// Event that is invoked when the last card in the deck is drawn.
        /// Replaces <see cref="CardDrawn"/> event in that case.
        /// </summary>
        public event EventHandler LastCardDrawn;

        /// <summary>
        /// Event that is invoked whenever the deck is shuffled.
        /// </summary>
        public event EventHandler DeckShuffled;


        public event EventHandler CardDrawn;

        private readonly Cards _cards = new Cards();

        public int Size => _cards.Count;

        public DeckSize DeckType { get; private set; }

        public int OriginalDeckSize => (int)DeckType * 4;

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

        public Deck(bool useTrumps, Suit trump = Suit.Blank, DeckSize size = DeckSize.FiftyTwo) : this(size)
        {
            CardBase.UseTrumps = useTrumps;
            CardBase.Trump = trump;
        }

        public Deck(bool isAceHigh, DeckSize size = DeckSize.FiftyTwo) : this(size)
        {
            CardBase.IsAceHigh = isAceHigh;
        }

        public Deck(bool isAceHigh, bool useTrumps, Suit trump = Suit.Blank, DeckSize size = DeckSize.FiftyTwo) : this(size)
        {
            CardBase.IsAceHigh = isAceHigh;
            CardBase.UseTrumps = useTrumps;
            CardBase.Trump = trump;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        public Deck(DeckSize size = DeckSize.FiftyTwo)
        {
            DeckType = size;
            
            for (var suitVal = 0; suitVal < 4; suitVal++)
            {
                // Add the ACE for that suit
                _cards.Add((T)Activator.CreateInstance(typeof(T), (Suit)suitVal, (Rank)1));

                for (var rankVal = 13; rankVal > 13 - (int)size + 1; rankVal--)
                {
                    _cards.Add((T)Activator.CreateInstance(typeof(T), (Suit)suitVal, (Rank)rankVal));
                }
            }
        }

        public T GetCard(int cardNum)
        {
            if (cardNum >= 0 && cardNum <= Size)
                return (T)_cards[cardNum];
   
            throw new ArgumentOutOfRangeException(nameof(cardNum), cardNum, $"Value must be between 0 and {Size}.");
        }

        public void Shuffle()
        {
            var newDeck = new Cards();

            var assigned = new bool[Size];
            var sourceGen = new Random();

            for (var i = 0; i < Size; i++)
            {

                var destCard = 0;
                var foundCard = false;

                while (foundCard == false)
                {
                    destCard = sourceGen.Next(Size);
                    if (assigned[destCard] == false)
                        foundCard = true;
                }

                assigned[destCard] = true;
                newDeck.Add(_cards[destCard]);
            }
            newDeck.CopyTo(_cards);
            DeckShuffled?.Invoke(this, EventArgs.Empty);
        }

        // TODO: Make a note that it throws an exception if the cards are out of range
        public T Draw()
        {
            var drawn = _cards.Last();

            _cards.RemoveAt(Size - 1);

            InvokeDrawEvents(EventArgs.Empty);
            return (T)drawn;
        }

        public Cards Draw(int amount)
        {
            var cards = new Cards();

            if (amount > Size) throw new ArgumentOutOfRangeException(nameof(amount), amount, 
                $"Cannot draw {amount}! Deck only has {Size} cards!");

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
            InvokeDrawEvents(EventArgs.Empty);
            return card;
        }

        private void InvokeDrawEvents(EventArgs e)
        {
            if ( _cards.Count == 1)
                LastCardDrawn?.Invoke(this, e);
            else
                CardDrawn?.Invoke(this, e);
        }

    }
}

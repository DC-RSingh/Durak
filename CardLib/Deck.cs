using System;
using System.Linq;

namespace CardLib
{
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

        /// <summary>
        /// Event that is invoked when a card is drawn from the deck.
        /// <para>
        /// Never invoked if the last card in the deck is drawn, <see cref="LastCardDrawn"/> is invoked instead.
        /// </para>
        /// </summary>
        public event EventHandler CardDrawn;

        // Readonly collection of Cards
        private readonly Cards _cards = new Cards();

        /// <summary>
        /// The current size of the deck.
        /// </summary>
        public int Size => _cards.Count;

        /// <summary>
        /// The type/<see cref="DeckSize"/> of the deck.
        /// </summary>
        public DeckSize DeckType { get; private set; }

        /// <summary>
        /// The size the deck was before any cards were drawn.
        /// </summary>
        public int OriginalDeckSize => (int)DeckType * 4;

        /// <summary>
        /// Whether or not it would be a valid operation to draw a card.
        /// </summary>
        public bool CanDraw => _cards.Count > 0;

        public object Clone()
        {
            var newDeck = new Deck<T>(_cards.Clone() as Cards);
            return newDeck;
        }

        // Private constructor used to initialize a deck from a Cards collection.
        private Deck(Cards newCards)
        {
            _cards = newCards;
        }

        /// <summary>
        /// Instantiates an instance of a deck of the specified size and sets the <see cref="CardBase.UseTrumps"/> property.
        /// and <see cref="CardBase.Trump"/>.
        /// </summary>
        /// <param name="useTrumps">Whether to use trumps or not.</param>
        /// <param name="trump">The trump suit to use, set to <see cref="Suit.Blank"/> by default.</param>
        /// <param name="size">The size of the deck, set to <see cref="DeckSize.FiftyTwo"/> by default.</param>
        public Deck(bool useTrumps, Suit trump = Suit.Blank, DeckSize size = DeckSize.FiftyTwo) : this(size)
        {
            CardBase.UseTrumps = useTrumps;
            CardBase.Trump = trump;
        }

        /// <summary>
        /// Instantiates an instance of a deck of the specified size and sets the <see cref="CardBase.IsAceHigh"/> property.
        /// </summary>
        /// <param name="isAceHigh">Whether the value of an ace is high or not.</param>
        /// <param name="size">The size of the deck, set to <see cref="DeckSize.FiftyTwo"/> by default.</param>
        public Deck(bool isAceHigh, DeckSize size = DeckSize.FiftyTwo) : this(size)
        {
            CardBase.IsAceHigh = isAceHigh;
        }

        /// <summary>
        /// Instantiates an instance of a deck with all the static properties of <see cref="CardBase"/> set.
        /// </summary>
        /// <param name="isAceHigh">Whether the value of an ace is high or not.</param>
        /// <param name="useTrumps">Whether to use trumps or not.</param>
        /// <param name="trump">The trump suit to use, set to <see cref="Suit.Blank"/> by default.</param>
        /// <param name="size">The size of the deck, set to <see cref="DeckSize.FiftyTwo"/> by default.</param>
        public Deck(bool isAceHigh, bool useTrumps, Suit trump = Suit.Blank, DeckSize size = DeckSize.FiftyTwo) : this(size)
        {
            CardBase.IsAceHigh = isAceHigh;
            CardBase.UseTrumps = useTrumps;
            CardBase.Trump = trump;
        }

        /// <summary>
        /// Instantiates an instance of a deck of <see cref="DeckSize.FiftyTwo"/>, or the specified size if any.
        /// </summary>
        /// <param name="size">The size of the deck.</param>
        public Deck(DeckSize size = DeckSize.FiftyTwo)
        {
            DeckType = size;
            
            for (var suitVal = 0; suitVal < 4; suitVal++)
            {
                // Add the ACE for that suit
                // Activator.CreateInstance attempts to invoke a constructor of the specified type with the specified params.
                _cards.Add((T)Activator.CreateInstance(typeof(T), (Suit)suitVal, (Rank)1));

                // Adds the remaining ranks for each suit, starting from the highest suit.
                for (var rankVal = 13; rankVal > 13 - (int)size + 1; rankVal--)
                {
                    _cards.Add((T)Activator.CreateInstance(typeof(T), (Suit)suitVal, (Rank)rankVal));
                }
            }
        }

        /// <summary>
        /// Gets the card at the specified position. Indexes start at 0.
        /// </summary>
        /// <param name="cardNum">The position of the card.</param>
        /// <returns>The card at the specified position.</returns>
        public T GetCard(int cardNum)
        {
            if (cardNum >= 0 && cardNum <= Size)
                return (T)_cards[cardNum];
   
            throw new ArgumentOutOfRangeException(nameof(cardNum), cardNum, $"Value must be between 0 and {Size}.");
        }

        /// <summary>
        /// Shuffles the cards in the deck, placing them in a random order.
        /// <para>Invokes the <see cref="DeckShuffled"/> event.</para>
        /// </summary>
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

        /// <summary>
        /// Draws the card at the top of the deck (the last element in the sequence). 
        /// </summary>
        /// <remarks>Removes the card in the process.</remarks>
        /// <returns>The card at the top of the deck. Returns null if there are no cards in the deck.</returns>
        /// /// <exception cref="InvalidOperationException"></exception>
        public T Draw()
        {
            if (!CanDraw) throw new InvalidOperationException("The deck is empty, no more cards can be drawn!");

            var drawn = _cards.Last();

            _cards.RemoveAt(Size - 1);

            InvokeDrawEvents(EventArgs.Empty);
            return (T)drawn;
        }

        /// <summary>
        /// Draws the specified amount of cards from the top of the deck
        /// </summary>
        /// <remarks>Removes the cards in the process.</remarks>
        /// <param name="amount">The amount of cards to draw from the top of the deck.s</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns>A <see cref="Cards"/> collection object with the cards drawn.</returns>
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

        /// <summary>
        /// Draws the card at the specified position in the deck.
        /// </summary>
        /// <remarks>Removes the card from the deck.</remarks>
        /// <param name="pos">The position of the card to draw.</param>
        /// <returns>The card at the specified position.</returns>
        public T DrawAt(int pos)
        {
            var card = GetCard(pos);
            _cards.RemoveAt(pos);
            InvokeDrawEvents(EventArgs.Empty);
            return card;
        }

        // Invokes either LastCardDrawn or CardDrawn depending on the state of the Deck object.
        private void InvokeDrawEvents(EventArgs e)
        {
            if ( _cards.Count == 0)
                LastCardDrawn?.Invoke(this, e);
            else
                CardDrawn?.Invoke(this, e);
        }

    }
}

using System;
using System.Linq;
using CardLib;

// TODO: This should probably be moved to a different class library
namespace DurakLib
{
    public class Player
    {
        public event EventHandler TurnBegin;
        public event EventHandler TurnEnd;

        #region Properties

        public bool IsAttacking { get; set; }

        public bool CanPlay => PlayableCards.Any();

        /// <summary>
        /// Represents the player's Hand
        /// </summary>
        public Cards Hand { get; set; }

        /// <summary>
        /// Represents the set of cards the player has in hands
        /// </summary>
        public int HandSize => Hand.Count;

        /// <summary>
        /// Indicates player's name
        /// </summary>
        public string PlayerName { get; private set; }

        private Cards _playableCards = new Cards();

        public Cards PlayableCards
        {
            get
            {
                //if (!(_playableCards is null)) return _playableCards;
                //_playableCards = new Cards();
                return _playableCards;
                //var isSubset = !_playableCards.Except(Hand).Any();
                //if (!isSubset)
                //{
                //    _playableCards = null;
                //}

            }

            protected set => _playableCards = value;
        }

        public CardBase ChosenCard { get; protected set; }

        public bool HasChosen { get; protected set; }

        #endregion

        #region Methods
        
        protected bool IsPlayable(Cards river, CardBase card)
        {
            // An attacker can only play cards whose rank matches a card in the river
            // A defender can only play cards that are greater than the last card played in the river, or a trump card.
            // Trump logic is handled in CardBase class
            if (river.Count > 0)
                return IsAttacking
                    ? river.Exists(ele => ele.Rank == card.Rank)
                    : card > river.Last();

            // If the River is Empty, any card is playable
            return true;
        }

        public virtual void TakeTurn(Cards river)
        {
            ResetChosen();
            DeterminePlayable(river);
            TurnBegin?.Invoke(this, EventArgs.Empty);
            TurnEnd?.Invoke(this, EventArgs.Empty);
        }

        public void ChooseCard(int choice)
        {
            HasChosen = true;
            ChosenCard = Hand.Retrieve(Hand.IndexOf(PlayableCards.CardAt(choice)));
        }

        public void ChooseCard(CardBase card)
        {
            ChooseCard(PlayableCards.IndexOf(card));
        }

        protected void DeterminePlayable(Cards river)
        {
            _playableCards.Clear();
            _playableCards.AddRange(Hand.FindAll(ele => IsPlayable(river, ele)));
        }

        protected void ResetChosen()
        {
            ChosenCard = null;
            HasChosen = false;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Method referring to player
        /// </summary>
        /// <param name="playerName">Refers to player name</param>
        /// <param name="hand">Refers to player's hand</param>
        public Player(string playerName, Cards hand)
        {
            PlayerName = playerName;
            Hand = hand;
        }

        #endregion

    }
}

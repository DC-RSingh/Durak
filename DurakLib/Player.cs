/**
 * OOP 4200 - Final Project - Durak
 * 
 * Player.cs conatins the logic to pertaining to the moves by the human player
 * 
 * @author      Raje Singh, Fleur Blanckaert, Gabriel Dietrich, Dalton Young
 * @version     1.0
 * @since       2021-03
 */

using System;
using System.Linq;
using CardLib;

namespace DurakLib
{
    /// <summary>
    /// Represents a player of the Durak Card Game.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// An event that is invoked at the start of <see cref="TakeTurn"/>, after <see cref="PlayableCards"/> has been determined.
        /// </summary>
        /// <remarks>
        /// This event is essential to proper <see cref="Player"/> functionality in
        /// Client Code, as without it or the subsequent event <see cref="TurnEnd"/> nothing but <see cref="PlayableCards"/> determination will happen.
        /// </remarks>
        public event EventHandler TurnBegin;

        /// <summary>
        /// An event that is invoked right after <see cref="TurnBegin"/> in <see cref="TakeTurn"/>. 
        /// </summary>
        /// <remarks>
        /// It is important to <b>note well</b> that this event is invoked <b>AFTER</b> <see cref="TurnBegin"/>. Be aware that changes to the object state
        /// may have occurred during that event.
        /// </remarks>
        public event EventHandler TurnEnd;

        /// <summary>
        /// Invoked when a player chooses a card using <see cref="ChooseCard(int)"/> pr <see cref="ChooseCard(CardBase)"/>.
        /// </summary>
        public event EventHandler ChoseCard;

        #region Properties

        /// <summary>
        /// Whether the <see cref="Player"/> is attacking or not.
        /// </summary>
        public bool IsAttacking { get; set; }

        /// <summary>
        /// Whether the <see cref="Player"/> is able to play or not. 
        /// </summary>
        /// <remarks>This value is only valid after <see cref="TakeTurn"/> is called, so be aware of stale values.</remarks>
        public bool CanPlay => PlayableCards.Any();

        /// <summary>
        /// Represents the <see cref="Player"/>'s Hand
        /// </summary>
        public Cards Hand { get; set; }

        /// <summary>
        /// Represents the number of cards left in the <see cref="Player"/>'s <see cref="Hand"/>.
        /// </summary>
        public int HandSize => Hand.Count;

        /// <summary>
        /// Indicates <see cref="Player"/>'s name
        /// </summary>
        public string PlayerName { get; private set; }

        /// <summary>
        /// Contains the cards the <see cref="Player"/> can play.
        /// </summary>
        private Cards _playableCards = new Cards();

        /// <summary>
        /// Represents the cards in the <see cref="Player"/>'s <see cref="Hand"/> that is playable.
        /// </summary>
        public Cards PlayableCards
        {
            get => _playableCards;
            //if (!(_playableCards is null)) return _playableCards;
            //_playableCards = new Cards();

            //var isSubset = !_playableCards.Except(Hand).Any();
            //if (!isSubset)
            //{
            //    _playableCards = null;
            //}
            protected set => _playableCards = value;
        }

        /// <summary>
        /// Represents the card that was chosen after the player takes his turn.
        /// </summary>
        /// <remarks>
        /// ChosenCard is only valid after <see cref="ChooseCard(int)"/> and its overloads are called.
        /// <para>Value is reset to null when a <see cref="TakeTurn"/> is invoked.</para>
        /// </remarks>
        public CardBase ChosenCard { get; protected set; }

        /// <summary>
        /// Represents whether a card is chosen.
        /// </summary>
        /// <remarks>
        /// It would be good practice to check this before trying to access ChosenCard.
        /// <para>This value is reset to false when <see cref="TakeTurn"/> is invoked.</para>
        /// </remarks>
        public bool HasChosen { get; protected set; }

        #endregion

        #region Methods
        
        /// <summary>
        /// Determines whether a card is playable, based on the <see cref="IsAttacking"/> value of this instance.
        /// </summary>
        /// <param name="river">The cards in play in the game.</param>
        /// <param name="card">The card to check.</param>
        /// <returns>true if the card is playable with the current state of <see cref="IsAttacking"/>, false otherwise.</returns>
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

        /// <summary>
        /// Resets the <see cref="ChosenCard"/> and <see cref="HasChosen"/> properties, determines the <see cref="PlayableCards"/> and invokes
        /// the <see cref="TurnBegin"/> and <see cref="TurnEnd"/> events respectively.
        /// </summary>
        /// <remarks>
        /// This method should be called when a <see cref="Player"/> should take their turn, whether they are attacking or defending.
        /// <para>he handlers subscribed to the
        /// <see cref="TurnBegin"/> and <see cref="TurnEnd"/> should at least handle any logic related to Game Play.</para>
        /// </remarks>
        /// <param name="river"></param>
        public virtual void TakeTurn(Cards river)
        {
            ResetChosen();
            DeterminePlayable(river);
            TurnBegin?.Invoke(this, EventArgs.Empty);
            TurnEnd?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Chooses the card that the <see cref="Player"/> wishes to play from their <see cref="PlayableCards"/>.
        /// </summary>
        /// <param name="choice">The index of the card in <see cref="PlayableCards"/> to choose.</param>
        public void ChooseCard(int choice)
        {
            HasChosen = true;
            ChosenCard = Hand.Retrieve(Hand.IndexOf(PlayableCards.CardAt(choice)));
            ChoseCard?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Chooses the card that the <see cref="Player"/> wishes to play from their <see cref="PlayableCards"/>
        /// </summary>
        /// <param name="card">The card in <see cref="PlayableCards"/> to choose.</param>
        public void ChooseCard(CardBase card)
        {
            ChooseCard(PlayableCards.IndexOf(card));
        }

        /// <summary>
        /// Determines the <see cref="Cards"/> that the <see cref="Player"/> can play.
        /// </summary>
        /// <param name="river">The cards in play.</param>
        protected void DeterminePlayable(Cards river)
        {
            _playableCards.Clear();
            _playableCards.AddRange(Hand.FindAll(ele => IsPlayable(river, ele)));
        }

        /// <summary>
        /// Resets the <see cref="Player"/>'s <see cref="ChosenCard"/> and <see cref="HasChosen"/> properties.
        /// </summary>
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

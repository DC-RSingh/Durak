/**
 * OOP 4200 - Final Project - Durak
 * 
 * DurakAI.cs contains the logic behind how the computer decides which card to throw when 
 * it is their turn to play
 * 
 * @author      Raje Singh, Fleur Blanckaert, Gabriel Dietrich, Dalton Young
 * @version     1.0
 * @since       2021-03 
 */

using System;
using System.Collections.Generic;
using System.Linq;
//using System.Threading;
using System.Threading.Tasks;
using CardLib;

namespace DurakLib
{
    /// <summary>
    /// A simple AI that is able to play Durak.
    /// </summary>
    public class DurakAI : Player
    {
        /// <summary>
        /// An event that is invoked before the <see cref="DurakAI"/> has decided on a move.
        /// </summary>
        public event EventHandler StartedThinking;

        /// <summary>
        /// An event that is invoked after the <see cref="DurakAI"/> has decided on a move.
        /// </summary>
        public event EventHandler StoppedThinking;

        /// <summary>
        /// The delay that the <see cref="DurakAI"/> should wait before selecting it's move in milliseconds.
        /// </summary>
        public int ThinkDelay { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="DurakAI"/> with a name and a hand.
        /// </summary>
        /// <param name="name">The name to give the AI.</param>
        /// <param name="hand">The initial hand of the AI.</param>
        public DurakAI(string name, Cards hand) : base(name, hand)
        {
            ThinkDelay = 3000;
        }

        /// <summary>
        /// Resets the <see cref="DurakAI"/>'s chosen card and <see cref="Player.HasChosen"/> status. The AI then automatically chooses a card.
        /// </summary>
        /// <remarks>
        /// There is no need to invoke <see cref="Player.ChooseCard(int)"/> or nay of its overrides, as the <see cref="DurakAI"/> will
        /// already choose a card if one is playable. If not, it will automatically set its <see cref="Player.HasChosen"/> property to false;
        /// </remarks>
        /// <param name="river"></param>
        public override void TakeTurn(Cards river)
        {
            ResetChosen();
            DeterminePlayable(river);

            StartedThinking?.Invoke(this, EventArgs.Empty);
            // await Task.Delay(ThinkDelay)
            Task.Delay(ThinkDelay).Wait();  // currently blocks the thread, the client code would continue without having the updated object state

            if (!CanPlay)
            {
                ChosenCard = null;
                HasChosen = false;
                StoppedThinking?.Invoke(this, EventArgs.Empty);
                return;
            }

            if (IsAttacking)
                Attacking();
            else
                Defending();

            StoppedThinking?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// The method of choosing Cards when <see cref="Player.IsAttacking"/> is true, meaning the <see cref="DurakAI"/> is attacking.
        /// </summary>
        protected void Attacking()
        {
            var cardValues = ValueCards(PlayableCards);

            var bestCard = cardValues.OrderBy(kvp => kvp.Value).First().Key;

            ChooseCard(bestCard);
        }

        /// <summary>
        /// The method of choosing Cards when <see cref="Player.IsAttacking"/> is false, meaning the <see cref="DurakAI"/> is defending.
        /// </summary>
        protected void Defending()
        {
            Attacking();
        }

        /// <summary>
        /// Values cards based on their rank, emphasizing Trump cards if <see cref="CardBase.UseTrumps"/> is enabled, placing the result in a dictionary mapped
        /// with the <see cref="CardBase"/> as the key and <see cref="int"/> as the Value of the card.
        /// </summary>
        /// <param name="playable"></param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> with <see cref="CardBase"/> as the key and <see cref="int"/> as the value. </returns>
        protected static Dictionary<CardBase, int> ValueCards(Cards playable)
        {
            var values = new Dictionary<CardBase, int>();
            int aceValue = CardBase.IsAceHigh ? (int)Rank.King + 1 : (int)Rank.Ace;

            foreach (var card in playable)
            {
                int trumpIncrease = CardBase.UseTrumps && card.Suit == CardBase.Trump ? 1000 : 0;
                int cardValue = card.Rank == Rank.Ace ? aceValue : (int)card.Rank;

                values.Add(card, cardValue + trumpIncrease);
            }

            return values;
        }

    }
}

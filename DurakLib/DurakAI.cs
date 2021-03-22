using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardLib;

namespace DurakLib
{
    public class DurakAI : Player
    {
        public event EventHandler StartedThinking;
        public event EventHandler StoppedThinking;

        public int ThinkDelay { get; set; }

        public DurakAI(string name, Cards hand) : base(name, hand)
        {
            ThinkDelay = 3000;
        }

        public override async void TakeTurn(Cards river)
        {
            ResetChosen();
            DeterminePlayable(river);

            StartedThinking?.Invoke(this, EventArgs.Empty);
            await Task.Delay(ThinkDelay);

            if (PlayableCards.Count == 0)
            {
                ChosenCard = null;
                HasChosen = true;
                return;
            }

            if (IsAttacking)
                Attacking();
            else
                Defending();

            StoppedThinking?.Invoke(this, EventArgs.Empty);
        }

        protected void Attacking()
        {
            var cardValues = ValueCards(PlayableCards);

            var bestCard = cardValues.OrderBy(kvp => kvp.Value).First().Key;

            ChooseCard(bestCard);
        }

        protected void Defending()
        {
            Attacking();
        }

        protected static Dictionary<CardBase, int> ValueCards(Cards playable)
        {
            var values = new Dictionary<CardBase, int>();
            int aceValue = CardBase.IsAceHigh ? (int)Rank.King + 1 : (int)Rank.Ace;

            foreach (var card in playable)
            {
                int trumpIncrease = card.Suit == CardBase.Trump ? 1000 : 0;
                int cardValue = card.Rank == Rank.Ace ? aceValue : (int)card.Rank;

                values.Add(card, cardValue + trumpIncrease);
            }

            return values;
        }

    }
}

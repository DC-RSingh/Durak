// TODO: This should probably be moved to a different class library
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardLib
{
    public class DurakAI : Player
    {
        public int ThinkDelay { get; set; }

        public DurakAI(string name, Cards hand) : base(name, hand)
        {
            ThinkDelay = 3000;
        }

        public override async void TakeTurn(Cards river)
        {
            PlayableCards = (Cards) Hand.FindAll(card => IsPlayable(river, card));

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
        }

        protected void Attacking()
        {
            var cardValues = ValueCards(PlayableCards);

            var bestCard = cardValues.OrderBy(kvp => kvp.Value).First().Key;

            ChosenCard = bestCard;
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
                int cardValue = card.Rank == Rank.Ace ? aceValue : (int) card.Rank;

                values.Add(card, cardValue + trumpIncrease);
            }

            return values;
        }

    }
}

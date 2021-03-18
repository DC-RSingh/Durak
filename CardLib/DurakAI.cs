// TODO: This should probably be moved to a different class library
namespace CardLib
{
    public class DurakAI : Player
    {

        public DurakAI(string name, Cards hand) : base(name, hand)
        {
        }

        public void TakeTurn(Cards river)
        {
            var playable = (Cards) Hand.FindAll(card => IsPlayable(river, card));
            if (IsAttacking)
                Attacking(river, playable);
            else
                Defending(river, playable);
        }

        private void Attacking(Cards river, Cards playableCards)
        {

        }

        private void Defending(Cards river, Cards playableCards)
        {

        }

    }
}

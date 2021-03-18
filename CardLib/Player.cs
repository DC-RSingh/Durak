using System.Linq;

// TODO: This should probably be moved to a different class library
namespace CardLib
{
    public class Player
    {
        #region Properties

        public bool IsAttacking { get; set; }

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

        #endregion

        #region Methods
        // TODO: Might be useful to client code as well, will keep public for now
        public bool IsPlayable(Cards river, CardBase card)
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

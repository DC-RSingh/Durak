// TODO: This and DurakAI should probably be moved to a different class library
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

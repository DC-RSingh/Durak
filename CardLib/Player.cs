using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardLib
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    namespace CardLib
    {
        public class Player
        {
            private String _playerName;
            private String _playerType;

            /// <summary>
            /// Represents the deck
            /// </summary>
            private Cards _hand;

            /// <summary>
            /// Default Constructor
            /// </summary>
            private Player()
            {
                throw new System.NotImplementedException();
            }

            #region Properties

            public bool IsAttacking { get; set; }

            /// <summary>
            /// Represents the player's Hand
            /// </summary>
            public Cards Hand { get; private set; }

            /// <summary>
            /// Represents the set of cards the player has in hands
            /// </summary>
            public int HandSize => Hand.Count;

            /// <summary>
            /// Indicates player's name
            /// </summary>
            public string PlayerName { get; private set; }

            /// <summary>
            /// Indicates player's type
            /// </summary>
            public string PlayerType
            {
                get => default;
                set
                {
                }
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
}

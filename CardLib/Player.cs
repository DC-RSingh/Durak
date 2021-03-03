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
            private String playerName;
            private String playerType;
            /// <summary>
            /// Represents the deck
            /// </summary>
            private Deck deck;
            private Card hand;

            /// <summary>
            /// Default Constructor
            /// </summary>
            private Player()
            {
                throw new System.NotImplementedException();
            }

            /// <summary>
            /// Method referring to player
            /// </summary>
            /// <param name="playerName">Refers to player name</param>
            /// <param name="playerType">Refers to player type</param>
            public Player(string playerName, string playerType)
            {
                throw new System.NotImplementedException();
            }

            /// <summary>
            /// Represents the deck
            /// </summary>
            public Deck Deck
            {
                get => default;
                set
                {
                }
            }

            /// <summary>
            /// Represents the set of cards the player has in hands
            /// </summary>
            public int Hand
            {
                get => default;
                set
                {
                }
            }

            /// <summary>
            /// Indicates player's name
            /// </summary>
            public String PlayerName
            {
                get => default;
                set
                {
                }
            }

            /// <summary>
            /// Indicates player's type
            /// </summary>
            public String PlayerType
            {
                get => default;
                set
                {
                }
            }
        }
    }
}

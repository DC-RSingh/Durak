/**
 * OOP 4200 - Final Project - Durak
 * 
 * DeckSize.cs is a enum containing the different decksizes. In our application
 * we only make use of 20, 36, and 52.
 * 
 * @author      Raje Singh, Fleur Blanckaert, Gabriel Dietrich, Dalton Young
 * @version     1.0
 * @since       2021-02 
 */

namespace CardLib
{
    /// <summary>
    /// Represents different sizes a deck can have, with the value of each member indicating the number of ranks in the deck.
    /// </summary>
    public enum DeckSize
    {
        Four = 1,
        Eight,
        Twelve,
        Sixteen,
        Twenty,
        TwentyFour,
        TwentyEight,
        ThirtyTwo,
        ThirtySix,
        Forty,
        FortyFour,
        FortyEight,
        FiftyTwo
    }
}

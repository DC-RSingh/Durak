using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using CardLib;
using CardLib.CardLib;
using CardUI;

namespace Client
{
    public class GameplayTest
    {
        private static Deck<PlayingCard> playDeck;
        private Cards discardPile;   // Where Discarded Cards Go
        private Cards inPlayPile;   // Where the attacking and defending cards go
        private static Player[] players;
        private static int currentCard;    // Represents the current card

        private GameplayTest()
        {

        }

        public static void Play()
        {
            #region Deck and Discard Pile Init
            var deck = new Deck<PlayingCard>();

            var discardPile = new Cards();

            var inPlayPile = new Cards();

            var river = new Cards();

            const int sixCards = 6;

            deck.Shuffle();

            Logger.Log($"Current Size: {deck.Size}");
            #endregion

            #region Player 1 Init
            var player1 = new Player("Test Player", deck.Draw(6));

            DisplayPlayer(player1);

            #endregion

            #region Player 2 Init
            var player2 = new Player("AI", deck.Draw(6));

            DisplayPlayer(player2);

            #endregion

            #region Determine Trump
            var trumpCard = deck.Draw();

            CardBase.Trump = trumpCard.Suit;

            Logger.Log($"Trump Card: {trumpCard}");

            Logger.Log(deck.Size.ToString());
            #endregion

            #region Determine First Attacker
            var flip = new Random();

            if (flip.Next(0, 1) == 0)
            {
                player2.IsAttacking = true;
                player1.IsAttacking = false;
            }
            else
            {
                player2.IsAttacking = false;
                player1.IsAttacking = true;
            }
            #endregion
            

            /// <summary>
            /// Game play logic
            /// </summary>
            public int Game()
            {
                bool winner = false; // Indicates if there is a winner

                // Initialize game vars
                Card playCard = playDeck.GetCard(currentCard++);
                Card playCard2 = playDeck.GetCard(currentCard++);
                Card cardToBeat; // Last card played by either attacker or defender, depending on whose turn it is

                // Loops through game until there is a winner
                do
                { 
                    // If player1 has no cards on their hand, end game and display winner
                    if(player1.HandSize < 0)
                    {
                        Console.WriteLine("*******Player1 WON!!*********");
                        winner = true;
                    }
                    
                    // If player2 has no cards on their hand, end game and display winner
                    if (player2.HandSize < 0)
                    {
                        Console.WriteLine("*******Player2 WON!!*********");
                        winner = true;
                    }

                    // If player1 is attacking
                    if (player1.IsAttacking == true)
                    {
                        // Prompts player1 to choose next move
                        bool inputOK = false;
                        do
                        {
                            // If Player1 chooses a card to play a card (Maybe a click event here?)
                            Console.WriteLine("Choose your move:");
                            string playerMove = Console.ReadLine();
                            if (playerMove == "play")
                            {
                                // Prompts player1 to choose which card to play
                                inputOK = false;
                                int choice = -1;
                                do
                                {
                                    Console.WriteLine("Choose card to discard:");
                                    string input = Console.ReadLine();
                                    try
                                    {
                                        // Attempt to convert input into a valid card number.
                                        choice = Convert.ToInt32(input);
                                        if ((choice > 0) && (choice <= 7))
                                            inputOK = true;

                                        // HOW CAN WE CHECK IF CARD IS FROM THE SAME SUIT AS TRUMP?

                                        // Moves selected card to inPlayPile
                                        inPlayPile.Add(choice);
                                        
                                    }
                                    catch
                                    {
                                        // Ignore failed conversions, just continue prompting.
                                    }
                                } while (inputOK == false);

                                // Player2 turn to defend
                                // If Player2 chooses to defend by discarding a card
                                // How can we do this??
                                Console.WriteLine("Choose your move (player 2):");
                                string playerMove2 = Console.ReadLine();
                                if (playerMove2 == "play")
                                {
                                    // Prompts player2 to choose which card to play
                                    int choice2 = -1;
                                    do
                                    {
                                        Console.WriteLine("Choose card to discard:");
                                        string input = Console.ReadLine();

                                        try
                                        {
                                            // Attempt to convert input into a valid card number.
                                            choice2 = Convert.ToInt32(input);
                                            if ((choice2 > 0) && (choice2 <= 7)) // What if they have more than 7 cards?
                                                inputOK = true;

                                            // Moves selected card to inPlayPile
                                            inPlayPile.Add(choice2);

                                        }
                                        catch
                                        {
                                            // Ignore failed conversions, just continue prompting.
                                        }
                                    } while (inputOK == false);
                                    
                                    // Checks if card played by defender is greater than card from attacker
                                    if(choice2 > choice)
                                    {
                                        Console.WriteLine("player2 succesfully defended");
                                    }
                                }
                                // Else if player2 chooses to take cards on the table
                                else if (playerMove2 == "take")
                                {
                                    // Moves all cards from inPlayPile to DiscardPile
                                    inPlayPile.Remove(playCard);
                                    inPlayPile.Remove(playCard2);
                                    discardPile.Add(playCard);
                                    discardPile.Add(playCard2);

                                    // Loop to draw new cards until both players have at least 6 cards or no more cards on deck
                                    bool cardsToDraw = true;
                                    do
                                    {
                                        // If deck has cards to be drawn
                                        if (deck.Size <= 0)
                                        {
                                            cardsToDraw = false;
                                        }
                                        // otherwise, deck has cards to be drawn
                                        else
                                        {
                                            // If player1 hand size is smaller than 6 cards
                                            if (player1.HandSize < sixCards)
                                            {
                                                deck.Draw();  // Draw a card
                                            }

                                            // If player2 hand size is smaller than 6 cards
                                            if (player2.HandSize < sixCards)
                                            {
                                                deck.Draw();  // Draw a card
                                            }
                                        }
                                    }
                                    while (cardsToDraw == false || (player1.HandSize >= sixCards && player2.HandSize >= sixCards));

                                }


                            }
                            else if (playerMove == "end") // Else if Player1 chooses to end attack
                            {
                                // Moves all cards from inPlayPile to DiscardPile
                                inPlayPile.Remove(playCard);
                                inPlayPile.Remove(playCard2);
                                discardPile.Add(playCard);
                                discardPile.Add(playCard2);

                                // Loop to draw new cards until both players have at least 6 cards or no more cards on deck
                                bool cardsToDraw = true;
                                do
                                {
                                    // If deck has cards to be drawn
                                    if (deck.Size <= 0)
                                    {
                                        cardsToDraw = false;
                                    }
                                    // otherwise, deck has cards to be drawn
                                    else
                                    {
                                        // If player1 hand size is smaller than 6 cards
                                        if (player1.HandSize < sixCards)
                                        {
                                            deck.Draw();  // Draw a card
                                        }
                                        
                                        // If player2 hand size is smaller than 6 cards
                                        if (player2.HandSize < sixCards)
                                        {
                                            deck.Draw();  // Draw a card
                                        }
                                    }
                                }
                                while (cardsToDraw == false || (player1.HandSize >= sixCards && player2.HandSize >= sixCards));

                                // Switches turn to attack
                                player1.IsAttacking = false;
                                player2.IsAttacking = true;
                            }
                        }
                        while (inputOK == true);
                        
                        
                    }
                    // Else If, player2 is attacking
                    else if (player2.IsAttacking == true)
                    {
                        // Prompts player2 to choose next move
                        bool inputOK = false;
                        do
                        {
                            // If Player2 chooses a card to play a card (Maybe a click event here?)
                            Console.WriteLine("Choose your move:");
                            string playerMove2 = Console.ReadLine();
                            if (playerMove2 == "play")
                            {
                                // Prompts player1 to choose which card to play
                                inputOK = false;
                                int choice2 = -1;
                                do
                                {
                                    Console.WriteLine("Choose card to discard:");
                                    string input = Console.ReadLine();
                                    try
                                    {
                                        // Attempt to convert input into a valid card number.
                                        choice2 = Convert.ToInt32(input);
                                        if ((choice2 > 0) && (choice2 <= 7))
                                            inputOK = true;

                                        // Moves selected card to inPlayPile
                                        inPlayPile.Add(choice2);

                                    }
                                    catch
                                    {
                                        // Ignore failed conversions, just continue prompting.
                                    }
                                } while (inputOK == false);

                                // Player1 turn to defend
                                // If Player1 chooses to defend by discarding a card
                                // How can we do this??
                                Console.WriteLine("Choose your move:");
                                string playerMove = Console.ReadLine();
                                if (playerMove == "play")
                                {
                                    // Prompts player1 to choose which card to play
                                    int choice = -1;
                                    do
                                    {
                                        Console.WriteLine("Choose card to discard:");
                                        string input = Console.ReadLine();

                                        try
                                        {
                                            // Attempt to convert input into a valid card number.
                                            choice = Convert.ToInt32(input);
                                            if ((choice > 0) && (choice <= 7))
                                                inputOK = true;

                                            // Moves selected card to inPlayPile
                                            inPlayPile.Add(choice);

                                        }
                                        catch
                                        {
                                            // Ignore failed conversions, just continue prompting.
                                        }
                                    } while (inputOK == false);

                                    // Checks if card played by defender is greater than card from attacker
                                    if (choice2 > choice)
                                    {
                                        Console.WriteLine("player1 succesfully defended");
                                    }
                                }
                                // Else if player2 chooses to take cards on the table
                                else if (playerMove == "take")
                                {
                                    // Moves all cards from inPlayPile to DiscardPile
                                    inPlayPile.Remove(playCard);
                                    inPlayPile.Remove(playCard2);
                                    discardPile.Add(playCard);
                                    discardPile.Add(playCard2);

                                    // Loop to draw new cards until both players have at least 6 cards or no more cards on deck
                                    bool cardsToDraw = true;
                                    do
                                    {
                                        // If deck has cards to be drawn
                                        if (deck.Size <= 0)
                                        {
                                            cardsToDraw = false;
                                        }
                                        // otherwise, deck has cards to be drawn
                                        else
                                        {
                                            // If player1 hand size is smaller than 6 cards
                                            if (player2.HandSize < sixCards)
                                            {
                                                deck.Draw();  // Draw a card
                                            }

                                            // If player2 hand size is smaller than 6 cards
                                            if (player1.HandSize < sixCards)
                                            {
                                                deck.Draw();  // Draw a card
                                            }
                                        }
                                    }
                                    while (cardsToDraw == false || (player1.HandSize >= sixCards && player2.HandSize >= sixCards));

                                }


                            }
                            else if (playerMove2 == "end") // Else if Player2 chooses to end attack
                            {
                                // Moves all cards from inPlayPile to DiscardPile
                                inPlayPile.Remove(playCard);
                                inPlayPile.Remove(playCard2);
                                discardPile.Add(playCard);
                                discardPile.Add(playCard2);

                                // Loop to draw new cards until both players have at least 6 cards or no more cards on deck
                                bool cardsToDraw = true;
                                do
                                {
                                    // If deck has cards to be drawn
                                    if (deck.Size <= 0)
                                    {
                                        cardsToDraw = false;
                                    }
                                    // otherwise, deck has cards to be drawn
                                    else
                                    {
                                        // If player1 hand size is smaller than 6 cards
                                        if (player2.HandSize < sixCards)
                                        {
                                            deck.Draw();  // Draw a card
                                        }

                                        // If player2 hand size is smaller than 6 cards
                                        if (player1.HandSize < sixCards)
                                        {
                                            deck.Draw();  // Draw a card
                                        }
                                    }
                                }
                                while (cardsToDraw == false || (player1.HandSize >= sixCards && player2.HandSize >= sixCards));

                                // Switches turn to attack
                                player1.IsAttacking = true;
                                player2.IsAttacking = false;
                            }
                        }
                        while (inputOK == true);
                    }
                }
                while (winner == false);

                return 0; // Returns integer
            }

            // OBJECTIVES

            // - GET RID OF ALL YOUR CARDS

            // TODO: Play 
            // - If the Defender defeats a set number of attacks,
            // all cards played in the bout are discarded. If the Defender loses, they must pick up all the cards played

            // The attacker plays down a card from their hand which the defender must beat with a higher card of the same suit
            // or any trump card, similar to the standard trick rules.

            // The attack can contain no more than 6 cards, or the size of the defender’s hand at the start of the bout,
            // if that is less than 6.

            // If the defender fails at any point, attacking players can continue to play cards up to the attack’s size limit,
            // but the defender cannot defend again and the defender must add all the cards played in the bout to their hand.

            // If the defender beats all the attacking cards, they win the bout and all cards played in that bout are discarded.

            // At the end of the bout, players draw cards to replenish their hands back to 6 cards.
            // Once the draw pile is empty, play continues without drawing cards.

            // TODO: DEFENDING
            // You Can Only Play a Higher Ranking Card of the Same Suit or a Trump Suit Card
            // Choose Not to Play and take the Draw Pile
            // Can't play so take
            // When You Take you miss your turn to attack and your opponent gets to go again

            // TODO: ATTACKING
            // Attackers Choose Cards to Play
            // They "Throw into" the Opponent any rank card that is already on the table
            // After the first attack, attackers can only play cards of the same rank as cards already played in the bout

            // When a card is defended, the attacker can play another card, which must be of equal rank to at least one other
            // card already played in the bout.

            // TODO: TRANSFERRING
            // On the first turn of attack, if the defender plays a card of the same rank but different suit, they get to attack
            // instead.

        }

        private static void DisplayPlayer(Player player1)
        {
            for (int i = 0; i < player1.Hand.Count; i++)
            {
                Logger.Log($"{player1.PlayerName}'s Card {i + 1}: {player1.Hand.ElementAt(i)}");
            }
        }

        private static void DisplayDeck(Deck<CardBase> s)
        {
            for (var i = 0; i < s.Size; i++)
            {
                Logger.Log(s.GetCard(i).ToString());
            }
        }


    }
}

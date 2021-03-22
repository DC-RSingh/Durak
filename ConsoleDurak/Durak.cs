using System;
using System.Linq;
using CardLib;
using DurakLib;

namespace ConsoleDurak
{
    /// <summary>
    /// Plays a game of Durak in a Console!
    /// </summary>
    public static class Durak
    {
        #region FIELDS

        /// <summary>
        /// A deck of <see cref="Card"/> representing the Play Deck.
        /// </summary>
        private static Deck<Card> _playDeck;

        /// <summary>
        /// A <see cref="Cards"/> collection representing the cards in play during a bout.
        /// </summary>
        private static Cards _river;

        /// <summary>
        /// An array of <see cref="Player"/> containing the players of the game.
        /// </summary>
        private static Player[] _players;

        /// <summary>
        /// Represents the amount of turns that have elapsed (a turn ends when an attack ends)
        /// </summary>
        private static int _turnCount;

        /// <summary>
        /// Represents the amount of bouts during the current turn (a bout ends after both players move)
        /// </summary>
        private static int _boutCount;

        /// <summary>
        /// The trump card that was chosen.
        /// </summary>
        private static CardBase _trumpCard;

        #endregion

        /// <summary>
        /// Durak Entry Point.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        private static void Main(string[] args)
        {
            Console.Title = "Durak - The Fool!";

            #region GAME SETUP

            Console.WriteLine("********** Welcome to the Game of DURAK! **********");

            #region Prompt for Deck Size

            var sizeChoice = DeckSize.FiftyTwo; // Set Default DeckSize to 52
            var sizeChosen = false;
            do
            {
                ConsoleAlert("What Deck Size would you like to play with 52, 36 or 20 (Enter for 52): ", ConsoleColor.DarkYellow);
                var deckSizeChoice = Console.ReadLine();
                switch (deckSizeChoice)
                {
                    case "52":
                        sizeChoice = DeckSize.FiftyTwo;
                        sizeChosen = true;
                        break;
                    case "36":
                        sizeChoice = DeckSize.ThirtySix;
                        sizeChosen = true;
                        break;
                    case "20":
                        sizeChoice = DeckSize.Twenty;
                        sizeChosen = true;
                        break;
                    case "":
                        sizeChosen = true;
                        break;
                }
            } while (!sizeChosen);

            ConsoleAlert($"Deck of {sizeChoice} chosen.\n", ConsoleColor.Green);

            #endregion

            #region Deck and Discard Pile Init

            _playDeck = new Deck<Card>(true, true, size: sizeChoice);

            _river = new Cards();

            const int DRAW_AMT = 6; // The Amount of Cards to Players should have in their hand at the start of every turn

            _playDeck.Shuffle();

            #endregion

            #region Player 1 Init

            var player1 = new Player("Human", _playDeck.Draw(DRAW_AMT));

            #endregion

            #region Player 2 Init

            var player2 = new DurakAI("AI", _playDeck.Draw(DRAW_AMT));

            #endregion

            #region Determine Trump

            _trumpCard = _playDeck.Draw();

            CardBase.Trump = _trumpCard.Suit;

            Console.WriteLine($"Trump Card: {_trumpCard}");

            #endregion

            #region Randomly Select First Attacker

            var flip = new Random();

            if (flip.Next(2) == 0)  // Coin Flip, randomly choosing the first attacker
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

            #region Wire Events

            // Add the Necessary Event Handlers to the players events
            player1.TurnBegin += HumanPlayer_TurnStart;
            player1.TurnEnd += HumanPlayer_TurnEnd;

            player2.StartedThinking += AI_StartThink;
            player2.StoppedThinking += AI_StopThink;

            #endregion

            _players = new[] { player1, player2 };  // initialize the player array

            var winner = false; // set the winner boolean to false

            // Init Turn Count
            _turnCount = 0;

            #endregion

            #region GAME LOOP
            do
            {
                // Determine the winner
                foreach (var player in _players)
                {
                    if (player.HandSize != 0) continue;
                    Console.WriteLine($@"GAME OVER! {player.PlayerName} has won!");
                    winner = true;
                    break;
                }

                // If there is a winner, break from the game loop
                if (winner) break;

                // Increment Total Turn Count
                _turnCount++;

                // Determine the attacker
                var attacker = _players.First(player => player.IsAttacking);

                // Determine defender
                var defender = _players.First(player => !player.IsAttacking);

                // Current player (always start with attacking phase)
                var currentPlayer = attacker;

                var attackSuccess = true;
                var defenseSuccess = true;

                // Reset Bout Count
                _boutCount = 1;

                // Refill Hands and Prompt the Player with the Current Game State
                RefillHands(DRAW_AMT);
                PromptGameState(currentPlayer, attacker, defender);

                // Turn Loop (increments bout every iteration), ends when an attack is failed
                do
                {
                    attacker.TakeTurn(_river);  // Perform the attacker's turn, handled by the HumanPlayer_StartTurn and HumanPlayer_EndTurn event handlers

                    // if the attacker chose a card to play, add that card to the play pile (river)
                    if (attacker.HasChosen)
                    {
                        _river.Add(attacker.ChosenCard);
                        if (attacker.HandSize == 0) break;  // if the attacker played their last card, break from the turn loop
                    }
                    else
                    { // otherwise, clear the river and declare the attack a failure (set attackSuccess to false)
                        _river.Clear();
                        attackSuccess = false;
                        continue;
                    }

                    currentPlayer = defender;
                    PromptGameState(currentPlayer, attacker, defender);

                    defender.TakeTurn(_river);  // Perform the defender's turn, logic is handled by the class for the differences.

                    _boutCount++;

                    if (defender.HasChosen) // if the defender chose, add to the play pile
                    {
                        _river.Add(defender.ChosenCard);
                        if (defender.HandSize == 0) break;
                    }
                    else
                    {   // otherwise, prompt that the defense failed and add the play pile (river) to the defender's hand
                        ConsoleAlert($"Defense Failed! {defender.PlayerName} will now take the River Cards!\n", ConsoleColor.DarkRed);
                        defender.Hand.AddRange(_river);
                        _river.Clear();
                        defenseSuccess = false;
                        
                    }

                    currentPlayer = attacker;   // Switch the current player to the attacker
                    PromptGameState(currentPlayer, attacker, defender); // Prompt the user with the game state
                } while (attackSuccess && defenseSuccess);

                if (defenseSuccess) // if the defense was a success, prompt user that attack failed and swap roles
                {
                    ConsoleAlert("\nAttack Failed! Switching attackers...", ConsoleColor.DarkRed);

                    // swap attacker and defender
                    foreach (var player in _players)
                    {
                        player.IsAttacking = !player.IsAttacking;
                    }
                }
                else
                {   // otherwise, the current attacker gets to attack again
                    ConsoleAlert($"\nAttack Success! {attacker.PlayerName} is attacking again!\n", ConsoleColor.Green);
                }

            } while (true);
            #endregion

            Console.WriteLine("Thanks for Playing! Press any key to exit...");
            Console.ReadKey();

        }

        #region EVENT HANDLERS

        /// <summary>
        /// An EventHandler that handles most of the game logic for the Human <see cref="Player"/> turn in Durak, outputting to and requesting input from the Console.
        /// </summary>
        /// <remarks>EventArgs passed is always <seealso cref="EventArgs.Empty"/>.</remarks>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The EventArgs of the event</param>
        private static void HumanPlayer_TurnStart(object sender, EventArgs e)
        {
            if (sender is Player player)
            {
                // Ask the Player if they wish to play
                bool moveValid = false;
                var moveChoice = "";
                var validChoices = new[] { "p", "e", "play", "end" };
                DisplayCards(player.Hand, "Your Hand");

                // Ask for Move Loop
                do
                {
                    if (_boutCount == 1 && player.IsAttacking)
                    {
                        ConsoleAlert("You must select a card to attack on the first bout!\n", ConsoleColor.Yellow);
                        moveChoice = "p";
                        break;
                    }

                    if (!player.CanPlay)
                    {
                        ConsoleAlert("You have no cards you can play!\n", ConsoleColor.Yellow);
                        moveChoice = "e";
                        break;
                    }

                    try
                    {
                        ConsoleAlert("What will you do, '(P)lay' or '(E)nd'? ", ConsoleColor.DarkYellow);
                        moveChoice = Console.ReadLine();

                        if (validChoices.Contains(moveChoice.ToLower()))
                            moveValid = true;
                    }
                    catch
                    {
                        // Ignore and keep prompting
                    }
                } while (!moveValid);

                // If the player chose to play, continue with prompting for card to play
                if (!moveChoice.StartsWith("p")) return;

                // Prompts player1 to choose which card to play
                bool inputOk = false;

                // Ask for Play Card Loop
                do
                {
                    DisplayCards(player.PlayableCards, "Your Playable Cards");

                    ConsoleAlert("Choose card to play: ", ConsoleColor.DarkYellow);
                    string input = Console.ReadLine();
                    try
                    {
                        // Attempt to convert input into a valid card number.
                        var choice = Convert.ToInt32(input);
                        if (choice > 0 && choice <= player.PlayableCards.Count)
                            inputOk = true;

                        player.ChooseCard(choice - 1);
                    }
                    catch
                    {
                        // Ignore failed conversions, just continue prompting.
                    }
                } while (inputOk == false);
            }
        }

        /// <summary>
        /// An EventHandler that executes at the end of a Human <see cref="Player"/>'s Turn.
        /// </summary>
        /// <remarks>EventArgs passed is always <seealso cref="EventArgs.Empty"/>.</remarks>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The EventArgs of the event</param>
        private static void HumanPlayer_TurnEnd(object sender, EventArgs e)
        {
            if (sender is Player player)
            {
            }
        }

        /// <summary>
        /// An EventHandler that executes when <see cref="DurakAI"/> "starts thinking".
        /// </summary>
        /// <remarks>EventArgs passed is always <seealso cref="EventArgs.Empty"/>.</remarks>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The EventArgs of the event.</param>
        private static void AI_StartThink(object sender, EventArgs e)
        {
            if (sender is DurakAI ai)
            {
                Console.WriteLine($"{ai.PlayerName} is Thinking...");
            }
        }

        /// <summary>
        /// An EventHandler that executes when <see cref="DurakAI"/> "stops thinking".
        /// </summary>
        /// <remarks>EventArgs passed is always <seealso cref="EventArgs.Empty"/>.</remarks>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The EventArgs of the event.</param>
        private static void AI_StopThink(object sender, EventArgs e)
        {
            if (sender is DurakAI ai)
            {
                ConsoleAlert(ai.CanPlay ? $"{ai.PlayerName} found a Move! Playing {ai.ChosenCard}.\n" : $"{ai.PlayerName} could not Find a Move! Passing...\n", ConsoleColor.Green);
            }
            
        }
        #endregion

        #region PRIVATE HELPER METHODS

        /// <summary>
        /// Performs a <see cref="Console.Write(string)"/> of the specified string and sets a <see cref="ConsoleColor"/> for that message.
        /// </summary>
        /// <remarks>Be careful with <b>\n</b> at the end of messages before reading from the <see cref="System.Console.In"/> stream.</remarks>
        /// <param name="message">The message to write.</param>
        /// <param name="color">The color to set the foreground to.</param>
        private static void ConsoleAlert(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Sets the number of cards in hands of the <see cref="Player"/> instances in <see cref="_players"/>, alternating until target amount
        /// is met or the <see cref="_playDeck"/> is empty.
        /// </summary>
        /// <param name="refillTo">The amount to refill the hands to.</param>
        private static void RefillHands(int refillTo)
        {
            if (!_playDeck.CanDraw) return;

            var refilledCount = 0;
            do
            {
                foreach (var player in _players)
                {
                    if (player.HandSize >= refillTo)
                    {
                        refilledCount++;
                        continue;
                    }

                    if (!_playDeck.CanDraw) continue;
                    player.Hand.Add(_playDeck.Draw());
                    if (player.HandSize == refillTo) refilledCount++;
                }
            } while (_playDeck.CanDraw && refilledCount < _players.Length);

        }

        /// <summary>
        /// Outputs to the console information relevant to the current state of the Durak game.
        /// </summary>
        /// <param name="currentPlayer">The player who is currently making their move.</param>
        /// <param name="attacker">The player who is the attacker in the current turn.</param>
        /// <param name="defender">The player who is the defender in the current turn.</param>
        private static void PromptGameState(Player currentPlayer, Player attacker, Player defender)
        {
            #region Game State Info

            // Wait for User Input to Continue
            Console.Write("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("                  DURAK                  ");
            Console.WriteLine("=========================================");

            // Output Deck Size, Turn and Bout Count and Current Player and Phase in DarkCyan
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"Current Deck Size: {_playDeck.Size}");
            Console.WriteLine($"Turn: {_turnCount} | Bout: {_boutCount}");
            var currentPhase = currentPlayer == attacker ? "Attacking" : "Defending";
            Console.WriteLine($"Currently Playing: {currentPlayer.PlayerName} | Current Phase: {currentPhase}");

            // Output This Turn's Attacker and Defender and their Hand Size in Magenta
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Turn Attacker: {attacker.PlayerName} | Hand Size: {attacker.HandSize}");
            Console.WriteLine($"Turn Defender: {defender.PlayerName} | Hand Size: {defender.HandSize}");

            // Output Trump Card and Suit in DarkCyan
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"TRUMP CARD: {_trumpCard} | TRUMP SUIT: {_trumpCard.Suit}");

            // Output River Card Count and Cards in DarkBlue
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine($"# of Cards in River: {_river.Count}");

            DisplayCards(_river);
            #endregion

            Console.ResetColor();

            Console.WriteLine(_river.Any() ? $"Last Card Played: {_river.Last()}" : "No Cards in the River!");
            Console.WriteLine("=========================================\n");
        }

        /// <summary>
        /// Displays the contents of a <see cref="Player"/> Hand.
        /// </summary>
        /// <param name="player1">The player to display.</param>
        private static void DisplayPlayer(Player player1)
        {
            for (int i = 0; i < player1.Hand.Count; i++)
            {
                Console.WriteLine($"{player1.PlayerName}'s Card {i + 1}: {player1.Hand.CardAt(i)}");
            }
        }

        /// <summary>
        /// Displays the contents of a <see cref="Deck{Card}"/>.
        /// </summary>
        /// <param name="s">The deck to display.</param>
        private static void DisplayDeck(Deck<Card> s)
        {
            for (var i = 0; i < s.Size; i++)
            {
                Console.WriteLine(s.GetCard(i).ToString());
            }
        }

        /// <summary>
        /// Displays the contents of a <see cref="Cards"/> collection. If <paramref name="displayName"/> is passed, output is formatted to include
        /// title of the collection.
        /// </summary>
        /// <param name="cards">The cards to display.</param>
        /// <param name="displayName">The display name, if any.</param>
        private static void DisplayCards(Cards cards, string displayName = "")
        {
            var isDisplayNameSet = !displayName.Equals("");
            if (isDisplayNameSet)
                Console.WriteLine($"\n--------------------------------------\n {displayName} \n--------------------------------------");

            var cardList = "";
            for (var index = 0; index < cards.Count; index++)
            {
                var cardBase = cards[index];
                if (index != cards.Count - 1)
                    cardList += $"{index + 1}: {cardBase}, ";
                else
                    cardList += $"{index + 1}: {cardBase}";
            }

            if (cardList != "") Console.WriteLine(cardList);

            if (isDisplayNameSet)
                Console.WriteLine("---------------------------------------\n");
        }

        #endregion
    }
}

using System;
using System.Linq;
using CardLib;
using DurakLib;

namespace ConsoleDurak
{
    public static class Durak
    {
        #region FIELDS

        private static Deck<Card> _playDeck;
        private static Cards _river;
        private static Player[] _players;
        private static int _turnCount;
        private static int _boutCount;
        private static CardBase _trumpCard;

        #endregion

        private static void Main(string[] args)
        {
            Console.Title = "Durak - The Fool!";

            #region GAME SETUP

            Console.WriteLine("********** Welcome to the Game of DURAK! **********");

            #region Prompt for Deck Size

            var sizeChoice = DeckSize.FiftyTwo;
            var sizeChosen = false;
            do
            {
                ConsoleAlert("What Deck Size would you like to play with 52, 36 or 20 (52 is Default): ", ConsoleColor.DarkYellow);
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

            const int DRAW_AMT = 6;

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

            if (flip.Next(2) == 0)
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

            player1.TurnBegin += HumanPlayer_TurnStart;
            player1.TurnEnd += HumanPlayer_TurnEnd;

            player2.StartedThinking += AI_StartThink;
            player2.StoppedThinking += AI_StopThink;

            #endregion

            _players = new[] { player1, player2 };

            var winner = false;

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

                RefillHands(DRAW_AMT);
                PromptGameState(currentPlayer, attacker, defender);

                do
                {
                    attacker.TakeTurn(_river);

                    if (attacker.HasChosen)
                    {
                        _river.Add(attacker.ChosenCard);
                        if (attacker.HandSize == 0) break;
                    }
                    else
                    {
                        _river.Clear();
                        attackSuccess = false;
                        continue;
                    }

                    currentPlayer = defender;
                    PromptGameState(currentPlayer, attacker, defender);

                    defender.TakeTurn(_river);

                    _boutCount++;

                    if (defender.HasChosen)
                    {
                        _river.Add(defender.ChosenCard);
                        if (defender.HandSize == 0) break;
                    }
                    else
                    {
                        ConsoleAlert($"Defense Failed! {defender.PlayerName} will now take the River Cards!\n", ConsoleColor.DarkRed);
                        defender.Hand.AddRange(_river);
                        _river.Clear();
                        defenseSuccess = false;
                        
                    }

                    currentPlayer = attacker;
                    PromptGameState(currentPlayer, attacker, defender);
                } while (attackSuccess && defenseSuccess);

                if (defenseSuccess)
                {
                    ConsoleAlert("Attack Failed! Switching attackers...\n", ConsoleColor.DarkRed);

                    // swap attacker and defender
                    foreach (var player in _players)
                    {
                        player.IsAttacking = !player.IsAttacking;
                    }
                }
                else
                {
                    ConsoleAlert($"Attack Success! {attacker.PlayerName} is attacking again!\n", ConsoleColor.Green);
                }

            } while (true);
            #endregion

            Console.WriteLine("Thanks for Playing! Press any key to exit...");
            Console.ReadKey();

        }

        #region EVENT HANDLERS

        private static void HumanPlayer_TurnStart(object sender, EventArgs e)
        {
            if (sender is Player player)
            {
                // Ask the Player if they wish to play
                bool moveValid = false;
                var moveChoice = "";
                var validChoices = new[] { "p", "e", "play", "end" };
                DisplayCards(player.Hand, "Your Hand");
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
                        ConsoleAlert(@"You have no cards you can play!\n", ConsoleColor.Yellow);
                        moveChoice = "e";
                        break;
                    }

                    try
                    {
                        ConsoleAlert(@"What will you do, '(P)lay' or '(E)nd'? ", ConsoleColor.DarkYellow);
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
                do
                {
                    DisplayCards(player.PlayableCards, "Your Playable Cards");

                    ConsoleAlert(@"Choose card to play: ", ConsoleColor.DarkYellow);
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

        private static void HumanPlayer_TurnEnd(object sender, EventArgs e)
        {
            if (sender is Player player)
            {
            }
        }

        private static void AI_StartThink(object sender, EventArgs e)
        {
            if (sender is DurakAI ai)
            {
                Console.WriteLine($"{ai.PlayerName} is Thinking...");
            }
        }

        private static void AI_StopThink(object sender, EventArgs e)
        {
            if (sender is DurakAI ai)
            {
                ConsoleAlert(ai.CanPlay ? $"{ai.PlayerName} found a Move! Playing {ai.ChosenCard}.\n" : $"{ai.PlayerName} could not Find a Move! Passing...\n", ConsoleColor.Green);
            }
            
        }
        #endregion

        #region PRIVATE HELPER METHODS

        private static void ConsoleAlert(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ResetColor();
        }

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

                }
            } while (_playDeck.CanDraw && refilledCount != _players.Length);

        }

        private static void PromptGameState(Player currentPlayer, Player attacker, Player defender)
        {
            #region Game State Info

            Console.Write("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("=========================================");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"Current Deck Size: {_playDeck.Size}");
            Console.WriteLine($"Turn: {_turnCount} | Bout: {_boutCount}");
            var currentPhase = currentPlayer == attacker ? "Attacking" : "Defending";
            Console.WriteLine($"Currently Playing: {currentPlayer.PlayerName} | Current Phase: {currentPhase}");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Turn Attacker: {attacker.PlayerName} | Hand Size: {attacker.HandSize}");
            Console.WriteLine($"Turn Defender: {defender.PlayerName} | Hand Size: {defender.HandSize}");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"TRUMP CARD: {_trumpCard} | TRUMP SUIT: {_trumpCard.Suit}");

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine($"# of Cards in River: {_river.Count}");

            DisplayCards(_river);
            #endregion

            Console.ResetColor();

            Console.WriteLine(_river.Any() ? $"Last Card Played: {_river.Last()}" : "No Cards in the River!");
            Console.WriteLine("=========================================\n");
        }

        private static void DisplayPlayer(Player player1)
        {
            for (int i = 0; i < player1.Hand.Count; i++)
            {
                Console.WriteLine($"{player1.PlayerName}'s Card {i + 1}: {player1.Hand.CardAt(i)}");
            }
        }

        private static void DisplayDeck(Deck<Card> s)
        {
            for (var i = 0; i < s.Size; i++)
            {
                Console.WriteLine(s.GetCard(i).ToString());
            }
        }

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

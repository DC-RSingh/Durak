using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CardLib;
using CardUI;
using DurakLib;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : UserControl
    {
        public GameView()
        {
            InitializeComponent();
            Play();


        }

        #region FIELDS

        /// <summary>
        /// A deck of <see cref="Card"/> representing the Play Deck.
        /// </summary>
        private static Deck<PlayingCard> _playDeck;

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
        private static PlayingCard _trumpCard;

        /// <summary>
        /// The amount, in points, that CardBox controls are enlarged when hovered over. 
        /// </summary>
        private const int POP = 25;

        /// <summary>
        /// The height of an image box control 
        /// </summary>
        private const int regularHeight = 107;

        /// <summary>
        /// The width of a image box control 
        /// </summary>
        private const int regularWidth = 75;

        #endregion

        #region METHODS
        public void Play()
        {
            #region Prompt for Deck Size

            var sizeChoice = DeckSize.FiftyTwo; // Set Default DeckSize to 52
            var sizeChosen = false;
            //do
            //{
            //    ConsoleAlert("What Deck Size would you like to play with 52, 36 or 20 (Enter for 52): ", ConsoleColor.DarkYellow);
            //    var deckSizeChoice = Console.ReadLine();
            //    switch (deckSizeChoice)
            //    {
            //        case "52":
            //            sizeChoice = DeckSize.FiftyTwo;
            //            sizeChosen = true;
            //            break;
            //        case "36":
            //            sizeChoice = DeckSize.ThirtySix;
            //            sizeChosen = true;
            //            break;
            //        case "20":
            //            sizeChoice = DeckSize.Twenty;
            //            sizeChosen = true;
            //            break;
            //        case "":
            //            sizeChosen = true;
            //            break;
            //    }
            //} while (!sizeChosen);

            //ConsoleAlert($"Deck of {sizeChoice} chosen.\n", ConsoleColor.Green);

            #endregion

            #region Deck and Discard Pile Init
            _playDeck = new Deck<PlayingCard>(true, true, size: sizeChoice);

            _river = new Cards();

            const int DRAW_AMT = 6; // The Amount of Cards to Players should have in their hand at the start of every turn

            _playDeck.Shuffle();
            #endregion

            #region Player 1 Init

            var player1 = new Player("Human", _playDeck.Draw(DRAW_AMT));
            //DisplayHand(player1);

            #endregion

            #region Player 2 Init

            var player2 = new DurakAI("AI", _playDeck.Draw(DRAW_AMT));
            DisplayHand(player2);

            #endregion

            #region Determine Trump

            _trumpCard = _playDeck.Draw();

            CardBase.Trump = _trumpCard.Suit;

            var img = _trumpCard.UpdateCardImage();

            pnlDeck.Children.Add(img);

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
            player1.TakeTurn(_river);
            //player1.TurnEnd += HumanPlayer_TurnEnd;
            //player2.StartedThinking += AI_StartThink;
            //player2.StoppedThinking += AI_StopThink;

            #endregion

            #region PLAYER ARRAY Init
            _players = new[] { player1, player2 };  // initialize the player array

            var winner = false; // set the winner boolean to false

            // Init Turn Count
            _turnCount = 0;

            #endregion

            #region GAME LOOP

            //do
            //{
            //    // Determine the winner
            //    foreach (var player in _players)
            //    {
            //        if (player.HandSize != 0) continue;
            //        MessageBox.Show($@"GAME OVER! {player.PlayerName} has won!");
            //        winner = true;
            //        break;
            //    }

            //    // If there is a winner, break from the game loop
            //    if (winner) break;

            //    // Increment Total Turn Count
            //    _turnCount++;

            //    // Determine the attacker
            //    var attacker = _players.First(player => player.IsAttacking);

            //    // Determine defender
            //    var defender = _players.First(player => !player.IsAttacking);

            //    // Current player (always start with attacking phase)
            //    var currentPlayer = attacker;

            //    var attackSuccess = true;
            //    var defenseSuccess = true;

            //    // Reset Bout Count
            //    _boutCount = 1;

            //    // Refill Hands and Prompt the Player with the Current Game State
            //    RefillHands(DRAW_AMT);
            //    PromptGameState(currentPlayer, attacker, defender);

            //    // Turn Loop (increments bout every iteration), ends when an attack is failed
            //    do
            //    {
            //        attacker.TakeTurn(_river);  // Perform the attacker's turn, handled by the HumanPlayer_StartTurn and HumanPlayer_EndTurn event handlers

            //        // if the attacker chose a card to play, add that card to the play pile (river)
            //        if (attacker.HasChosen)
            //        {
            //            _river.Add(attacker.ChosenCard);
            //            if (attacker.HandSize == 0) break;  // if the attacker played their last card, break from the turn loop
            //        }
            //        else
            //        { // otherwise, clear the river and declare the attack a failure (set attackSuccess to false)
            //            _river.Clear();
            //            attackSuccess = false;
            //            continue;
            //        }

            //        currentPlayer = defender;
            //        PromptGameState(currentPlayer, attacker, defender);

            //        defender.TakeTurn(_river);  // Perform the defender's turn, logic is handled by the class for the differences.

            //        _boutCount++;

            //        if (defender.HasChosen) // if the defender chose, add to the play pile
            //        {
            //            _river.Add(defender.ChosenCard);
            //            if (defender.HandSize == 0) break;
            //        }
            //        else
            //        {   // otherwise, prompt that the defense failed and add the play pile (river) to the defender's hand
            //            MessageBox.Show($"Defense Failed! {defender.PlayerName} will now take the River Cards!\n");
            //            defender.Hand.AddRange(_river);
            //            _river.Clear();
            //            defenseSuccess = false;

            //        }

            //        currentPlayer = attacker;   // Switch the current player to the attacker
            //        PromptGameState(currentPlayer, attacker, defender); // Prompt the user with the game state
            //    } while (attackSuccess && defenseSuccess);

            //    if (defenseSuccess) // if the defense was a success, prompt user that attack failed and swap roles
            //    {
            //        MessageBox.Show("\nAttack Failed! Switching attackers...");

            //        // swap attacker and defender
            //        foreach (var player in _players)
            //        {
            //            player.IsAttacking = !player.IsAttacking;
            //        }
            //    }
            //    else
            //    {   // otherwise, the current attacker gets to attack again
            //        MessageBox.Show($"\nAttack Success! {attacker.PlayerName} is attacking again!\n");
            //    }

            //} while (true);
            #endregion

            lblDeckSize.Content = $"Remaining {_playDeck.Size}";
        }

        #endregion

        #region EVENT HANDLERS

        /// <summary>
        /// An EventHandler that handles most of the game logic for the Human <see cref="Player"/> turn in Durak, outputting to and requesting input from the Console.
        /// </summary>
        /// <remarks>EventArgs passed is always <seealso cref="EventArgs.Empty"/>.</remarks>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The EventArgs of the event</param>
        private void HumanPlayer_TurnStart(object sender, EventArgs e)
        {
            if (sender is Player player)
            {
                // Ask the Player if they wish to play
                lbl1.Content = "Player 1 Starts!";
                //DisplayHand(player);
                //bool moveValid = false;
                //var moveChoice = "";
                //var validChoices = new[] { "p", "e", "play", "end" };
                //Canvas myCanvas = new Canvas();
                Canvas canvas = pnlPlayerHand;

                for (int i = 0; i < player.Hand.Count; i++)
                {
                    
                    PlayingCard player_card = new PlayingCard(player.Hand.ElementAt(i).Suit, player.Hand.ElementAt(i).Rank, player.Hand.ElementAt(i).Face);

                    //Updates card image
                    var img = player_card.UpdateCardImage();

                    //Adds image to the canvas panel
                    canvas.Children.Add(img);                   

                    MouseEvents(img, canvas);
                }
                Animate.RealignCards(canvas);


                //// Ask for Move Loop
                //do
                //{
                    if (_boutCount == 1 && player.IsAttacking)
                    {
                        lbl1.Content = "Player 1 is Attacking!";
                    //ConsoleAlert("You must select a card to attack on the first bout!\n", ConsoleColor.Yellow);
                    //moveChoice = "p";
                    //break;
                    }

                    if (!player.CanPlay)
                    {
                        //TODO: Disable Images?
                        //TODO: Add button click event
                        lbl1.Content = "You have no cards! End your turn...";                        
                        //ConsoleAlert("You have no cards you can play!\n", ConsoleColor.Yellow);
                        //moveChoice = "e";
                        //break;
                     }

                //try
                //{
                //    MessageBox.Show("What will you do, '(P)lay' or '(E)nd'? ");
                //    moveChoice = Console.ReadLine();



                //    if (validChoices.Contains(moveChoice.ToLower()))
                //        moveValid = true;
                //}
                //catch
                //{
                //    // Ignore and keep prompting
                //}
                //} while (!moveValid);

                //// If the player chose to play, continue with prompting for card to play
                //if (!moveChoice.StartsWith("p")) return;

                //Prompts player1 to choose which card to play
                //bool inputOk = false;

                //// Ask for Play Card Loop
                //do
                //{
                //    DisplayHand(player);

                //    lbl1.Content = "Choose card to play!";
                //    string input = Console.ReadLine();
                //    try
                //    {
                //        // Attempt to convert input into a valid card number.
                //        var choice = Convert.ToInt32(input);
                //        if (choice > 0 && choice <= player.PlayableCards.Count)
                //            inputOk = true;

                //        player.ChooseCard(choice - 1);
                //    }
                //    catch
                //    {
                //        // Ignore failed conversions, just continue prompting.
                //    }
                //} while (inputOk == false);
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
                MessageBox.Show(ai.CanPlay ? $"{ai.PlayerName} found a Move! Playing {ai.ChosenCard}.\n" : $"{ai.PlayerName} could not Find a Move! Passing...\n");
            }

        }
        #endregion

        #region HELPER METHODS


        /// <summary>
        /// Displays the contents of a <see cref="Cards"/> collection. If <paramref name="displayName"/> is passed, output is formatted to include
        /// title of the collection.
        /// </summary>
        /// <param name="cards">The cards to display.</param>
        /// <param name="displayName">The display name, if any.</param>
        private void DisplayCards(Cards cards)
        {
            for (var index = 0; index < cards.Count; index++)
            {
                var cardBase = (PlayingCard)cards[index];
                if (index != cards.Count - 1)

                pnlPlayerHand.Children.Add(cardBase.UpdateCardImage());
            }
        }

        private void DisplayHand(Player player)
        {
            //Canvas myCanvas = new Canvas()

            Canvas canvas = pnlPlayerHand;            

            for (int i = 0; i < player.Hand.Count; i++)
            {
                // Turn AI cards down
                if (player.PlayerName == "AI")
                {
                    player.Hand.ElementAt(i).Face = Face.Down;
                    canvas = pnlAIHand;
                }

                PlayingCard test_card = new PlayingCard(player.Hand.ElementAt(i).Suit, player.Hand.ElementAt(i).Rank, player.Hand.ElementAt(i).Face);

                //Updates card image
                var img = test_card.UpdateCardImage();

                //Adds image to the canvas panel
                canvas.Children.Add(img);
               
            }

            Animate.RealignCards(canvas);
        }

        private void MouseEvents(Image img, Canvas canvas)
        {
            img.MouseEnter += (sender, args) =>
            {
                // Convert sender to a CardBox
                img = sender as Image;

                // If the conversion worked
                if (img != null)
                {
                    // if the card is in the home panel...
                    if (img.Parent == canvas)
                    {
                        // Enlarge the card for visual effect
                        img.Height += POP;
                        img.Width += POP;

                        // move the card to the top edge of the panel.
                        Canvas.SetTop(img, 0);
                    }
                    else
                    {
                        //Disable images
                        DisableImage(img);
                    }
                }
            };

            img.MouseLeave += (sender, args) =>
            {
                // Enlarge the card for visual effect
                img.Height = regularHeight;
                img.Width = regularWidth;

                // move the card to the top edge of the panel.
                Canvas.SetTop(img, POP);
            };

            img.MouseLeftButtonDown += (sender, args) =>
            {
                // Convert sender to a CardBox
                img = sender as Image;

                // If the conversion worked
                if (img != null)
                {

                    // if the card is in the home panel...
                    if (img.Parent == canvas)
                    {
                        // Remove the card from the home panel

                        canvas.Children.Remove(img);
                        pnlRiver.Children.Add(img);
                    }
                    else
                    {
                        //TODO: Disable images
                        DisableImage(img);

                    }
                }

                // Realign the cards 
                Animate.RealignCards(canvas);
                Animate.RealignCards(pnlRiver);
            };
        }
        

        ///// <summary>
        ///// Repositions the cards in a panel so that they are evenly distributed in the area available.
        ///// </summary>
        ///// @see 
        ///// <param name="panelHand"></param>
        //private void RealignCards(Canvas canvasHand)
        //{
        //    // Determine the number of cards/controls in the panel.
        //    int myCount = canvasHand.Children.Count;

        //    // If there are any cards in the panel
        //    if (myCount > 0)
        //    {
        //        // Determine how wide one card/control is.

        //        int cardWidth = 50;
        //        // Determine where the left-hand edge of a card/control placed 
        //        // in the middle of the panel should be  
        //        int startPoint = (int)((canvasHand.Width - cardWidth) / 2);

        //        // An offset for the remaining cards
        //        int offset = 0;

        //        // If there are more than one cards/controls in the panel
        //        if (myCount > 1)
        //        {
        //            // Determine how wide one card/control is.
        //            offset = (int)((canvasHand.Width - cardWidth - 2 * POP) / (myCount - 1));

        //            // If the offset is bigger than the card/control width, i.e. there is lots of room, 
        //            // set the offset to the card width. The cards/controls will not overlap at all.
        //            if (offset > cardWidth)
        //                offset = cardWidth;


        //            // Determine width of all the cards/controls 
        //            int allCardsWidth = (myCount - 1) * offset + cardWidth;

        //            startPoint = (int)((canvasHand.Width - allCardsWidth) / 2);

        //        }

        //        // Aligning the cards: Note that I align them in reserve order from how they
        //        // are stored in the controls collection. This is so that cards on the left 
        //        // appear underneath cards to the right. This allows the user to see the rank
        //        // and suit more easily.
        //        //// Align the "first" card (which is the last control in the collection)
        //        Canvas.SetTop(canvasHand.Children[myCount - 1], POP);
        //        Canvas.SetRight(canvasHand.Children[myCount - 1], startPoint);


        //        // for each of the remaining controls, in reverse order.
        //        for (int index = myCount - 2; index >= 0; index--)
        //        {
        //            // Align the current card
        //            Canvas.SetTop(canvasHand.Children[index], POP);
        //            Canvas.SetRight(canvasHand.Children[index], Canvas.GetRight(canvasHand.Children[index + 1]) + offset);
        //        }

        //    }
        //}

        /// <summary>
        /// Disables an image in the playing field 
        /// </summary>
        /// <param name="img"></param>
        private void DisableImage(Image img)
        {
            img.Height = regularHeight;
            img.Width = regularWidth;
        }

        private void DisplayDeck<T>(Deck<T> s) where T : Card
        {
            for (var i = 0; i < s.Size; i++)
            {
                s.GetCard(i);
                //MessageBox.Show(s.GetCard(i).ToString());

                PlayingCard test_card = new PlayingCard(s.GetCard(i).Suit, s.GetCard(i).Rank, s.GetCard(i).Face = Face.Down);

                var img = test_card.UpdateCardImage();

                //Adds image to the canvas panel
                pnlDeck.Children.Add(img);

            }
        }

        /// <summary>
        /// Sets the number of cards in hands of the <see cref="Player"/> instances in <see cref="_players"/>, alternating until target amount
        /// is met or the <see cref="_playDeck"/> is empty.
        /// </summary>
        /// <param name="refillTo">The amount to refill the hands to.</param>
        private void RefillHands(int refillTo)
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

                    //Displays the updated hand
                    DisplayHand(player);

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

            //// Wait for User Input to Continue
            //Console.Write("\nPress any key to continue...");
            //Console.ReadKey();
            //Console.Clear();

            //Console.WriteLine("                  DURAK                  ");
            //Console.WriteLine("=========================================");

            //// Output Deck Size, Turn and Bout Count and Current Player and Phase in DarkCyan
            //Console.ForegroundColor = ConsoleColor.DarkCyan;
            //Console.WriteLine($"Current Deck Size: {_playDeck.Size}");
            //Console.WriteLine($"Turn: {_turnCount} | Bout: {_boutCount}");
            //var currentPhase = currentPlayer == attacker ? "Attacking" : "Defending";
            //Console.WriteLine($"Currently Playing: {currentPlayer.PlayerName} | Current Phase: {currentPhase}");

            //// Output This Turn's Attacker and Defender and their Hand Size in Magenta
            //Console.ForegroundColor = ConsoleColor.Magenta;
            //Console.WriteLine($"Turn Attacker: {attacker.PlayerName} | Hand Size: {attacker.HandSize}");
            //Console.WriteLine($"Turn Defender: {defender.PlayerName} | Hand Size: {defender.HandSize}");

            //// Output Trump Card and Suit in DarkCyan
            //Console.ForegroundColor = ConsoleColor.DarkCyan;
            //Console.WriteLine($"TRUMP CARD: {_trumpCard} | TRUMP SUIT: {_trumpCard.Suit}");

            //// Output River Card Count and Cards in DarkBlue
            //Console.ForegroundColor = ConsoleColor.DarkBlue;
            //Console.WriteLine($"# of Cards in River: {_river.Count}");

            //DisplayCards(_river);

            //Console.ResetColor();

            //Console.WriteLine(_river.Any() ? $"Last Card Played: {_river.Last()}" : "No Cards in the River!");
            //Console.WriteLine("=========================================\n");

            #endregion
        }

        #endregion

        private void btnPass_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDealNew_Click(object sender, RoutedEventArgs e)
        {            
            GameView gameView = new GameView();
            DataContext = gameView;
        }
    }
}
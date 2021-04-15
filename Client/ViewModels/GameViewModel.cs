using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CardLib;
using CardUI;
using DurakLib;

// TODO: Configure for up to 6 players.
// TODO: Maybe not use loops?
namespace Client.ViewModels
{
    /// <summary>
    /// Provides fields and a method to play a two-player game of Durak between a human and an AI.
    /// </summary>
    public class GameViewModel : BaseViewModel
    {
        #region FIELDS

        /// <summary>
        /// A deck of <see cref="Card"/> representing the Play Deck.
        /// </summary>
        public Deck<PlayingCard> PlayDeck { get; set; }

        /// <summary>
        /// A <see cref="Cards"/> collection representing the cards in play during a bout.
        /// </summary>
        private Cards _river;

        /// <summary>
        /// An array of <see cref="Player"/> containing the players of the game.
        /// </summary>
        public Player[] Players { get; set; }

        /// <summary>
        /// The Human Player in this game.
        /// </summary>
        public Player HumanPlayer { get; set; }

        /// <summary>
        /// The AI player in this game.
        /// </summary>
        public DurakAI AiPlayer { get; set; }

        /// <summary>
        /// An <see cref="ObservableCollection{T}"/> of <see cref="PlayingCard"/> representing the <see cref="PlayingCard"/> in the Human's hand.
        /// </summary>
        public ObservableCollection<PlayingCard> HumanCards { get; set; } = new ObservableCollection<PlayingCard>();

        /// <summary>
        /// An <see cref="ObservableCollection{T}"/> of <see cref="PlayingCard"/> representing the <see cref="PlayingCard"/> in the AI's hand.
        /// </summary>
        public ObservableCollection<PlayingCard> AiCards { get; set; } = new ObservableCollection<PlayingCard>();

        /// <summary>
        /// An <see cref="ObservableCollection{T}"/> of <see cref="PlayingCard"/> representing the <see cref="PlayingCard"/> in the play area (River).
        /// </summary>
        public ObservableCollection<PlayingCard> River { get; set; } = new ObservableCollection<PlayingCard>();

        /// <summary>
        /// The Attacker in this current turn.
        /// </summary>
        public Player Attacker { get; set; }

        /// <summary>
        /// The Defender in this current turn.
        /// </summary>
        public Player Defender { get; set; }

        /// <summary>
        /// The player who is currently making a play in the bout.
        /// </summary>
        public Player CurrentPlayer { get; set; }

        /// <summary>
        /// The size of the <see cref="Deck{T}"/> used in the game.
        /// </summary>
        public int CurrentDeckSize => PlayDeck.Size;

        /// <summary>
        /// Represents the amount of turns that have elapsed (a turn ends when an attack ends)
        /// </summary>
        public int TurnCount { get; set; } = 0;

        /// <summary>
        /// Represents the amount of bouts during the current turn (a bout ends after both players move)
        /// </summary>
        public int BoutCount { get; set; } = 0;

        /// <summary>
        /// The trump card that was chosen.
        /// </summary>
        public PlayingCard TrumpCard { get; set; }

        private const int _drawAmount = 6;

        /// <summary>
        /// Represents whether the last attack in a turn was a success.
        /// </summary>
        private bool AttackSuccess { get; set; } = true;

        /// <summary>
        /// Represents whether the last defense in a turn was a success.
        /// </summary>
        private bool DefenseSuccess { get; set; } = true;

        /// <summary>
        /// Whether the game has a winner or not.
        /// </summary>
        public bool HasWinner => Winner != null;

        /// <summary>
        /// The first player to get
        /// </summary>
        public Player Winner { get; set; }

        /// <summary>
        /// A <see cref="TaskCompletionSource{TResult}"/> which tells the game when the player has chose.
        /// <para>
        /// Try to set the result of this object when you have confirmed a player's choice using <see cref="TaskCompletionSource{TResult}.TrySetResult"/>
        /// </para>
        /// </summary>
        public TaskCompletionSource<bool> PlayerChoseCompletionSource { get; set; } = null;

        /// <summary>
        /// Represents whether a game is in progress or not.
        /// </summary>
        public static bool GameInProgress = false;

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Initializes and sets up a new two-player game of Durak using the specified <see cref="DeckSize"/>, <see cref="HumanPlayer"/> name and
        /// <see cref="AiPlayer"/> name, if any.
        ///
        /// <para>
        /// The game does not start unless <see cref="GameViewModel.PlayGame"/> is invoked.
        /// </para>
        /// </summary>
        /// <param name="gameDeckSize">The size of the game deck to use.</param>
        /// <param name="humanName">The name of the human player.</param>
        /// <param name="aiName">The name of the AI player.</param>
        public GameViewModel(DeckSize gameDeckSize = DeckSize.ThirtySix, string humanName = "Human", string aiName = "Player 2 (AI)")
        {
            ResetGame(gameDeckSize, humanName, aiName);
        }

        #endregion

        #region EVENTS

        /// <summary>
        /// Invoked when a Winner of the game is found.
        /// </summary>
        public EventHandler WinnerFound;

        /// <summary>
        /// Invoked when the turn changed (after a successful defense or attack).
        /// </summary>
        public EventHandler TurnChange;

        /// <summary>
        /// Invoked when the bout changed (after both players have played).
        /// </summary>
        public EventHandler BoutChange;

        /// <summary>
        /// Invoked when a successful attack occurs.
        /// </summary>
        public EventHandler AttackSuccessful;

        /// <summary>
        /// Invoked when a successful defense occurs.
        /// </summary>
        public EventHandler DefenseSuccessful;

        #endregion

        #region METHODS

        // Not sure how this would interact when invoked while PlayGame is executing.
        /// <summary>
        /// Resets the game, creating a completely new game of Durak.
        /// </summary>
        /// <param name="gameDeckSize"></param>
        /// <param name="humanName"></param>
        /// <param name="aiName"></param>
        private void ResetGame(DeckSize gameDeckSize = DeckSize.ThirtySix, string humanName = "Human", string aiName = "Player 2 (AI)")
        {
            PlayDeck = new Deck<PlayingCard>(true, true, size: gameDeckSize);
            _river = new Cards();

            PlayDeck.Shuffle();

            var humanCards = PlayDeck.Draw(_drawAmount);
            AddHandToObservableCollection(HumanCards, humanCards);

            var aiCards = PlayDeck.Draw(_drawAmount);
            aiCards.ForEach(card => card.Face = Face.Down);
            AddHandToObservableCollection(AiCards, aiCards);

            HumanPlayer = new Player(humanName, humanCards);
            AiPlayer = new DurakAI(aiName, aiCards);
            Players = new[] { HumanPlayer, AiPlayer };

            TrumpCard = PlayDeck.Draw();
            CardBase.Trump = TrumpCard.Suit;

            var flip = new Random();

            if (flip.Next(2) == 0)
            {
                AiPlayer.IsAttacking = true;
                HumanPlayer.IsAttacking = false;
            }
            else
            {
                HumanPlayer.IsAttacking = true;
                AiPlayer.IsAttacking = false;
            }

            Winner = null;

            GameInProgress = false;
        }

        /// <summary>
        /// Plays the Game of Durak, terminating when a winner is found. Multiple events are invoked during gameplay.
        /// <para>
        /// The game waits for player input when it is a human's turn to attack or defend. Use the <see cref="PlayerChoseCompletionSource"/>
        /// to notify the game when the player has made their move.
        /// </para>
        /// </summary>
        public async void PlayGame()
        {
            if (GameInProgress) return;

            GameInProgress = true;

            // Game Loop
            do
            {
                foreach (var player in Players)
                {
                    if (player.HandSize != 0) continue;
                    Winner = player;
                    OnPropertyChanged(nameof(HasWinner));
                    WinnerFound?.Invoke(this, EventArgs.Empty);
                    return;
                }

                TurnCount++;

                Attacker = Players.First(player => player.IsAttacking);
                Defender = Players.First(player => !player.IsAttacking);

                CurrentPlayer = Attacker;

                BoutCount = 1;

                RefillHands(_drawAmount);

                AttackSuccess = true;
                DefenseSuccess = true;

                TurnChange?.Invoke(this, EventArgs.Empty);

                // Turn Loop
                do
                {
                    Attacker.TakeTurn(_river);

                    if (Attacker == HumanPlayer)
                    {
                        PlayerChoseCompletionSource = new TaskCompletionSource<bool>();
                        await PlayerChoseCompletionSource.Task;
                    }

                    if (Attacker.HasChosen)
                    {
                        Attacker.ChosenCard.Face = Face.Up;
                        _river.Add(Attacker.ChosenCard);
                        AddCardToObservableCollection(River, (PlayingCard)Attacker.ChosenCard);

                        if (HumanPlayer == Attacker) HumanCards.Remove((PlayingCard)Attacker.ChosenCard);
                        if (AiPlayer == Attacker) AiCards.Remove((PlayingCard)Attacker.ChosenCard);
                        if (Attacker.HandSize == 0) break;
                    }
                    else
                    {
                        _river.Clear();
                        River.Clear();
                        AttackSuccess = false;
                        continue;
                    }

                    CurrentPlayer = Defender;

                    Defender.TakeTurn(_river);

                    if (Defender == HumanPlayer)
                    {
                        PlayerChoseCompletionSource = new TaskCompletionSource<bool>();
                        await PlayerChoseCompletionSource.Task;
                        //MessageBox.Show($"You have Chosen: {(!(Defender.ChosenCard is null) ? Defender.ChosenCard.ToString() : "You did not choose a card")}");
                    }

                    if (Defender.HasChosen)
                    {
                        Defender.ChosenCard.Face = Face.Up;
                        _river.Add(Defender.ChosenCard);
                        AddCardToObservableCollection(River, (PlayingCard)Defender.ChosenCard);
                        if (HumanPlayer == Defender) HumanCards.Remove((PlayingCard)Defender.ChosenCard);
                        if (AiPlayer == Defender) AiCards.Remove((PlayingCard)Defender.ChosenCard);
                        if (Defender.HandSize == 0) break;
                    }
                    else
                    {
                        Defender.Hand.AddRange(_river);
                        if (HumanPlayer == Defender) AddHandToObservableCollection(HumanCards, _river);
                        if (AiPlayer == Defender)
                        {
                            _river.ForEach(card => card.Face = Face.Down);
                            AddHandToObservableCollection(AiCards, _river);
                        }
                        _river.Clear();
                        River.Clear();
                        DefenseSuccess = false;
                    }

                    BoutCount++;

                    BoutChange?.Invoke(this, EventArgs.Empty);

                    CurrentPlayer = Attacker;
                } while (AttackSuccess && DefenseSuccess);  // End of Turn Loop

                if (DefenseSuccess)
                {
                    DefenseSuccessful?.Invoke(this, EventArgs.Empty);

                    foreach (var player in Players)
                    {
                        player.IsAttacking = !player.IsAttacking;
                    }
                }
                else AttackSuccessful?.Invoke(this, EventArgs.Empty);
            } while (true); // End of Game Loop
        }
        #endregion

        #region HELPER METHODS

        /// <summary>
        /// Sets the number of cards in hands of the <see cref="Player"/> instances in <see cref="_players"/>, alternating until target amount
        /// is met or the <see cref="PlayDeck"/> is empty.
        /// </summary>
        /// <param name="refillTo">The amount to refill the hands to.</param>
        private void RefillHands(int refillTo)
        {
            if (!PlayDeck.CanDraw) return;

            var refilledCount = 0;
            do
            {
                foreach (var player in Players)
                {
                    if (player.HandSize >= refillTo)
                    {
                        refilledCount++;
                        continue;
                    }

                    if (!PlayDeck.CanDraw) continue;
                    player.Hand.Add(PlayDeck.Draw());

                    if (player.HandSize == refillTo) refilledCount++;
                }
            } while (PlayDeck.CanDraw && refilledCount < Players.Length);

            OnPropertyChanged(nameof(CurrentDeckSize));
            AiCards.Clear();
            HumanCards.Clear();
            AiPlayer.Hand.ForEach(card => card.Face = Face.Down);
            AddHandToObservableCollection(AiCards, AiPlayer.Hand);
            AddHandToObservableCollection(HumanCards, HumanPlayer.Hand);
        }

        /// <summary>
        /// Adds a <see cref="Cards"/> collection to an <see cref="ObservableCollection{T}"/> as <see cref="PlayingCard"/>.
        /// </summary>
        /// <param name="collection">The observable collection to add cards to.</param>
        /// <param name="hand">The cards to add to the collection.</param>
        private static void AddHandToObservableCollection(ObservableCollection<PlayingCard> collection, Cards hand)
        {
            foreach (var card in hand.Cast<PlayingCard>())
            {
                //card.Face = Face.Up;
                card.UpdateCardImage();
                collection.Add(card);
            }
        }

        /// <summary>
        /// Adds a <see cref="PlayingCard"/> to an <see cref="ObservableCollection{T}"/> of <see cref="PlayingCard"/>.
        /// </summary>
        /// <param name="collection">The collection to add the card to.</param>
        /// <param name="card">The card to add to the collection.</param>
        private static void AddCardToObservableCollection(ObservableCollection<PlayingCard> collection, PlayingCard card)
        {
            card.UpdateCardImage();
            collection.Add(card);
        }

        #endregion
    }
}

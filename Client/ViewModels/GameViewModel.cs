using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CardLib;
using CardUI;
using DurakLib;

namespace Client.ViewModels
{
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
        private readonly Cards _river;

        /// <summary>
        /// An array of <see cref="Player"/> containing the players of the game.
        /// </summary>
        public Player[] Players { get; set; }

        public Player HumanPlayer { get; set; }

        public DurakAI AiPlayer { get; set; }

        public ObservableCollection<PlayingCard> HumanCards { get; set; } = new ObservableCollection<PlayingCard>();

        public ObservableCollection<PlayingCard> AiCards { get; set; } = new ObservableCollection<PlayingCard>();

        public ObservableCollection<PlayingCard> River { get; set; } = new ObservableCollection<PlayingCard>();

        public Player Attacker { get; set; }

        public Player Defender { get; set; }

        public Player CurrentPlayer { get; set; }

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

        public bool AttackSuccess { get; set; } = true;

        public bool DefenseSuccess { get; set; } = true;

        /// <summary>
        /// Whether the game has a winner or not.
        /// </summary>
        public bool HasWinner => Winner != null;

        public Player Winner { get; set; }

        public TaskCompletionSource<bool> PlayerChoseCompletionSource { get; set; } = null;

        #endregion

        #region Constructors

        public GameViewModel(DeckSize gameDeckSize = DeckSize.ThirtySix, string humanName = "Human", string aiName = "Player 2 (AI)")
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

        }

        #endregion

        #region EVENTS

        public EventHandler WinnerFound;
        public EventHandler TurnChange;
        public EventHandler BoutChange;
        public EventHandler AttackSuccessful;
        public EventHandler DefenseSuccessful;

        #endregion

        #region METHODS

        public async void PlayGame()
        {
            // Game Loop
            do
            {
                foreach (var player in Players)
                {
                    if (player.HandSize != 0) continue;
                    Winner = player;
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

        private void AddHandToObservableCollection(ObservableCollection<PlayingCard> collection, Cards hand)
        {
            foreach (var card in hand.Cast<PlayingCard>())
            {
                //card.Face = Face.Up;
                card.UpdateCardImage();
                collection.Add(card);
            }
        }

        private void AddCardToObservableCollection(ObservableCollection<PlayingCard> collection, PlayingCard card)
        {
            card.UpdateCardImage();
            collection.Add(card);
        }

        #endregion
    }
}

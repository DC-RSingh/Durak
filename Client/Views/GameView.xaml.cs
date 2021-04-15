using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CardLib;
using CardUI;
using Client.ViewModels;
using DurakLib;
using Logging;

namespace Client.Views
{
    // TODO: Have a Visual Change for Cards that cannot be played
    /// <summary>
    /// Interaction logic for GameView.xaml, presenting a two player game of Durak.
    /// </summary>
    public partial class GameView : UserControl
    {
        /// <summary>
        /// Initializes an instance of a <see cref="GameView"/> with the specified <see cref="DeckSize"/> and <paramref name="playerName"/> if any is provided.
        /// <para>
        /// Starts a two player game of Durak.
        /// </para>
        /// </summary>
        /// <param name="deckSize">The size of the deck to use in the game of Durak.</param>
        /// <param name="playerName">The name of the human player.</param>
        public GameView(DeckSize deckSize = DeckSize.ThirtySix, string playerName = "Player 1 (Human)")
        {
            InitializeComponent();
            _chosenDeckSize = deckSize;
            _username = playerName;
            InitGame();
        }

        private GameViewModel _gameViewModel;
        private readonly string _username;
        private readonly DeckSize _chosenDeckSize;

        #region FIELDS

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

        #region EVENT HANDLERS

        /// <summary>
        /// Occurs when the hands are first dealt. Logs contents using the <see cref="Logger"/> class.
        /// </summary>
        /// <param name="sender">A <see cref="GameViewModel"/></param>
        /// <param name="e">The event arguments.</param>
        private void Players_HandsDealt(object sender, EventArgs e)
        {
            if (!(sender is GameViewModel gvm)) return;

            foreach (var player in gvm.Players)
            {
                if(player.GetType() == typeof(DurakAI)) player.Hand.ForEach(card => card.Flip());

                Logger.Log($"{player.PlayerName}'s Cards: {player.Hand}", LoggingLevel.Log, player.GetType());

                if (player.GetType() == typeof(DurakAI)) player.Hand.ForEach(card => card.Flip());
            }

            Logger.Log($"Trump Card: {gvm.TrumpCard}", source: typeof(CardBase));
        }

        /// <summary>
        /// Occurs when hands are refilled. Logs contents using the <see cref="Logger"/> class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayerHands_Refilled(object sender, EventArgs e)
        {
            if (!(sender is GameViewModel gvm)) return;
            foreach (var player in gvm.Players)
            {
                if (player.GetType() == typeof(DurakAI)) player.Hand.ForEach(card => card.Flip());

                Logger.Log($"{player.PlayerName}'s Cards Refilled! New Hand: {player.Hand}", LoggingLevel.Log, player.GetType());

                if (player.GetType() == typeof(DurakAI)) player.Hand.ForEach(card => card.Flip());
            }
        }

        /// <summary>
        /// Occurs when the human chooses a card. Logs contents using the <see cref="Logger"/> class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Human_ChoseCard(object sender, EventArgs e)
        {
            if (!(sender is Player player)) return;

            Logger.Log($"{player.PlayerName} chose to play {player.ChosenCard} on Turn {_gameViewModel.TurnCount}, Bout {_gameViewModel.BoutCount}", source: typeof(Player));
        }

        /// <summary>
        /// An EventHandler that executes when <see cref="DurakAI"/> "stops thinking".
        /// </summary>
        /// <remarks>EventArgs passed is always <seealso cref="EventArgs.Empty"/>.</remarks>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The EventArgs of the event.</param>
        private void AI_StopThink(object sender, EventArgs e)
        {
            if (sender is DurakAI ai)
            {
                Logger.Log(ai.CanPlay
                    ? $"{ai.PlayerName} found a Move on Turn {_gameViewModel.TurnCount}, Bout {_gameViewModel.BoutCount}! Played {ai.ChosenCard}."
                    : $"{ai.PlayerName} could not Find a Move on Turn {_gameViewModel.TurnCount}, Bout {_gameViewModel.BoutCount}! Passing...", source: typeof(DurakAI));
            }

        }

        /// <summary>
        /// Occurs when a successful attack happens in the game of Durak.
        /// </summary>
        /// <param name="sender">A <see cref="GameViewModel"/></param>
        /// <param name="e">The event args.</param>
        private void Durak_AttackSuccess(object sender, EventArgs e)
        {
            if (!(sender is GameViewModel gvm)) return;

            var msg = $"{gvm.Attacker.PlayerName} was successful in their attack! Defender took up river cards!";

            //MessageBox.Show(msg, "Attack Success!", MessageBoxButton.OK, MessageBoxImage.Information);
            Logger.Log(msg, source: gvm.Attacker.GetType());

            if (gvm.Defender is DurakAI) gvm.Defender.Hand.ForEach(card => card.Flip());

            Logger.Log($"{gvm.Defender.PlayerName}'s Hand: {gvm.Defender.Hand}", source: gvm.Defender.GetType());

            if (gvm.Defender is DurakAI) gvm.Defender.Hand.ForEach(card => card.Flip());
        }

        /// <summary>
        /// Occurs when a successful defense happens in the game of Durak.
        /// </summary>
        /// <param name="sender">A <see cref="GameViewModel"/></param>
        /// <param name="e">The event args.</param>
        private void Durak_DefenseSuccess(object sender, EventArgs e)
        {
            if (!(sender is GameViewModel gvm)) return;

            var msg = $"{gvm.Defender.PlayerName} has successfully defended. Attackers have been switched!";

            //MessageBox.Show(msg, "Defense Success!", MessageBoxButton.OK, MessageBoxImage.Information);
            Logger.Log(msg, source: gvm.Defender.GetType());
        }

        /// <summary>
        /// Updates the AI Hand canvas when the AI Cards collection changes.
        /// </summary>
        /// <param name="sender">A <see cref="ObservableCollection{PlayingCard}"/> of <see cref="PlayingCard"/></param>
        /// <param name="e">The event arguments.</param>
        private void AiHand_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is ObservableCollection<PlayingCard> collection)
            {
                pnlAIHand.Children.Clear();
                foreach (var card in collection)
                {
                    pnlAIHand.Children.Add(card.CardImage);
                }
            }
            Animate.RealignCards(pnlAIHand);
            
        }

        /// <summary>
        /// Updates the Human Hand canvas when the Human Cards collection changes.
        /// </summary>
        /// <param name="sender">A <see cref="ObservableCollection{PlayingCard}"/> of <see cref="PlayingCard"/></param>
        /// <param name="e">The event arguments.</param>
        private void HumanHand_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is ObservableCollection<PlayingCard> collection)
            {
                pnlPlayerHand.Children.Clear();
                foreach (var card in collection)
                {
                    WireMouseEvents(card.CardImage, pnlPlayerHand);
                    pnlPlayerHand.Children.Add(card.CardImage);
                }
            }

            Animate.RealignCards(pnlPlayerHand);
        }

        /// <summary>
        /// Updates the River canvas when the River Cards collection changes.
        /// </summary>
        /// <param name="sender">A <see cref="ObservableCollection{PlayingCard}"/> of <see cref="PlayingCard"/></param>
        /// <param name="e">The event arguments.</param>
        private void River_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is ObservableCollection<PlayingCard> river)
            {
                pnlRiver.Children.Clear();
                foreach (var card in river)
                {
                    pnlRiver.Children.Add(card.CardImage);
                }
            }

            Animate.RealignCards(pnlRiver);
        }

        /// <summary>
        /// Wires Mouse Events to the images of the <see cref="PlayingCard"/> used in the player's hand.
        /// </summary>
        /// <param name="img">The image to add events to.</param>
        /// <param name="canvas">The canvas the image is in.</param>
        private void WireMouseEvents(Image img, Canvas canvas)
        {

            img.MouseEnter -= PlayingCard_MouseEnter;
            img.MouseEnter += PlayingCard_MouseEnter;

            img.MouseLeave -= PlayingCard_MouseLeave;
            img.MouseLeave += PlayingCard_MouseLeave;

            //foreach (var card in _gameViewModel.HumanPlayer.PlayableCards)
            //{
            //    var playingCard = (PlayingCard) card;

            //    // Wire Visual Effect Events
            //    img.MouseEnter -= PlayingCard_MouseEnter;
            //    img.MouseLeave -= PlayingCard_MouseLeave;
            //    img.MouseLeftButtonDown -= PlayingCard_LeftMouseButtonDown;

            //    if (img.Source != playingCard.CardImage.Source) continue;

            //    img.MouseEnter += PlayingCard_MouseEnter;
            //    img.MouseLeave += PlayingCard_MouseLeave;
            //    img.MouseLeftButtonDown += PlayingCard_LeftMouseButtonDown;

            //}

            //img.MouseLeftButtonDown -= PlayingCard_LeftMouseButtonDown;
            //img.MouseLeftButtonDown += PlayingCard_LeftMouseButtonDown;

        }

        /// <summary>
        /// Enlarges a card image when it is hovered for visual effect.
        /// </summary>
        /// <param name="sender">An <see cref="Image"/>.</param>
        /// <param name="e">The event arguments.</param>
        private void PlayingCard_MouseEnter(object sender, MouseEventArgs e)
        {
            // Convert sender to a Image
            if (!(sender is Image img)) return;

            // TODO: Extreme Optimize this
            foreach (var card in _gameViewModel.HumanPlayer.Hand)
            {
                var playingCard = (PlayingCard) card;

                // Remove Left Mouse Button Down event
                img.MouseLeftButtonDown -= PlayingCard_LeftMouseButtonDown;

                if (!_gameViewModel.HumanPlayer.PlayableCards.Contains(playingCard) ||
                    img.Source != playingCard.CardImage.Source) continue;
                // Enlarge the card for visual effect
                img.Height += POP;
                img.Width += POP;

                // move the card to the top edge of the panel.
                Canvas.SetTop(img, 0);

                img.MouseLeftButtonDown += PlayingCard_LeftMouseButtonDown;
                break;
            }
        }

        /// <summary>
        /// Sets the card back to its original height and width when the mouse leaves.
        /// </summary>
        /// <param name="sender">Ann <see cref="Image"/>.</param>
        /// <param name="e">The event arguments.</param>
        private void PlayingCard_MouseLeave(object sender, MouseEventArgs e)
        {
            // Set card back to regular height and width
            if (!(sender is Image img)) return;
            img.Height = regularHeight;
            img.Width = regularWidth;

            // move the card to the top edge of the panel.
            Canvas.SetTop(img, POP);
        }

        /// <summary>
        /// Plays a card when the image is clicked if that card is playable.
        /// </summary>
        /// <param name="sender">An <see cref="Image"/> associated with a <see cref="PlayingCard"/></param>
        /// <param name="args">The event arguments.</param>
        private void PlayingCard_LeftMouseButtonDown(object sender, MouseButtonEventArgs args)
        {
            // Convert sender to a Image
            if (sender is Image cardImage)
            {
                //try
                //{
                _gameViewModel.HumanPlayer.ChooseCard(_gameViewModel.HumanPlayer.PlayableCards.First(card =>
                    (card as PlayingCard)?.CardImage.Source == cardImage.Source));
                _gameViewModel.PlayerChoseCompletionSource.TrySetResult(true);
                //}
                //catch (InvalidOperationException)
                //{
                // Silently handle invalid OP for if event fires more than once
                //}
            }
        }

        /// <summary>
        /// Displays a loser message when the game found a winner.
        /// </summary>
        /// <param name="sender">A <see cref="GameViewModel"/></param>
        /// <param name="e">The event arguments.</param>
        private void Player_Won(object sender, EventArgs e)
        {
            var msg =
                $"Game Over! {_gameViewModel.Players.First(player => player != _gameViewModel.Winner).PlayerName} is the Fool!";
            MessageBox.Show(msg, "Game Over!", MessageBoxButton.OK, MessageBoxImage.Information);
            Logger.Log(msg, source: typeof(GameViewModel));

            //Add to Win or Loss Stats
            if (_gameViewModel.Winner == _gameViewModel.HumanPlayer)
            {
                Statistics.UpdateWins();
            }
            else
            {
                Statistics.UpdateLosses();
            }
        }

        /// <summary>
        /// Passes the human player's current turn.
        /// </summary>
        /// <param name="sender">A <see cref="GameView"/></param>
        /// <param name="e">The event arguments.</param>
        private void btnPass_Click(object sender, RoutedEventArgs e)
        {
            if (_gameViewModel.BoutCount == 1 && _gameViewModel.HumanPlayer == _gameViewModel.Attacker)
            {
                MessageBox.Show("You must choose a card to play on the first bout as the attacker!");
            }
            else
            {
                Logger.Log(
                    $"{_gameViewModel.HumanPlayer.PlayerName} has chosen not to play on Turn {_gameViewModel.TurnCount}, Bout {_gameViewModel.BoutCount}.",
                    source: typeof(Player));
                _gameViewModel.PlayerChoseCompletionSource.TrySetResult(true);
            }
        }

        /// <summary>
        /// Prompts the user before they start a new game.
        /// </summary>
        /// <param name="sender">A <see cref="GameView"/>.</param>
        /// <param name="e">The event arguments.</param>
        private void btnDealNew_Click(object sender, RoutedEventArgs e)
        {
            var msgBoxResult = MessageBox.Show("Are you sure you want to start a new game?", "Start New Game?", MessageBoxButton.YesNo);

            if (msgBoxResult == MessageBoxResult.Yes) InitGame();
        }

        #endregion

        #region HELPER METHODS

        /// <summary>
        /// Disables an image in the playing field 
        /// </summary>
        /// <param name="img"></param>
        private void DisableImage(Image img)
        {
            img.Height = regularHeight;
            img.Width = regularWidth;
            img.Opacity = 0.7;

            //Animate.RealignCards(pnlPlayerHand);
        }

        

        /// <summary>
        /// Initializes a game of two player Durak with the <see cref="GameViewModel.PlayGame"/> method.
        ///
        /// <para>
        /// Initializes all canvases and wires events.
        /// </para>
        /// </summary>
        private void InitGame()
        {
            Logger.Log($"{Statistics.PlayerName} has started a new game...", LoggingLevel.Log, typeof(GameView));
            var vm = new GameViewModel(_chosenDeckSize, _username);
            this.DataContext = vm;
            _gameViewModel = vm;

            _gameViewModel.AiPlayer.ThinkDelay = 200;

            pnlAIHand.Children.Clear();
            pnlPlayerHand.Children.Clear();
            pnlRiver.Children.Clear();

            pnlDeck.Children.Clear();

            pnlDeck.Children.Add(_gameViewModel.TrumpCard.UpdateCardImage());

            // Align and Initialize AI and Human Hand Canvases
            foreach (var card in _gameViewModel.AiCards)
            {
                pnlAIHand.Children.Add(card.CardImage);
            }

            Animate.RealignCards(pnlAIHand);

            foreach (var card in _gameViewModel.HumanCards)
            {
                WireMouseEvents(card.CardImage, pnlPlayerHand);
                pnlPlayerHand.Children.Add(card.CardImage);
            }

            Animate.RealignCards(pnlPlayerHand);

            // Wire Play Deck Events
            _gameViewModel.HandsRefilled += PlayerHands_Refilled;

            // Wire Hand Dealt Event
            _gameViewModel.HandsDealt += Players_HandsDealt;

            // Wire Ai Events
            _gameViewModel.AiPlayer.StoppedThinking += AI_StopThink;

            // Wire Human Events
            _gameViewModel.HumanPlayer.ChoseCard += Human_ChoseCard;

            // Wire Phasing Events
            _gameViewModel.AttackSuccessful += Durak_AttackSuccess;
            _gameViewModel.DefenseSuccessful += Durak_DefenseSuccess;

            // Wiring Collection Events
            _gameViewModel.AiCards.CollectionChanged += AiHand_CollectionChanged;
            _gameViewModel.HumanCards.CollectionChanged += HumanHand_CollectionChanged;
            _gameViewModel.River.CollectionChanged += River_CollectionChanged;

            // Wiring Winner Event
            _gameViewModel.WinnerFound += Player_Won;

            _gameViewModel.PlayGame();
        }

        #endregion

    }
}
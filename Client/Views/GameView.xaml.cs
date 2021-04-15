using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

        private void Players_HandsDealt(object sender, EventArgs e)
        {
            if (!(sender is GameViewModel gvm)) return;

            foreach (var player in gvm.Players)
            {
                if(player.GetType() == typeof(DurakAI)) player.Hand.ForEach(card => card.Flip());

                Logger.Log($"{player.PlayerName}'s Cards: {player.Hand}", LoggingLevel.Log, player.GetType());

                if (player.GetType() == typeof(DurakAI)) player.Hand.ForEach(card => card.Flip());
            }
        }

        private void PlayDeck_Draw(object sender, EventArgs e)
        {
            if (!(sender is Deck<PlayingCard> playDeck)) return;

            
        }

        /// <summary>
        /// An EventHandler that handles most of the game logic for the Human <see cref="Player"/> turn in Durak, outputting to and requesting input from the Console.
        /// </summary>
        /// <remarks>EventArgs passed is always <seealso cref="EventArgs.Empty"/>.</remarks>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The EventArgs of the event</param>
        private void HumanPlayer_TurnStart(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// An EventHandler that executes at the end of a Human <see cref="Player"/>'s Turn.
        /// </summary>
        /// <remarks>EventArgs passed is always <seealso cref="EventArgs.Empty"/>.</remarks>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The EventArgs of the event</param>
        private void HumanPlayer_TurnEnd(object sender, EventArgs e)
        {
        }

        //TODO: Removed static 
        /// <summary>
        /// An EventHandler that executes when <see cref="DurakAI"/> "starts thinking".
        /// </summary>
        /// <remarks>EventArgs passed is always <seealso cref="EventArgs.Empty"/>.</remarks>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The EventArgs of the event.</param>
        private void AI_StartThink(object sender, EventArgs e)
        {
            if (sender is DurakAI ai)
            {
                //txtAIThink.Text = "Thinking...";
            }
        }

        /// <summary>
        /// An EventHandler that executes when <see cref="DurakAI"/> "stops thinking".
        /// </summary>
        /// <remarks>EventArgs passed is always <seealso cref="EventArgs.Empty"/>.</remarks>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The EventArgs of the event.</param>
        private void AI_StopThink(object sender, EventArgs e)
        {
            //if (!(sender is DurakAI ai)) return;
            ////MessageBox.Show(ai.CanPlay ? $"{ai.PlayerName} found a Move! Playing {ai.ChosenCard}.\n" : $"{ai.PlayerName} could not Find a Move! Passing...\n");

            //if (!ai.CanPlay) return;
            //ai.ChosenCard.Face = Face.Up;
            //_river.Add(ai.ChosenCard);
            //var card = (PlayingCard)ai.ChosenCard;
            //pnlRiver.Children.Add(card.CardImage);
            //pnlAIHand.Children.Clear();
            //foreach (var handCard in ai.Hand)
            //{
            //    var playingCard = (PlayingCard)handCard;
            //    pnlAIHand.Children.Add(playingCard.UpdateCardImage());
            //}

            ////Animate.RealignCards(pnlAIHand);
            //Animate.RealignCards(pnlRiver);

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

            foreach (var card in _gameViewModel.HumanPlayer.PlayableCards)
            {
                var playingCard = (PlayingCard) card;

                // Remove Left Mouse Button Down event
                img.MouseLeftButtonDown -= PlayingCard_LeftMouseButtonDown;

                if (img.Source != playingCard.CardImage.Source) continue;

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
        private void PlayingCard_LeftMouseButtonDown (object sender, MouseButtonEventArgs args) 
        {
            // Convert sender to a Image
            if (sender is Image cardImage)
            {
                //// Remove the card from the home panel
                //foreach (var card in _gameViewModel.HumanPlayer.PlayableCards)
                //{
                //    var playingCard = (PlayingCard)card;

                //    // Wire Visual Effect Events
                //    cardImage.MouseEnter -= PlayingCard_MouseEnter;
                //    cardImage.MouseLeave -= PlayingCard_MouseLeave;

                //if (cardImage.Source == playingCard.CardImage.Source)
                //{
                //cardImage.MouseEnter += PlayingCard_MouseEnter;
                //cardImage.MouseLeave += PlayingCard_MouseLeave;
                _gameViewModel.HumanPlayer.ChooseCard(_gameViewModel.HumanPlayer.PlayableCards.First(card => (card as PlayingCard)?.CardImage.Source == cardImage.Source));
                _gameViewModel.PlayerChoseCompletionSource.TrySetResult(true); 
                //break;
                //}

                //}

                //MessageBox.Show($"You have Chosen: {(!(_gameViewModel.Attacker.ChosenCard is null) ? _gameViewModel.Attacker.ChosenCard.ToString() : "You did not choose a card")}");
            }
        }

        /// <summary>
        /// Displays a loser message when the game found a winner.
        /// </summary>
        /// <param name="sender">A <see cref="GameViewModel"/></param>
        /// <param name="e">The event arguments.</param>
        private void Player_Won(object sender, EventArgs e)
        {
            MessageBox.Show($"{_gameViewModel.Players.First(player => player != _gameViewModel.Winner).PlayerName} is the Fool!");

            //Add to Win Stats
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
                //txtPass.Text = "You Have Chosen Not to Play.";
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
            Logger.Log($"{Statistics.PlayerName} has started a new game.", LoggingLevel.Log, typeof(GameView));
            var vm = new GameViewModel(_chosenDeckSize, _username);
            this.DataContext = vm;
            _gameViewModel = vm;

            _gameViewModel.AiPlayer.ThinkDelay = 0;

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

            Statistics.UpdateGame();

            _gameViewModel.HandsDealt += Players_HandsDealt;

            // Wiring Player Turn Events
            _gameViewModel.HumanPlayer.TurnBegin += HumanPlayer_TurnStart;
            _gameViewModel.AiPlayer.StartedThinking += AI_StartThink;

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
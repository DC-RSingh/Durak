using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CardLib;
using CardUI;
using Client.ViewModels;
using DurakLib;

namespace Client.Views
{
    // TODO: Have a Visual Change for Cards that cannot be played
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : UserControl
    {
        public GameView(DeckSize deckSize = DeckSize.ThirtySix, string playerName = "Player 1 (Human)")
        {
            InitializeComponent();
            ChosenDeckSize = deckSize;
            Username = playerName;
            InitGame();
        }

        private GameViewModel _gameViewModel;
        private string Username;
        private DeckSize ChosenDeckSize;

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
        private static void HumanPlayer_TurnEnd(object sender, EventArgs e)
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

        #endregion

        #region HELPER METHODS

        private void WireMouseEvents(Image img, Canvas canvas)
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
                        img.Height = 132;
                        img.Width = 100;

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
            
            img.MouseLeftButtonDown -= PlayingCard_LeftMouseButtonDown;
            img.MouseLeftButtonDown += PlayingCard_LeftMouseButtonDown;

        }   

        private void PlayingCard_LeftMouseButtonDown (object sender, MouseButtonEventArgs args) 
        {
            // Convert sender to a Image
            if (sender is Image cardImage)
            {
                // Remove the card from the home panel
                foreach (var card in _gameViewModel.HumanPlayer.PlayableCards)
                {
                    var playingCard = (PlayingCard)card;
                    if (cardImage.Source == playingCard.CardImage.Source) 
                    {
                        _gameViewModel.HumanPlayer.ChooseCard(card);
                        _gameViewModel.PlayerChoseCompletionSource.TrySetResult(true);
                        break;
                    }
                }

                //MessageBox.Show($"You have Chosen: {(!(_gameViewModel.Attacker.ChosenCard is null) ? _gameViewModel.Attacker.ChosenCard.ToString() : "You did not choose a card")}");
            }
        }

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

        //private void EnableImage(Image img)
        //{
        //    img.Height += POP;
        //    img.Width += POP;
        //}
        #endregion

        private void InitGame()
        {
            var vm = new GameViewModel(ChosenDeckSize, Username);
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

        private void Player_Won(object sender, EventArgs e)
        {
            MessageBox.Show($"{_gameViewModel.Winner.PlayerName} has won the match!");

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

        private void btnPass_Click(object sender, RoutedEventArgs e)
        {
            if (_gameViewModel.BoutCount == 1 && _gameViewModel.HumanPlayer == _gameViewModel.Attacker)
            {
                MessageBox.Show("You must choose a card to play on the first bout!");
            }
            else
            {
                //txtPass.Text = "You Have Chosen Not to Play.";
                _gameViewModel.PlayerChoseCompletionSource.TrySetResult(true);
            }
        }

        private void btnDealNew_Click(object sender, RoutedEventArgs e)
        {
            var msgBoxResult = MessageBox.Show("Are you sure you want to start a new game?", "Start New Game?", MessageBoxButton.YesNo);

            if (msgBoxResult == MessageBoxResult.Yes) InitGame();
            Statistics.UpdateGame();
        }


    }
}
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardLib;
using CardLib.CardLib;
using CardUI;

namespace Client
{
    public class GameplayTest
    {
        private Deck<PlayingCard> playDeck;
        private Cards dicardPile;   // Where Discarded Cards Go
        private Cards inPlayPile;   // Where the attacking and defending cards go
        private Player[] players;

        private GameplayTest()
        {

        }

        public static void Play()
        {
            #region Deck and Discard Pile Init
            var deck = new Deck<PlayingCard>();

            var discardPile = new Cards();    

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

        }

        private static void DisplayPlayer(Player player1)
        {
            for (int i = 0; i < player1.Hand.Count; i++)
            {
                Logger.Log($"{player1.PlayerName}'s Card {i+1}: {player1.Hand.ElementAt(i)}");
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using CardLib;

namespace CardUI
{
    public class PlayingCard : Card
    {
        //Constant Declaration
        private const string CARD_PATH = @"pack://application:,,,/Resources/Cards/";
        private const string PACK_PATH = @"pack://application:,,,/Resources/Pack/";
        // TODO: Dynamic Card Back
        private string CURRENT_BACK = "gray_back.png";

        public BitmapImage PlayingCardImage { get; set; }

        public PlayingCard(Suit suit, Rank rank) : base(suit, rank)
        {
            PlayingCardImage = GetCardImage(suit, rank, Face.Down);
        }

        public PlayingCard(Suit suit, Rank rank, Face face) : base(suit, rank, face)
        {
            PlayingCardImage = GetCardImage(suit, rank, face);
        }

        // Class Methods
        public BitmapImage GetImage()
        {
            return PlayingCardImage;
        }

        // Private Methods
        private BitmapImage GetCardImage(Suit newSuit, Rank newRank, Face newFace)
        {

            if (newFace == Face.Down)
            {
                return GetImage(PACK_PATH + CURRENT_BACK);
            }
            
            return GetImage($"{CARD_PATH + Rank + Suit}.png");
        }

        private BitmapImage GetImage(string path)
        {
            BitmapImage card = new BitmapImage();
            card.BeginInit();
            card.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            card.EndInit();
            return card;
        }

    }
}
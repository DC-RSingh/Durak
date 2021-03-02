using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Client
{
    class PlayingCard
    {
            public static BitmapImage GetImage(string path)
            {
                BitmapImage card = new BitmapImage();
                card.BeginInit();
                card.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
                card.EndInit();
                return card;
            }
     }
    
}

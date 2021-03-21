using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CardLib;
using CardUI;

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
            DisplayHand();
        }
        //const string PACK_PATH = @"pack://application:,,,/Resources/Pack/";

        // Testing Displaying the Card
        private void DisplayHand()
        {
            // Create Face Up Card
            var test_card = new PlayingCard(Suit.Club, Rank.King, Face.Up);

            // TODO: Can do a lot with this, crop it and what not with CroppedBitMap
            // Refer to: https://www.c-sharpcorner.com/UploadFile/mahesh/using-xaml-image-in-wpf/
            // Create Image to Add to Form, set Dimensions, Source
            var dynamicImage = new Image { Width = 70, Height = 107, Source = test_card.GetImage() };

            // Add Image to Form
            //DurakRoot.Children.Add(dynamicImage);
        }
    }
}

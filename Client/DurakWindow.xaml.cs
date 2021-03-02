using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for DurakWindow.xaml
    /// </summary>
    public partial class DurakWindow : Window
    {
        //PictureBox[] cards;

        public DurakWindow()
        {
            InitializeComponent();
            DisplayHand();
        }
        const string PACK_PATH = @"pack://application:,,,/Resources/Pack/";
        private void DisplayHand()
        {

            PlayingCard.GetImage(PACK_PATH + "gray_back.png");
        }
    }
}

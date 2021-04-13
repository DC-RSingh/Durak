using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Client.ViewModels;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for AttackingView.xaml
    /// </summary>
    public partial class AttackingView : UserControl
    {
        public AttackingView()
        {
            InitializeComponent();
        }
        private void Attacking_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new AttackingView();
        }
    }
}

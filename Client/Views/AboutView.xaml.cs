/**
 * OOP 4200 - Final Project - Durak
 * 
 * AboutView.xaml.cs supports the AboutView.xaml. It display the about window.
 * 
 * @author      Raje Singh, Fleur Blanckaert, Gabriel Dietrich, Dalton Young
 * @version     1.0
 * @since       2021-03 
 */

using System.Windows.Controls;
using Client.ViewModels;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    public partial class AboutView : UserControl
    {
        /// <summary>
        /// Initializes the about view
        /// </summary>
        public AboutView()
        {
            InitializeComponent();
            this.DataContext = null;
        }

    }
}

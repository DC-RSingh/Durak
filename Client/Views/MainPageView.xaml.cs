using Client.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for MainPageView.xaml
    /// </summary>
    public partial class MainPageView : UserControl
    {
        public MainPageView()
        {
            InitializeComponent();
            this.DataContext = null;
        }
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            UserView userView = new UserView();
            DataContext = userView;
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutView aboutView = new AboutView();
            DataContext = aboutView;
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            HelpView helpWindow = new HelpView();
            helpWindow.Show();
            //System.Diagnostics.Process.Start("https://www.pagat.com/beating/podkidnoy_durak.html");
        }

        private void Statistics_Click(object sender, RoutedEventArgs e)
        {
            StatisticsView statisticsView = new StatisticsView();
            DataContext = statisticsView;
        }
    }
}

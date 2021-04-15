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

        private void GameOptions_Click(object sender, RoutedEventArgs e)
        {
            GameSettingsView gameSettingsView = new GameSettingsView();
            DataContext = gameSettingsView;
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            MainHelpView helpWindow = new MainHelpView();
            DataContext = helpWindow;
        }

        private void Statistics_Click(object sender, RoutedEventArgs e)
        {
            StatisticsView statisticsView = new StatisticsView();
            DataContext = statisticsView;
        }
    }
}

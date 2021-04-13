using System.IO;
using System.Windows;
using System.Windows.Controls;
using Client.ViewModels;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : UserControl
    {
        public UserView()
        {
            InitializeComponent();
            this.DataContext = null;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            //string fileName = @"log/pregame_settings.txt";
            var pwd = Directory.GetCurrentDirectory();
            var fileName = System.IO.Path.Combine(pwd, "GameLog");
            var textFile = System.IO.Path.Combine(fileName, "statistics.txt");
            string[] option = File.ReadAllLines(textFile);

            //Check if the file exists
            if (!Directory.Exists(fileName))
            {
                Directory.CreateDirectory(fileName);
            }
            var name = option[0];
            var wins = option[1];
            var losses = option[2];
            var ties = option[3];
            var total = option[4];

            // Create the file and use streamWriter to write text to it.
            //If the file existence is not check, this will overwrite said file.
            //Use the using block so the file can close and vairable disposed correctly
            if (File.Exists(textFile))
            {
                using (StreamWriter writer = File.CreateText(textFile))
                {
                    writer.WriteLine(tbName.Text);
                    writer.WriteLine(wins);
                    writer.WriteLine(losses);
                    writer.WriteLine(ties);
                    writer.WriteLine(total);
                }
            }
                GameView gameView = new GameView(playerName: tbName.Text);
                DataContext = gameView;
        }
    }
}

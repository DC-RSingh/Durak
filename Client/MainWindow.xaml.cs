using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            ConsoleManager.Show();  // Show the Console
            var icoPath = Directory.GetParent(Environment.CurrentDirectory).
                Parent.
                FullName; // Really Scuffed Icon Path Retrieval
            ConsoleManager.SetConsoleIcon(new Icon($"{icoPath}/icon.ico"));
            InitializeComponent();
            Logger.Start(); // Start Logging
        }
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        { 
            DurakWindow gameWindow = new DurakWindow();
            gameWindow.ShowDialog();
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This is the final project for OOP4200, group 4. We have created a working version of the card game 'Durak'." +
                " It was created by Gabriel Dietrich, Fleur Blanckaert, Raje Singh, and Dalton Young. ");
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.pagat.com/beating/podkidnoy_durak.html");
        }


        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

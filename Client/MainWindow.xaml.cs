using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Logging;


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
            Logger.Start(); // Start Logging
            try
            {
                var icoPath = Directory.GetParent(Environment.CurrentDirectory).
                    Parent?.
                    FullName; // Really Scuffed Icon Path Retrieval
                ConsoleManager.SetConsoleIcon(new Icon($"{icoPath}/icon.ico"));
            } catch
            {
                Logger.Log("Could not find Icon!", LoggingLevel.Warn, typeof(Directory));// Ignore exception
            }
            
            InitializeComponent();
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

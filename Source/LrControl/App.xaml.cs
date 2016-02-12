using System;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace micdah.LrControl
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = new MainWindow
            {
                //WindowState = WindowState.Minimized
            };

            mainWindow.Show();
        }
    }
}

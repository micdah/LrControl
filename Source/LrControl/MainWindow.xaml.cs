using System;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace micdah.LrControl
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs args)
        {
            try
            {
                var sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sendSocket.Connect("localhost", 52008);

                try
                {
                    sendSocket.Send(Encoding.UTF8.GetBytes("This is a test\n"));
                }
                finally
                {
                    if (sendSocket.IsBound)
                    {
                        sendSocket.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
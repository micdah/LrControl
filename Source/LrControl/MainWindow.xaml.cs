using System;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Input;
using LrControlApi;

namespace micdah.LrControl
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Communicator _communicator;

        public MainWindow()
        {
            InitializeComponent();

            _communicator = new Communicator(52008, 52009);
            _communicator.Open();
        }
        
        private void ButtonBase_OnClick(object sender, RoutedEventArgs args)
        {
            SendMessage();
        }

        private void Message_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }

        private void SendMessage()
        {
            var message = Message.Text;

            String response;
            if (_communicator.SendMessage(message, out response))
            {
                Response.Text = response;
            }
            else
            {
                Response.Text = "An error occurred while trying to send message";
            }

            Message.Text = String.Empty;
        }
    }
}
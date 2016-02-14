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

            var response = _communicator.SendMessage(message);

            Message.Text = String.Empty;
            Response.Text = response;
        }
    }
}
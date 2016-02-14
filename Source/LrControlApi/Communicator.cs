using System;
using System.Net.Sockets;
using System.Text;
using log4net;

namespace LrControlApi
{
    public class Communicator
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (Communicator));
        private static readonly byte EndOfLineByte = Encoding.UTF8.GetBytes("\n")[0];

        private readonly int _sendPort;
        private readonly int _receivePort;
        
        private Socket _sendSocket;
        private Socket _receiveSocket;

        public Communicator(int sendPort, int receivePort)
        {
            _sendPort = sendPort;
            _receivePort = receivePort;
        }

        public bool Open()
        {
            if (_sendSocket != null && _receiveSocket != null)
                throw new InvalidOperationException("Already connected, Close first");

            try
            {
                _sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _sendSocket.Connect("localhost", _sendPort);

                _receiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _receiveSocket.Connect("localhost", _receivePort);
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

        public void Close()
        {
            if (_sendSocket != null)
            {
                if (_sendSocket.Connected)
                    _sendSocket.Close();
                _sendSocket.Dispose();
                _sendSocket = null;
            }

            if (_receiveSocket != null)
            {
                if (_receiveSocket.Connected)
                    _receiveSocket.Close();
                _receiveSocket.Dispose();
                _receiveSocket = null;
            }
        }

        public bool IsConnected => _sendSocket != null && _receiveSocket != null;

        public string SendMessage(string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (message.Contains("\n"))
                throw new ArgumentException("Must not contain newline characters", nameof(message));

            if (!IsConnected) return null;

            try
            {
                FlushMessages();

                var messageBytes = Encoding.UTF8.GetBytes(message + "\n");
                _sendSocket.Send(messageBytes);

                return ReceiveMessage();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        private void FlushMessages()
        {
            var available = _receiveSocket.Available;
            if (available <= 0) return;

            var bytes = new byte[available];
            _receiveSocket.Receive(bytes);
        }

        public string ReceiveMessage()
        {
            if (_receiveSocket == null)
                throw new InvalidOperationException("Canont receive message, there is no connection");

            var message = new StringBuilder();
            var bytes = new byte[1024];

            while (true)
            {
                var receivedBytes = _receiveSocket.Receive(bytes);
                if (receivedBytes <= 0) continue;

                var part = Encoding.UTF8.GetString(bytes, 0, receivedBytes);
                message.Append(part);

                if (bytes[receivedBytes - 1] == EndOfLineByte)
                {
                    var str = message.ToString();
                    return str.Substring(0, str.Length - 1);    // Remove EndOfLine character
                }
            }
        }
    }
}
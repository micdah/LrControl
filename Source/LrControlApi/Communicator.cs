using System;
using System.Net.Sockets;
using System.Text;
using log4net;

namespace LrControlApi
{
    public class Communicator
    {
        private const string HostName = "localhost";
        private static readonly ILog Log = LogManager.GetLogger(typeof (Communicator));
        private static readonly byte EndOfLineByte = Encoding.UTF8.GetBytes("\n")[0];
        private readonly int _receivePort;

        private readonly int _sendPort;
        private Socket _receiveSocket;

        private Socket _sendSocket;

        public Communicator(int sendPort, int receivePort)
        {
            _sendPort = sendPort;
            _receivePort = receivePort;
        }

        public bool IsConnected => _sendSocket != null && _receiveSocket != null;

        public bool Open()
        {
            if (_sendSocket != null && _receiveSocket != null)
                throw new InvalidOperationException("Already connected, Close first");

            Log.Debug($"Opening send (port {_sendPort}) and receive (port {_receivePort}) sockets");

            _sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                SendTimeout       = 500,
                ReceiveTimeout    = 500,
                SendBufferSize    = 8192,
                ReceiveBufferSize = 8192,
            };
            _receiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                SendTimeout       = 500,
                ReceiveTimeout    = 500,
                SendBufferSize    = 8192,
                ReceiveBufferSize = 8192,
            };

            try
            {
                _sendSocket.Connect(HostName, _sendPort);
                _receiveSocket.Connect(HostName, _receivePort);

                return true;
            }
            catch (SocketException e)
            {
                Log.Error("An error occurred while trying to open connection to LrControlPlugin", e);
                return false;
            }
        }

        public void Close()
        {
            Close(ref _sendSocket);
            Close(ref _receiveSocket);
        }

        public bool SendMessage(string message, out string response)
        {
            if (!IsConnected) throw new InvalidOperationException("Cannot send a message, when not open");

            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (message.Contains("\n"))
                throw new ArgumentException("Must not contain newline characters", nameof(message));

            try
            {
                if (!FlushMessages())
                {
                    response = null;
                    return false;
                }

                var messageBytes = Encoding.UTF8.GetBytes(message + "\n");
                _sendSocket.Send(messageBytes);

                return ReceiveMessage(out response);
            }
            catch (SocketException e)
            {
                Log.Error("An error occurred while trying to send a message", e);
                response = null;
                return false;
            }
        }

        public bool ReceiveMessage(out string response)
        {
            if (_receiveSocket == null)
                throw new InvalidOperationException("Canont receive message, there is no connection");

            var message = new StringBuilder();
            var bytes = new byte[1024];

            try
            {
                while (true)
                {
                    var receivedBytes = _receiveSocket.Receive(bytes);
                    if (receivedBytes <= 0) continue;

                    var part = Encoding.UTF8.GetString(bytes, 0, receivedBytes);
                    message.Append(part);

                    if (bytes[receivedBytes - 1] == EndOfLineByte)
                    {
                        var str = message.ToString();
                        response = str.Substring(0, str.Length - 1); // Remove EndOfLine character
                        return true;
                    }
                }
            }
            catch (SocketException e)
            {
                Log.Error("An error occurred while trying to receive a message", e);

                response = null;
                return false;
            }
        }

        private bool FlushMessages()
        {
            try
            {
                var available = _receiveSocket.Available;
                if (available <= 0) return true;

                var bytes = new byte[available];
                _receiveSocket.Receive(bytes);

                return true;
            }
            catch (SocketException e)
            {
                Log.Error("An errorr occurred while trying to flush messages on the receive port", e);
                Reconnect();
                return false;
            }
        }

        private void Reconnect()
        {
            Reconnect(ref _sendSocket, _sendPort);
            Reconnect(ref _receiveSocket, _receivePort);
        }

        private static void Reconnect(ref Socket socket, int port)
        {
            if (socket.Connected)
            {
                try
                {
                    socket.Disconnect(true);
                }
                catch (Exception e)
                {
                    Log.Error("An error occurred while trying to disconect send socket, cannot reuse", e);

                    socket.Dispose();
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }
            }
            socket.Connect(HostName, port);
        }

        private static void Close(ref Socket socket)
        {
            try
            {
                if (socket.Connected)
                {
                    socket.Disconnect(false);
                }
                socket.Close();
            }
            catch (Exception e)
            {
                Log.Error("An error occurred while trying to close socket", e);
            }
            finally
            {
                socket.Dispose();
            }
            socket = null;
        }
    }
}
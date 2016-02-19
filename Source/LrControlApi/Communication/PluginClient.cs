using System;
using System.Collections.Concurrent;
using System.Threading;

namespace micdah.LrControlApi.Communication
{
    public delegate void ConnectionStatusHandler(bool connected);

    internal class PluginClient
    {
        private const string HostName = "localhost";
        private readonly BlockingCollection<string> _receivedMessages = new BlockingCollection<string>();
        private readonly SocketWrapper _receiveSocket;
        private readonly object _sendLock = new object();
        private readonly SocketWrapper _sendSocket;
        private bool _lastConnectionStatus;

        public PluginClient(int sendPort, int receivePort)
        {
            _sendSocket                    = new SocketWrapper(HostName, sendPort, false);
            _receiveSocket                 = new SocketWrapper(HostName, receivePort, true);
            _sendSocket.LostConnection    += () => _receiveSocket.Reconnect(false);
            _receiveSocket.LostConnection += () => _sendSocket.Reconnect(false);
            _sendSocket.Connected         += ConnectedHandler;
            _receiveSocket.Connected      += ConnectedHandler;
            _receiveSocket.MessageReceived += ReceiveSocketOnMessageReceived;
        }
        
        public bool IsConnected => _sendSocket.IsConnected && _receiveSocket.IsConnected;

        public event ConnectionStatusHandler ConnectionStatus;

        public bool Open()
        {
            if (IsConnected)
                throw new InvalidOperationException("Already connected, Close first");

            if (!_sendSocket.Open())
            {
                return false;
            }
            if (!_receiveSocket.Open())
            {
                _sendSocket.Close();
                return false;
            }

            return true;
        }

        public void Close()
        {
            if (_sendSocket.IsOpen)
                _sendSocket.Close();

            if (_receiveSocket.IsOpen)
                _receiveSocket.Close();
        }

        public bool SendMessage(string message, out string response)
        {
            if (!IsConnected) throw new InvalidOperationException("Cannot send a message, when not open");

            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (message.Contains("\n"))
                throw new ArgumentException("Must not contain newline characters", nameof(message));

            lock (_sendLock)
            {
                if (!_sendSocket.Send(message))
                {
                    response = null;
                    return false;
                }

                _receivedMessages.TryTake(out response, SocketWrapper.SocketTimeout);

                return _receiveSocket.Receive(out response, EndOfLineByte);
            }
        }

        private void ConnectedHandler()
        {
            var isConnected = IsConnected;
            if (_lastConnectionStatus == isConnected) return;

            lock (this)
            {
                if (_lastConnectionStatus == isConnected) return;

                _lastConnectionStatus = isConnected;
                OnConnectionStatus(IsConnected);
            }
        }

        private void ReceiveSocketOnMessageReceived(string message)
        {
            _receivedMessages.Add(message);
        }

        private void OnConnectionStatus(bool connected)
        {
            ThreadPool.QueueUserWorkItem(state => ConnectionStatus?.Invoke(connected));
        }
    }
}
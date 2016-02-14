using System;
using System.Text;
using System.Threading;
using log4net;

namespace LrControlApi.Communication
{
    public delegate void ConnectionStatusHandler(bool connected);

    internal class PluginClient
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (PluginClient));

        private const string HostName = "localhost";
        private static readonly byte EndOfLineByte = Encoding.UTF8.GetBytes("\n")[0];
        private readonly SocketWrapper _sendSocket;
        private readonly SocketWrapper _receiveSocket;
        private bool _isInsideLostConnectionHandler;
        
        public PluginClient(int sendPort, int receivePort)
        {
            _sendSocket                    = new SocketWrapper(HostName, sendPort);
            _receiveSocket                 = new SocketWrapper(HostName, receivePort);
            _sendSocket.LostConnection    += () => LostConnectionHandler(_sendSocket);
            _receiveSocket.LostConnection += () => LostConnectionHandler(_receiveSocket);
            _sendSocket.Connected         += ConnectedHandler;
            _receiveSocket.Connected      += ConnectedHandler;
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

            if (!_receiveSocket.Flush())
            {
                response = null;
                return false;
            }

            if (!_sendSocket.Send(message))
            {
                response = null;
                return false;
            }

            return _receiveSocket.Receive(out response, EndOfLineByte);
        }

        /// <summary>
        ///     When one socket looses connection, the other probably will also need a reconnect
        /// </summary>
        /// <param name="socket"></param>
        private void LostConnectionHandler(SocketWrapper socket)
        {
            if (_isInsideLostConnectionHandler) return;

            _isInsideLostConnectionHandler = true;
            try
            {
                if (socket == _receiveSocket) 
                    _sendSocket.Reconnect();
                else if (socket == _sendSocket)
                    _receiveSocket.Reconnect();
            }
            finally
            {
                _isInsideLostConnectionHandler = false;
            }
        }

        private bool _lastConnectionStatus;

        private void ConnectedHandler()
        {
            var isConnected = IsConnected;
            if (_lastConnectionStatus != isConnected)
            {
                lock (this)
                {
                    if (_lastConnectionStatus != isConnected)
                    {
                        _lastConnectionStatus = isConnected;
                        OnConnectionStatus(IsConnected);
                    }
                }
            }
        }

        protected virtual void OnConnectionStatus(bool connected)
        {
            ThreadPool.QueueUserWorkItem(state => ConnectionStatus?.Invoke(connected));
        }
    }
}
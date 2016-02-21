using System;
using System.Collections.Concurrent;
using System.Threading;
using micdah.LrControlApi.Modules.LrApplicationView;

namespace micdah.LrControlApi.Communication
{
    internal delegate void ChangeMessageHandler(string parameterName);

    internal delegate void ModuleMessageHandler(string moduleName);

    internal class PluginClient
    {
        private const string HostName = "localhost";
        private const string Changed = "Changed:";
        private const string Module = "Module:";

        private readonly BlockingCollection<string> _receivedMessages = new BlockingCollection<string>();
        private readonly SocketWrapper _receiveSocket;
        private readonly SocketWrapper _sendSocket;
        private readonly object _sendLock = new object();
        private readonly object _connectionStatusLock = new object();

        private bool _lastConnectionStatus;

        public PluginClient(int sendPort, int receivePort)
        {
            _sendSocket                     = new SocketWrapper(HostName, sendPort, false);
            _receiveSocket                  = new SocketWrapper(HostName, receivePort, true);
            _sendSocket.Connection         += connected => SocketConnectionHandler(_sendSocket, connected);
            _receiveSocket.Connection      += connected => SocketConnectionHandler(_receiveSocket, connected);
            _receiveSocket.MessageReceived += ReceiveSocketOnMessageReceived;
        }

        public bool IsConnected => _sendSocket.IsConnected && _receiveSocket.IsConnected;
        public event ConnectionHandler Connection;
        public event ChangeMessageHandler ChangeMessage;
        public event ModuleMessageHandler ModuleMessage;

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
                // Empty message queue
                if (_receivedMessages.Count > 0)
                {
                    string throwAway;
                    while (_receivedMessages.TryTake(out throwAway))
                    {
                    }
                }

                // Send message
                if (!_sendSocket.Send(message))
                {
                    response = null;
                    return false;
                }

                // Wait for response
                return _receivedMessages.TryTake(out response, SocketWrapper.SocketTimeout);
            }
        }

        private void SocketConnectionHandler(SocketWrapper socket, bool connected)
        {
            if (!connected)
            {
                if (socket == _sendSocket)
                {
                    _receiveSocket.Reconnect(false);
                }
                else
                {
                    _sendSocket.Reconnect(false);
                }
            }

            UpdateConnectionStatus();
        }

        private void ReceiveSocketOnMessageReceived(string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            if (message.StartsWith(Changed))
            {
                OnChangeMessage(message.Substring(Changed.Length));
            }
            else if (message.StartsWith(Module))
            {
                OnModuleMessage(message.Substring(Module.Length));
            }
            else
            {
                _receivedMessages.Add(message);
            }
        }
        
        private void UpdateConnectionStatus()
        {
            lock (_connectionStatusLock)
            {
                var isConnected = IsConnected;
                if (_lastConnectionStatus == isConnected) return;

                ThreadPool.QueueUserWorkItem(state => Connection?.Invoke((bool)state), isConnected);
                _lastConnectionStatus = isConnected;
            }
        }

        private void OnChangeMessage(string parameterName)
        {
            ThreadPool.QueueUserWorkItem(state => ChangeMessage?.Invoke((string) state), parameterName);
        }

        protected virtual void OnModuleMessage(string moduleName)
        {
            ThreadPool.QueueUserWorkItem(state => ModuleMessage?.Invoke((string)state), moduleName);
        }
    }
}
using System;
using System.Collections.Concurrent;
using System.Threading;
using micdah.LrControlApi.Common;

namespace micdah.LrControlApi.Communication
{
    internal delegate void ChangeMessageHandler(string parameterName);

    internal delegate void ModuleMessageHandler(string moduleName);

    internal class PluginClient
    {
        private const string HostName = "localhost";
        private const string ChangedToken = "Changed:";
        private const string ModuleToken = "Module:";
        private readonly StartStopThread _changeProcessingThread;

        private readonly BlockingCollection<string> _changeQueue = new BlockingCollection<string>();
        private readonly object _connectionStatusLock = new object();
        private readonly StartStopThread _moduleProcessingThread;
        private readonly BlockingCollection<string> _moduleQueue = new BlockingCollection<string>();
        private readonly BlockingCollection<string> _receivedMessages = new BlockingCollection<string>();
        private readonly SocketWrapper _receiveSocket;
        private readonly object _sendLock = new object();
        private readonly SocketWrapper _sendSocket;

        private bool _lastConnectionStatus;

        public PluginClient(int sendPort, int receivePort)
        {
            _sendSocket = new SocketWrapper(HostName, sendPort, false);
            _receiveSocket = new SocketWrapper(HostName, receivePort, true);
            _sendSocket.Connection += connected => SocketConnectionHandler(_sendSocket, connected);
            _receiveSocket.Connection += connected => SocketConnectionHandler(_receiveSocket, connected);
            _receiveSocket.MessageReceived += ReceiveSocketOnMessageReceived;

            _changeProcessingThread = new StartStopThread("PluginClient Change processing thread", stop =>
            {
                string param;
                if (_changeQueue.TryTake(out param, 100))
                {
                    ChangeMessage?.Invoke(param);
                }
            });

            _moduleProcessingThread = new StartStopThread("PluginClient Moduyle change processing thread", stop =>
            {
                string module;
                if (_moduleQueue.TryTake(out module, 100))
                {
                    ModuleMessage?.Invoke(module);
                }
            });
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

            _changeProcessingThread.Start();
            _moduleProcessingThread.Start();

            return true;
        }

        public void Close()
        {
            if (_sendSocket.IsOpen)
                _sendSocket.Close();

            if (_receiveSocket.IsOpen)
                _receiveSocket.Close();

            _changeProcessingThread.Stop();
            _moduleProcessingThread.Stop();
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

            if (message.StartsWith(ChangedToken))
            {
                _changeQueue.Add(message.Substring(ChangedToken.Length));
            }
            else if (message.StartsWith(ModuleToken))
            {
                _moduleQueue.Add(message.Substring(ModuleToken.Length));
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

                ThreadPool.QueueUserWorkItem(state => Connection?.Invoke((bool) state), isConnected);
                _lastConnectionStatus = isConnected;
            }
        }
    }
}
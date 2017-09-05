using System;
using System.Collections.Concurrent;
using System.Threading;
using LrControl.Api.Common;
using LrControl.Api.Communication.Sockets;
using Serilog;

namespace LrControl.Api.Communication
{
    internal delegate void ChangeMessageHandler(string parameterName);
    internal delegate void ModuleMessageHandler(string moduleName);
    
    internal class PluginClient
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<PluginClient>();
        private const string HostName = "localhost";
        private const string ChangedToken = "Changed:";
        private const string ModuleToken = "Module:";

        private readonly BlockingCollection<string> _inputQueue = new BlockingCollection<string>();
        private readonly BlockingCollection<PluginEvent> _eventQueue = new BlockingCollection<PluginEvent>();
        private readonly ProcessingThread _eventProcessingThread;
        private readonly SendSocket _sendSocket;
        private readonly ReceiveSocket _receiveSocket;
        private readonly object _connectionStatusLock = new object();
        private readonly object _sendLock = new object();
        

        private bool _lastConnectionStatus;

        public PluginClient(int sendPort, int receivePort)
        {
            _sendSocket = new SendSocket(HostName, sendPort);
            _receiveSocket = new ReceiveSocket(HostName, receivePort);
            _sendSocket.Connection += connected => SocketConnectionHandler(_sendSocket, connected);
            _receiveSocket.Connection += connected => SocketConnectionHandler(_receiveSocket, connected);
            _receiveSocket.MessageReceived += ReceiveSocketOnMessageReceived;

            _eventProcessingThread = new ProcessingThread("PluginClient Event Processing thread", EventProcessingIteration);
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

            _eventProcessingThread.Start();
            return true;
        }

        public void Close()
        {
            if (_sendSocket.IsOpen)
                _sendSocket.Close();

            if (_receiveSocket.IsOpen)
                _receiveSocket.Close();

            _eventProcessingThread.Stop();
            ClearQueue(_eventQueue);
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
                ClearQueue(_inputQueue);

                // Send message
                if (!_sendSocket.Send(message))
                {
                    response = null;
                    return false;
                }

                // Wait for response
                var success = _inputQueue.TryTake(out response, SocketBase.SocketTimeout);
                if (!success)
                    Log.Warning("Sent message '{Message}' but did not get any response within timeout of {SocketTimeout}", message, SocketBase.SocketTimeout);

                return success;
            }
        }

        private void EventProcessingIteration(RequestStopHandler requestStopHandler)
        {
            if (!_eventQueue.TryTake(out var pluginEvent, 100)) return;

            switch (pluginEvent.PluginEventType)
            {
                case PluginEventType.Change:
                    ChangeMessage?.Invoke(pluginEvent.Data);
                    break;
                case PluginEventType.Module:
                    ModuleMessage?.Invoke(pluginEvent.Data);
                    break;
                default:
                    Log.Error("Unknown event type received in {@Event}", pluginEvent);
                    break;
            }
        }

        private void SocketConnectionHandler(SocketBase socket, bool connected)
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
                _eventQueue.Add(new PluginEvent(PluginEventType.Change, message.Substring(ChangedToken.Length)));
            }
            else if (message.StartsWith(ModuleToken))
            {
                _eventQueue.Add(new PluginEvent(PluginEventType.Module, message.Substring(ModuleToken.Length)));
            }
            else
            {
                _inputQueue.Add(message);
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

        private static void ClearQueue<T>(BlockingCollection<T> queue)
        {
            if (queue.Count <= 0) return;

            Log.Warning("There are {Count} messages left in queue, emptying", queue.Count);

            while (queue.TryTake(out T item))
            {
                Log.Debug("Throwing away queue item {@Item}", item);
            }
        }
    }
}
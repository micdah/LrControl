using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using micdah.LrControlApi.Common;
using Serilog;

namespace micdah.LrControlApi.Communication
{
    public delegate void MessageHandler(string message);

    public delegate void ConnectionHandler(bool connected);

    internal class SocketWrapper : IDisposable
    {
        public const int SocketTimeout = 1000;
        private static readonly ILogger Log = Serilog.Log.Logger.ForContext<SocketWrapper>();
        private static readonly byte EndOfLineByte = Encoding.UTF8.GetBytes("\n")[0];

        private readonly string _hostName;
        private readonly int _port;
        private readonly byte[] _receiveBuffer = new byte[1024];
        private readonly bool _receiving;
        private readonly Stopwatch _stopwatch = new Stopwatch();

        private StartStopThread _receiveThread;
        private StartStopThread _reconnectThread;
        private Socket _socket;


        public SocketWrapper(string hostName, int port, bool receiving)
        {
            _hostName = hostName;
            _port = port;
            _receiving = receiving;
        }

        public bool IsOpen { get; private set; }
        public bool IsConnected { get; private set; }

        public void Dispose()
        {
            if (IsOpen)
            {
                Close();
            }
        }

        public event ConnectionHandler Connection;
        public event MessageHandler MessageReceived;

        public bool Open()
        {
            if (IsOpen)
                throw new InvalidOperationException("Cannot open, already open");

            // Try to create socket
            if (!TryCreateSocket())
                return false;

            _reconnectThread = new StartStopThread($"Socket Reconnect Thread (port {_port})", ReconnectIteration);

            if (_receiving)
            {
                _receiveThread = new StartStopThread($"Socket Receive Thread (port {_port})", ReceiveIteration);
            }

            // Initial state
            IsOpen = true;
            IsConnected = false;
            _reconnectThread.Start();
            if (_receiving)
                _receiveThread.Start();

            return true;
        }

        public void Close()
        {
            if (!IsOpen)
                throw new InvalidOperationException("Cannot close, is not open");

            _reconnectThread.Dispose();
            _reconnectThread = null;

            _receiveThread?.Dispose();
            _receiveThread = null;

            CloseSocket();

            IsOpen = false;
            IsConnected = false;

            Log.Debug("Socket connected to {HostName}:{Port} has been closed", _hostName, _port);
        }

        public void Reconnect(bool fireEvent = true)
        {
            if (!IsOpen)
                throw new InvalidOperationException("Canont reconnect, is not open");

            if (fireEvent) OnConnection(false);

            IsConnected = false;
            _reconnectThread.Start();
        }

        public bool Send(string message)
        {
            if (!IsOpen)
                throw new InvalidOperationException("Cannot send when not open");

            if (!IsConnected)
                return false;

            try
            {
                var bytes = Encoding.UTF8.GetBytes(message + "\n");
                _socket.Send(bytes);
                return true;
            }
            catch (SocketException e)
            {
                Log.Error(e, "Error while sending message '{Message}', reconnecting", message);
                Reconnect();

                return false;
            }
        }

        private bool Receive(out string message, byte stopByte)
        {
            if (!IsOpen)
                throw new InvalidOperationException("Canont receive whne not open");

            if (!IsConnected)
            {
                message = null;
                return false;
            }

            var inputBuffer = new StringBuilder();

            try
            {
                _stopwatch.Restart();

                while (true)
                {
                    var receivedBytes = _socket.Receive(_receiveBuffer);
                    if (receivedBytes == 0)
                    {
                        if (_stopwatch.ElapsedMilliseconds > SocketTimeout)
                        {
                            message = null;
                            return false;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    var part = Encoding.UTF8.GetString(_receiveBuffer, 0, receivedBytes);
                    inputBuffer.Append(part);

                    if (_receiveBuffer[receivedBytes - 1] == stopByte)
                    {
                        var str = inputBuffer.ToString();
                        message = str.Substring(0, str.Length - 1);
                        return true;
                    }
                }
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode != SocketError.TimedOut)
                {
                    Log.Error(e, "Error while receiving message, reconnecting");
                    Reconnect();
                }

                message = null;
                return false;
            }
            finally
            {
                _stopwatch.Stop();
            }
        }

        private void ReconnectIteration(RequestStopHandler stop)
        {
            // Stop trying to receive messages, while reconnecting
            _receiveThread?.Stop(true);

            Log.Debug("Trying to reconnect to {HostName}:{Port}", _hostName, _port);

            if (TryReconnect())
            {
                Log.Debug("Successfully reconnected to {HostName}:{Port}", _hostName, _port);

                IsConnected = true;
                OnConnection(true);

                // Start trying to receive messages again
                _receiveThread?.Start();

                // Stop reconnect loop, successfully reconnected
                stop();
            }
        }

        private void ReceiveIteration(RequestStopHandler stop)
        {
            string messages;
            if (Receive(out messages, EndOfLineByte))
            {
                // There can be multiple messages bundled into a single receive
                foreach (var message in messages.Split('\n'))
                {
                    OnMessageReceived(message);
                }
            }
        }

        private bool TryReconnect()
        {
            CloseSocket();

            if (!TryCreateSocket())
                return false;

            // Try to connect
            try
            {
                _socket.Connect(_hostName, _port);
                return true;
            }
            catch (SocketException e)
            {
                Log.Error(e, "Unable to connect to {HostName}:{Port}", _hostName, _port);
                return false;
            }
        }

        private bool TryCreateSocket()
        {
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                {
                    SendTimeout = SocketTimeout,
                    ReceiveTimeout = SocketTimeout,
                    SendBufferSize = 8192,
                    ReceiveBufferSize = 8192
                };

                return true;
            }
            catch (SocketException e)
            {
                Log.Error(e, "An error occurred while trying to allocate a socket");
                return false;
            }
        }

        private void CloseSocket()
        {
            try
            {
                if (_socket.Connected)
                {
                    _socket.Disconnect(false);
                }
                _socket.Close();
            }
            catch (Exception e)
            {
                Log.Error(e, "An error occurred while trying to close socket");
            }
            finally
            {
                _socket.Dispose();
            }
            _socket = null;
        }

        private void OnMessageReceived(string message)
        {
            MessageReceived?.Invoke(message);
        }

        private void OnConnection(bool connected)
        {
            Connection?.Invoke(connected);
        }
    }
}
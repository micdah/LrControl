using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using log4net;

namespace micdah.LrControlApi.Communication
{
    internal class SocketWrapper : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (SocketWrapper));
        private static readonly int SocketTimeout = 500;
        private readonly string _hostName;
        private readonly int _port;
        private readonly ManualResetEvent _reconnectEvent;
        private readonly ManualResetEvent _stopReconnectThreadEvent;
        private readonly ManualResetEvent _stopReconnectThreadFinishedEvent;
        private readonly Stopwatch _stopwatch;
        private readonly byte[] _receiveBuffer = new byte[1024];
        private Thread _reconnectThread;
        private Socket _socket;

        public SocketWrapper(string hostName, int port)
        {
            _hostName                         = hostName;
            _port                             = port;
            _reconnectEvent                   = new ManualResetEvent(false);
            _stopReconnectThreadEvent         = new ManualResetEvent(false);
            _stopReconnectThreadFinishedEvent = new ManualResetEvent(false);
            _stopwatch                        = new Stopwatch();
        }

        public bool IsOpen { get; private set; }

        public bool IsConnected { get; private set; }

        public event Action LostConnection;
        public event Action Connected;

        public bool Open()
        {
            if (IsOpen)
                throw new InvalidOperationException("Cannot open, already open");

            // Try to create socket
            if (!TryCreateSocket())
                return false;

            // Create reconnect thread
            _reconnectThread = new Thread(ReconnectThreadStart)
            {
                Name         = $"Socket Reconnect Thread (port {_port})",
                IsBackground = true
            };
            _reconnectThread.Start();

            // Initial state
            IsOpen = true;
            IsConnected = false;
            _reconnectEvent.Set();

            return true;
        }

        public void Close()
        {
            if (!IsOpen)
                throw new InvalidOperationException("Cannot close, is not open");

            StopReconnectThread();
            CloseSocket();

            IsOpen = false;
            IsConnected = false;

            Log.Debug($"Socket connected to {_hostName}:{_port} has been closed");
        }

        public void Dispose()
        {
            if (IsOpen)
            {
                Close();
            }
        }

        public void Reconnect()
        {
            if (!IsOpen)
                throw new InvalidOperationException("Canont reconnect, is not open");

            OnLostConnection();

            IsConnected = false;
            _reconnectEvent.Set();
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
                Log.Error("Unable to send message '{message}'", e);
                Reconnect();

                return false;
            }
        }

        public bool Receive(out string message, byte stopByte)
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
                    if (receivedBytes <= 0)
                    {
                        if (_stopwatch.ElapsedMilliseconds > SocketTimeout)
                        {
                            Log.Error($"Waited more than {SocketTimeout}, probably lost connection");
                            Reconnect();

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
                Log.Error("Unable to receive message", e);
                Reconnect();

                message = null;
                return false;
            }
            finally
            {
                _stopwatch.Stop();
            }
        }

        public bool Flush()
        {
            if (!IsOpen)
                throw new InvalidOperationException("Cannot flush when not open");

            if (!IsConnected)
                return false;

            try
            {
                var available = _socket.Available;
                if (available <= 0) return true;

                var bytes = new byte[available];
                _socket.Receive(bytes);

                return true;
            }
            catch (SocketException e)
            {
                Log.Error("Unable to flush", e);
                Reconnect();

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
                Log.Error("An error occurred while trying to close socket", e);
            }
            finally
            {
                _socket.Dispose();
            }
            _socket = null;
        }

        private void ReconnectThreadStart()
        {
            while (true)
            {
                // Wait until reconnect is requested
                _reconnectEvent.WaitOne();

                if (!_stopReconnectThreadEvent.WaitOne(0))
                {
                    Log.Debug($"Trying to reconnect to {_hostName}:{_port}");

                    if (TryReconnect())
                    {
                        // Stop reconnect loop, successfully reconnected
                        _reconnectEvent.Reset();

                        Log.Debug($"Successfully reconnected to {_hostName}:{_port}");
                        IsConnected = true;

                        OnConnected();
                    }
                }
                else
                {
                    // Asked to stop
                    break;
                }
            }

            _stopReconnectThreadFinishedEvent.Set();
        }

        private void StopReconnectThread()
        {
            // Stop reconnect thread
            _stopReconnectThreadEvent.Set();
            _reconnectEvent.Set();

            // Wait for thread to stop
            _stopReconnectThreadFinishedEvent.WaitOne();

            // Reset events
            _reconnectEvent.Reset();
            _stopReconnectThreadEvent.Reset();
            _stopReconnectThreadFinishedEvent.Reset();
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
                Log.Error($"Unable to connect to {_hostName}:{_port}", e);
                return false;
            }
        }

        private bool TryCreateSocket()
        {
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                {
                    SendTimeout       = SocketTimeout,
                    ReceiveTimeout    = SocketTimeout,
                    SendBufferSize    = 8192,
                    ReceiveBufferSize = 8192
                };

                return true;
            }
            catch (SocketException e)
            {
                Log.Error("An error occurred while trying to allocate a socket", e);
                return false;
            }
        }

        protected virtual void OnLostConnection()
        {
            LostConnection?.Invoke();
        }

        protected virtual void OnConnected()
        {
            Connected?.Invoke();
        }
    }
}
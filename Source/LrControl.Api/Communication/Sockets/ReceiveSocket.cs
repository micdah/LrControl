using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using LrControl.Api.Common;
using Serilog;

namespace LrControl.Api.Communication.Sockets
{
    public delegate void MessageHandler(string message);

    internal class ReceiveSocket : SocketBase
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<ReceiveSocket>();
        private static readonly byte EndOfLineByte = Encoding.UTF8.GetBytes("\n")[0];
        private readonly int _port;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private ProcessingThread _receiveThread;
        private readonly byte[] _receiveBuffer = new byte[1024];

        public ReceiveSocket(string hostName, int port) : base(hostName, port)
        {
            _port = port;
        }

        public event MessageHandler MessageReceived;

        protected override void OnOpen()
        {
            _receiveThread = new ProcessingThread($"Socket Receive Thread (port {_port})", ReceiveIteration);
        }

        protected override void OnClose()
        {
            _receiveThread?.Dispose();
            _receiveThread = null;
        }

        protected override void BeforeReconnect()
        {
            // Stop trying to receive messages, while reconnecting
            _receiveThread?.Stop(true);
        }

        protected override void AfterReconnect()
        {
            // Start trying to receive messages again
            _receiveThread?.Start();
        }

        private void ReceiveIteration(RequestStopHandler stop)
        {
            if (Receive(out var messages, EndOfLineByte))
            {
                // There can be multiple messages bundled into a single receive
                foreach (var message in messages.Split('\n'))
                {
                    OnMessageReceived(message);
                }
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
                    var receivedBytes = Socket.Receive(_receiveBuffer);
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
            catch (SocketException e) when (e.SocketErrorCode == SocketError.TimedOut)
            {
                Log.Debug("Socket timouet");
            }
            catch (SocketException e)
            {
                Log.Error(e, "Error while receiving message, reconnecting");
                Reconnect();
            }
            finally
            {
                _stopwatch.Stop();
            }

            message = null;
            return false;
        }

        private void OnMessageReceived(string message)
        {
            MessageReceived?.Invoke(message);
        }
    }
}
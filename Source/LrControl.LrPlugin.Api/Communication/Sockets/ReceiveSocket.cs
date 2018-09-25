using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using LrControl.LrPlugin.Api.Common;
using Serilog;

namespace LrControl.LrPlugin.Api.Communication.Sockets
{
    public delegate void MessageHandler(string message);

    internal class ReceiveSocket : SocketBase
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<ReceiveSocket>();
        private static readonly byte EndOfLineByte = Encoding.UTF8.GetBytes("\n")[0];
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly ProcessingThread _receiveThread;
        private readonly byte[] _receiveBuffer = new byte[1024];
        private readonly StringBuilder _inputBuffer = new StringBuilder();

        public ReceiveSocket(string hostName, int port) : base(hostName, port)
        {
            _receiveThread = new ProcessingThread($"Socket Receive Thread (port {port})", ReceiveIteration);
        }

        public event MessageHandler MessageReceived;

        protected override void BeforeReconnect()
        {
            // Stop trying to receive messages, while reconnecting
            _receiveThread.Stop();
        }

        protected override void AfterReconnect()
        {
            // Start trying to receive messages again
            _receiveThread.Start();
        }

        protected override void OnDispose()
        {
            _receiveThread.Dispose();
        }

        private void ReceiveIteration(CancellationToken cancellationToken, Action stop)
        {
            if (!Receive(out var messages, EndOfLineByte, cancellationToken, stop)) return;
            
            // There can be multiple messages bundled into a single receive
            foreach (var message in messages.Split('\n'))
            {
                MessageReceived?.Invoke(message);
            }
        }

        private bool Receive(out string message, byte stopByte, CancellationToken cancellationToken, Action stop)
        {
            void StopAndReconnect()
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    Log.Debug("Error occurred, trying to reconnect");
                    stop();
                    Reconnect();
                }
            }
            
            if (!IsOpen)
                throw new InvalidOperationException("Canont receive when not open");

            if (!IsConnected)
            {
                message = null;
                return false;
            }

            _inputBuffer.Clear();

            try
            {
                _stopwatch.Restart();

                while (!cancellationToken.IsCancellationRequested)
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
                    _inputBuffer.Append(part);

                    if (_receiveBuffer[receivedBytes - 1] == stopByte)
                    {
                        var str = _inputBuffer.ToString();
                        message = str.Substring(0, str.Length - 1);
                        return true;
                    }
                }
            }
            catch (SocketException e) when (e.SocketErrorCode == SocketError.TimedOut)
            {
                Log.Verbose(e, "Socket timeout");
            }
            catch (SocketException e) when (e.SocketErrorCode == SocketError.ConnectionRefused)
            {
                Log.Debug(e, "Connection refused, waiting {Duration} before trying again");
            }
            catch (SocketException e)
            {
                Log.Error(e, "Exception while receiving message");
                StopAndReconnect();
            }
            finally
            {
                _stopwatch.Stop();
            }

            message = null;
            return false;
        }
    }
}
using System;
using System.Net.Sockets;
using System.Text;
using Serilog;

namespace LrControl.Api.Communication.Sockets
{
    internal class SendSocket : SocketBase
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<SendSocket>();

        public SendSocket(string hostName, int port) : base(hostName, port)
        {
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
                Socket.Send(bytes);
                return true;
            }
            catch (SocketException e)
            {
                Log.Error(e, "Error while sending message '{Message}', reconnecting", message);
                Reconnect();

                return false;
            }
        }
    }
}

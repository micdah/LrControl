using System;
using System.Net.Sockets;
using System.Text;
using Serilog;

namespace LrControl.Api.Communication.Sockets
{
    internal class SendSocket : SocketBase
    {
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
            catch (SocketException e) when (e.SocketErrorCode == SocketError.TimedOut)
            {
                Log.Debug(e, "Socket timouet");
            }
            catch (SocketException e) when (e.SocketErrorCode == SocketError.ConnectionRefused)
            {
                Log.Debug(e, "Connection refused");
            }
            catch (SocketException e)
            {
                Log.Error(e, "Exception while sending message '{Message}'", message);
            }

            Log.Debug("Error occurred while sending, trying to reconnect");
            Reconnect();
            return false;
        }
    }
}

using System;
using System.Runtime.Serialization;
using LrControlApi.Communication;

namespace LrControlApi.LrControl
{
    internal class LrControl : ILrControl
    {
        private readonly MessageProtocol<LrControl> _messageProtocol;

        public LrControl(MessageProtocol<LrControl> messageProtocol)
        {
            _messageProtocol = messageProtocol;
        }

        public string GetApiVersion()
        {
            return _messageProtocol.SendWithResponse("getApiVersion");
        }
    }

    public class ApiException : Exception
    {
        public ApiException()
        {
        }

        public ApiException(string message) : base(message)
        {
        }

        public ApiException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
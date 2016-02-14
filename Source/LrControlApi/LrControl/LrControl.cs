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
}
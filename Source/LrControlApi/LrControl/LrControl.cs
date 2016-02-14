using LrControlApi.Common;
using LrControlApi.Communication;

namespace LrControlApi.LrControl
{
    internal class LrControl : ModuleBase<LrControl>, ILrControl
    {
        public LrControl(MessageProtocol<LrControl> messageProtocol) : base(messageProtocol)
        {
        }

        public string GetApiVersion()
        {
            return InvokeWithResult("getApiVersion");
        }
    }
}
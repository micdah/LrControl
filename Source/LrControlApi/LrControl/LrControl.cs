using LrControlApi.Common;
using LrControlApi.Communication;

namespace LrControlApi.LrControl
{
    internal class LrControl : ModuleBase<LrControl>, ILrControl
    {
        public LrControl(MessageProtocol<LrControl> messageProtocol) : base(messageProtocol)
        {
        }

        public bool GetApiVersion(out string apiVersion)
        {
            return Invoke(out apiVersion, nameof(GetApiVersion));
        }
    }
}
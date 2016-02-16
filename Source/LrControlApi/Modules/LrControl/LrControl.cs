using micdah.LrControlApi.Common;
using micdah.LrControlApi.Communication;

namespace micdah.LrControlApi.Modules.LrControl
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
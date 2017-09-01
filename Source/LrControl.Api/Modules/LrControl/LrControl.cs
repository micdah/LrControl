using LrControl.Api.Common;
using LrControl.Api.Communication;

namespace LrControl.Api.Modules.LrControl
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
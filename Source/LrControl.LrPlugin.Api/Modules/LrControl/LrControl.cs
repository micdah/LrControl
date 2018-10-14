using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Communication;

namespace LrControl.LrPlugin.Api.Modules.LrControl
{
    internal class LrControl : ModuleBase, ILrControl
    {
        public LrControl(MessageProtocol messageProtocol) : base(messageProtocol)
        {
        }

        public bool GetApiVersion(out string apiVersion)
        {
            return Invoke(out apiVersion, nameof(GetApiVersion));
        }
    }
}
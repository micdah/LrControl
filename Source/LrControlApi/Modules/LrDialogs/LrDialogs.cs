using micdah.LrControlApi.Common;
using micdah.LrControlApi.Communication;

namespace micdah.LrControlApi.Modules.LrDialogs
{
    internal class LrDialogs : ModuleBase<LrDialogs>, ILrDialogs
    {
        public LrDialogs(MessageProtocol<LrDialogs> messageProtocol) : base(messageProtocol)
        {
        }

        public bool Confirm(out ConfirmResult confirmResult, string message, string info, string actionVerb, string cancelVerb,
            string otherVerb = null)
        {
            return Invoke(out confirmResult, nameof(Confirm), message, info, actionVerb, cancelVerb, otherVerb);
        }

        public bool Message(string message, string info = null, DialogStyle style = null)
        {
            return Invoke(nameof(message), info, style);
        }

        public bool ShowBezel(string message, double? fadeDelay = null)
        {
            return Invoke(nameof(ShowBezel), message, fadeDelay);
        }

        public bool ShowError(string errorString)
        {
            return Invoke(nameof(ShowError), errorString);
        }
    }
}
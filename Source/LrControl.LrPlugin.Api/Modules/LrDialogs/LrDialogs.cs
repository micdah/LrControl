using LrControl.LrPlugin.Api.Common;
using LrControl.LrPlugin.Api.Communication;

namespace LrControl.LrPlugin.Api.Modules.LrDialogs
{
    internal class LrDialogs : ModuleBase<LrDialogs>, ILrDialogs
    {
        public LrDialogs(MessageProtocol<LrDialogs> messageProtocol) : base(messageProtocol)
        {
        }

        public bool Confirm(out ConfirmResult confirmResult, string message, string info, string actionVerb, string cancelVerb,
            string otherVerb = null)
        {
            if (Invoke(out string result, nameof(Confirm), message, info, actionVerb, cancelVerb, otherVerb))
            {
                confirmResult = ConfirmResult.GetEnumForValue(result);
                return true;
            }
            confirmResult = null;
            return false;
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
using LrControl.Api.Common;

namespace LrControl.Api.Modules.LrDialogs
{
    public class ConfirmResult : ClassEnum<string, ConfirmResult>
    {
        public static readonly ConfirmResult Ok     = new ConfirmResult("ok", "Ok");
        public static readonly ConfirmResult Cancel = new ConfirmResult("cancel", "Cancel");
        public static readonly ConfirmResult Other  = new ConfirmResult("other", "Other");

        
        private ConfirmResult(string value, string name) : base(value, name)
        {
        }
    }
}
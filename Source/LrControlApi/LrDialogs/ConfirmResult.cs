using LrControlApi.Common;

namespace LrControlApi.LrDialogs
{
    public class ConfirmResult : ClassEnum<string, ConfirmResult>
    {
        public static readonly ConfirmResult Ok     = new ConfirmResult("Ok", "ok");
        public static readonly ConfirmResult Cancel = new ConfirmResult("Cancel", "cancel");
        public static readonly ConfirmResult Other  = new ConfirmResult("Other", "other");


        private ConfirmResult(string name, string value) : base(name, value)
        {
        }
    }
}
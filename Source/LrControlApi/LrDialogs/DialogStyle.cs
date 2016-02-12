using LrControlApi.Common;

namespace LrControlApi.LrDialogs
{
    public class DialogStyle : ClassEnum<string, DialogStyle>
    {
        public static readonly DialogStyle Info     = new DialogStyle("Info", "info");
        public static readonly DialogStyle Warning  = new DialogStyle("Warning", "warning");
        public static readonly DialogStyle Critical = new DialogStyle("Critical", "critical");

        private DialogStyle(string name, string value) : base(name, value)
        {
        }
    }
}
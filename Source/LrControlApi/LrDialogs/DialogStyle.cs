using LrControlApi.Common;

namespace LrControlApi.LrDialogs
{
    public class DialogStyle : ClassEnum<string, DialogStyle>
    {
        public static readonly DialogStyle Info     = new DialogStyle("info", "Info");
        public static readonly DialogStyle Warning  = new DialogStyle("warning", "Warning");
        public static readonly DialogStyle Critical = new DialogStyle("critical", "Critical");

        static DialogStyle()
        {
            AddEnums(Info,Warning,Critical);
        }

        private DialogStyle(string value, string name) : base(value, name)
        {
        }
    }
}
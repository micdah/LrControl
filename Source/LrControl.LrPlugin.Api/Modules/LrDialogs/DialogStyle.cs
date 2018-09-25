using LrControl.LrPlugin.Api.Common;

namespace LrControl.LrPlugin.Api.Modules.LrDialogs
{
    public class DialogStyle : ClassEnum<string, DialogStyle>
    {
        public static readonly DialogStyle Info     = new DialogStyle("info", "Info");
        public static readonly DialogStyle Warning  = new DialogStyle("warning", "Warning");
        public static readonly DialogStyle Critical = new DialogStyle("critical", "Critical");

        
        private DialogStyle(string value, string name) : base(value, name)
        {
        }
    }
}
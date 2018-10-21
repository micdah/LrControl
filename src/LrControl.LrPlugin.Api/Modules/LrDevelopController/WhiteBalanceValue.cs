using LrControl.LrPlugin.Api.Common;

namespace LrControl.LrPlugin.Api.Modules.LrDevelopController
{
    public class WhiteBalanceValue : Enumeration<WhiteBalanceValue,string>
    {
        public static readonly WhiteBalanceValue AsShot      = new WhiteBalanceValue("As Shot");
        public static readonly WhiteBalanceValue Auto        = new WhiteBalanceValue("Auto");
        public static readonly WhiteBalanceValue Cloudy      = new WhiteBalanceValue("Cloudy");
        public static readonly WhiteBalanceValue Daylight    = new WhiteBalanceValue("Daylight");
        public static readonly WhiteBalanceValue Flash       = new WhiteBalanceValue("Flash");
        public static readonly WhiteBalanceValue Flourescent = new WhiteBalanceValue("Flourescent");
        public static readonly WhiteBalanceValue Shade       = new WhiteBalanceValue("Shade");
        public static readonly WhiteBalanceValue Tungsten    = new WhiteBalanceValue("Tungsten");

        private WhiteBalanceValue(string name) : base(name, name)
        {
        }
    }
}
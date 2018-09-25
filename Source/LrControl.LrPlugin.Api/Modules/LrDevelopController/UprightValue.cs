using LrControl.LrPlugin.Api.Common;

namespace LrControl.LrPlugin.Api.Modules.LrDevelopController
{
    public class UprightValue : Enumeration<UprightValue,int>
    {
        public static readonly UprightValue Off      = new UprightValue(0, "Off");
        public static readonly UprightValue Auto     = new UprightValue(1, "Auto");
        public static readonly UprightValue Full     = new UprightValue(2, "Full");
        public static readonly UprightValue Level    = new UprightValue(3, "Level");
        public static readonly UprightValue Vertical = new UprightValue(4, "Vertical");

        
        private UprightValue(int value, string name) : base(value, name)
        {
        }
    }
}